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
    [SerializeField] public bool isVulnerable = true; // Set if the player can be hurt or not.
    [SerializeField] private float jumpPower = 12;
    [SerializeField] private float maxSpeed = 8;

    [Header("Inventory/UI")]
    public int coinsCollected = 0;
    public float health = 50;
    [SerializeField] private float maxHealth = 100;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject attackBox;
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
            Destroy(this.gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        this.gameObject.name = "OriginalPlayer";

        healthBarRect = GameManager.Instance.healthBar.rectTransform;
        healthBarOriginalSize = healthBarRect.sizeDelta;
        UpdateUI();
        SetSpawnPosition();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            AnimatorFunctions animatorFunctions = this.gameObject.GetComponent<AnimatorFunctions>();
            animatorFunctions.PlaySound("jump");
            velocity.y = jumpPower;
        }

        targetVelocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeed, 0);

        // Flip the player's localScale.x if move speed is great than .01 or less than -.01.
        if (targetVelocity.x < -.5)
            transform.localScale = new Vector2(-1, 1);
        else if (targetVelocity.x > .5)
            transform.localScale = new Vector2(1, 1);

        // Activate the attackBox when pressing Fire1 key.
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("attack");
        }

        if (health <= 0)
            Die();

        // Set each animator parameters.
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        animator.SetFloat("velocityY", velocity.y);
        animator.SetBool("grounded", grounded);
        animator.SetFloat("attackDirectionY", Input.GetAxis("Vertical"));
    }

    private IEnumerator InvulnerableCoroutine(float waitSeconds)
    {
        NewPlayer.Instance.isVulnerable = false;
        yield return new WaitForSeconds(waitSeconds);
        NewPlayer.Instance.isVulnerable = true;
    }

    public void BecomeInvulnerable()
    {
        StartCoroutine(InvulnerableCoroutine(0.2f));  // Become invulnerable for 0.2s when hurting an enemy.
    }

    // Player hurt.
    public void Hurt(float hurtAmount)
    {
        if (!isVulnerable)
            return;

        StartCoroutine(InvulnerableCoroutine(0.2f)); // Become invulnerable for 0.2s after being hurted.
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

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetSpawnPosition()
    {
        this.gameObject.transform.position = GameObject.Find("SpawnLocation").transform.position;
    }

}
