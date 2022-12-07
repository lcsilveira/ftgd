using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayer : PhysicsObject
{
    [SerializeField] private float maxSpeed = 8;
    [SerializeField] private float jumpPower = 12;
    [SerializeField] private float attackDuration = 0.14159f;
    [SerializeField] private GameObject attackBox;
    [SerializeField] public float attackPower = 10;

    public Dictionary<string, Sprite> inventory = new Dictionary<string, Sprite>();
    public Image inventoryItemImage;
    //public Sprite keySprite;
    //public Sprite keyGemSprite;
    public Sprite inventoryBlankSprite;

    // Coins.
    public int coinsCollected = 0;
    public TMP_Text coinsText;

    // Health bar stuff.
    public Image healthBar;
    public float health = 50;
    private float maxHealth = 100;
    private Vector2 healthBarOriginalSize;
    private RectTransform healthBarRect;

    // Singleton instantiation.
    private static NewPlayer instance;
    public static NewPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<NewPlayer>();
            }
            return instance;
        }
    }


    void Start()
    {
        healthBarRect = healthBar.rectTransform;
        healthBarOriginalSize = healthBarRect.sizeDelta;
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpPower;
        }
        // targetVelocity = vem do obj
        targetVelocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeed, 0);

        // Flip the player's localScale.x if move speed is great than .01 or less than -.01.
        if (targetVelocity.x < -.01)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if (targetVelocity.x > .01)
        {
            transform.localScale = new Vector2(1, 1);
        }

        // Activate the attackBox when pressing Fire1 key.
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(ActivateAttack());
        }
    }

    public void UpdateUI()
    {
        float healthPercentage;
        if (health <= maxHealth)
        {
            healthPercentage = health / maxHealth;
        }
        else
        {
            // Avoid health being greater than maxHealth.
            health = maxHealth;
            healthPercentage = 1;
        }
        healthBarRect.sizeDelta = new Vector2(healthBarOriginalSize.x * healthPercentage, healthBarRect.sizeDelta.y);

        coinsText.text = coinsCollected.ToString();
    }

    public void AddInventoryItem(string itemName, Sprite sprite)
    {
        inventory.Add(itemName, sprite);
        inventoryItemImage.sprite = inventory[itemName];
    }
    public void ClearInventory()
    {
        inventory.Clear();
        inventoryItemImage.sprite = inventoryBlankSprite;
    }

    // Show attackbox and wait X seconds before hidding it (called through coroutine).
    public IEnumerator ActivateAttack()
    {
        attackBox.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        attackBox.SetActive(false);
    }
}
