using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    enum ItemType { Coin, Health, Ammo, Inventory };
    [SerializeField] private ItemType itemType;
    [SerializeField] private string inventoryItemName;
    public float health = 100;
    public float maxHealth = 100;

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Alternative to the "global" property
        //NewPlayer newPlayer = collision.gameObject.GetComponent<NewPlayer>();
        //if (collision.gameObject.name == "Player")
        if (collision.gameObject == NewPlayer.Instance.gameObject)
        {
            if (itemType == ItemType.Coin)
            {
                NewPlayer.Instance.coinsCollected++;
            }
            else if (itemType == ItemType.Health)
            {
                NewPlayer.Instance.health+=10;
            }
            else if (itemType == ItemType.Inventory)
            {
                Sprite itemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                NewPlayer.Instance.AddInventoryItem(inventoryItemName, itemSprite);
            }
            NewPlayer.Instance.UpdateUI();
            Destroy(gameObject);
        }
    }
}
