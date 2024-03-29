using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewPlayer : PhysicsObject
{
    [Header("Attributes")]
    public float attackPower = 10;
    [SerializeField] private float fallForgiveness = 1; // How much time the players have as "forgiveness" when falling from ledges. This way they can still jump even when they are not grounded.
    [SerializeField] private float fallForgivenessCounter;
    private bool frozen = false;
    [SerializeField] public bool isVulnerable = true; // Set if the player can be hurt or not.
    [SerializeField] private float jumpPower = 12;
    [SerializeField] private float maxSpeed = 8;

    [Header("Inventory/UI")]
    public int coinsCollected = 0;
    public float health = 50;
    [SerializeField] private float maxHealth = 100;

    [Header("References")]
    [SerializeField] private Animator animator;
    private AnimatorFunctions animatorFunctions;
    [SerializeField] private GameObject attackBox;
    public CameraEffects cameraEffects;
    // Default/empty inventory item slot sprite.
    [SerializeField] private Sprite inventoryBlankSprite;
    // The Dictionary stores all inventory items (key + value).
    public Dictionary<string, Sprite> inventory = new Dictionary<string, Sprite>();
    private Vector2 healthBarOriginalSize;
    // Health bar reference to be used while updating the UI.
    private RectTransform healthBarRect;
    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;
    public AudioSource ambianceAudioSource;

    // Singleton instantiation.
    private static NewPlayer instance;
    public static NewPlayer Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<NewPlayer>();
            return instance;
        }
    }

    // Happens BEFORE Start().
    private void Awake()
    {
        // If the original player is here, the scene player is self-destroyed.
        if (GameObject.Find("OriginalPlayer"))
            Destroy(gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.name = "OriginalPlayer";

        animatorFunctions = gameObject.GetComponent<AnimatorFunctions>();

        healthBarRect = GameManager.Instance.healthBar.rectTransform;
        healthBarOriginalSize = healthBarRect.sizeDelta;
        UpdateUI();
        SetSpawnPosition();
    }

    void Update()
    {
        if (frozen)
            return;

        if (!grounded)
            fallForgivenessCounter += Time.deltaTime;
        else
            fallForgivenessCounter = 0;

        if (Input.GetButtonDown("Jump") && fallForgivenessCounter < fallForgiveness)
            Jump();

        targetVelocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeed, 0);

        // Flip the player's localScale.x if move speed is great than .01 or less than -.01.
        if (targetVelocity.x < -.5)
            transform.localScale = new Vector2(-1, 1);
        else if (targetVelocity.x > .5)
            transform.localScale = new Vector2(1, 1);

        // Activate the attackBox when pressing Fire1 key.
        if (Input.GetButtonDown("Fire1"))
            animator.SetTrigger("attack");

        if (health <= 0)
            StartCoroutine(Die());

        // Set each animator parameters.
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        animator.SetFloat("velocityY", velocity.y);
        animator.SetBool("grounded", grounded);
        animator.SetFloat("attackDirectionY", Input.GetAxis("Vertical"));
    }

    private void Jump()
    {
        animatorFunctions.PlaySound("jump");
        animatorFunctions.EmitParticles("footsteps");
        velocity.y = jumpPower;

        grounded = false;
        fallForgivenessCounter = fallForgiveness; // Prevent double jump.
    }

    private IEnumerator InvulnerableCoroutine(float invulnerableTime)
    {
        NewPlayer.Instance.isVulnerable = false;
        yield return new WaitForSecondsRealtime(invulnerableTime);
        NewPlayer.Instance.isVulnerable = true;
    }

    public void BecomeInvulnerable(float invulnerableTime)
    {
        StartCoroutine(InvulnerableCoroutine(invulnerableTime));  // Become invulnerable for 0.2s when hurting an enemy.
    }

    // Player hurt.
    public void Hurt(float hurtAmount)
    {
        if (!isVulnerable)
            return;

        StartCoroutine(FreezeEffect(.2f, .7f));
        cameraEffects.Shake(2, .1f);

        BecomeInvulnerable(1f); // Become invulnerable for 1s after being hurted.
        animator.SetTrigger("hurt");
        health -= hurtAmount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        float healthPercentage;
        if (health <= maxHealth)
            healthPercentage = health / maxHealth;
        else
        {
            // Avoid health being greater than maxHealth.
            health = maxHealth;
            healthPercentage = 1;
        }
        healthBarRect.sizeDelta = new Vector2(healthBarOriginalSize.x * healthPercentage, healthBarRect.sizeDelta.y);

        GameManager.Instance.coinsText.text = coinsCollected.ToString();
    }

    public void AddInventoryItem(string itemName, Sprite sprite)
    {
        inventory.Add(itemName, sprite);
        GameManager.Instance.inventoryItemImage.sprite = inventory[itemName];
    }
    public void ClearInventory()
    {
        inventory.Clear();
        GameManager.Instance.inventoryItemImage.sprite = inventoryBlankSprite;
    }

    public IEnumerator Die()
    {
        frozen = true;
        isVulnerable = false;
        animatorFunctions.EmitParticles("dead");
        animatorFunctions.PlaySound("dead");
        animator.SetBool("dead", true);
        yield return new WaitForSecondsRealtime(2);
        LoadLevel(SceneManager.GetActiveScene().name);
    }

    public IEnumerator FreezeEffect(float length, float timeScale)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(length);
        Time.timeScale = 1;
    }

    public void LoadLevel(string loadSceneString)
    {
        animator.SetBool("dead", false);
        health = 50;
        coinsCollected = 0;
        ClearInventory();
        frozen = false;
        isVulnerable = true;
        SceneManager.LoadScene(loadSceneString);
        SetSpawnPosition();
        UpdateUI();
    }

    public void SetSpawnPosition()
    {
        gameObject.transform.position = GameObject.Find("SpawnLocation").transform.position;
    }

}
