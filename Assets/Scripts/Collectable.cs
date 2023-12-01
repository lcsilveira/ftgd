using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    enum ItemType { Coin, Health, Ammo, Inventory };

    [Header("General")]
    [SerializeField] private ItemType itemType;
    [SerializeField] private string inventoryItemName;
    public float health = 100;
    public float maxHealth = 100;

    [Header("SFX")]
    [SerializeField] private AudioClip collectionSound;
    [Range(0f, 1f)]
    [SerializeField] private float collectionSoundVolume = 1f;

    [Header("References")]
    [SerializeField] private ParticleSystem particlesGlitter;
    [SerializeField] private Sprite inventoryIcon; // If this specific item will be represented by a different icon on the UI.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == NewPlayer.Instance.gameObject)
        {
            if (particlesGlitter)
            {
                particlesGlitter.gameObject.transform.parent = null;
                particlesGlitter.gameObject.SetActive(true);
                Destroy(particlesGlitter.gameObject, particlesGlitter.main.duration);
            }

            if (collectionSound)
                NewPlayer.Instance.sfxAudioSource.PlayOneShot(collectionSound, collectionSoundVolume * Random.Range(0.8f, 1f));

            if (itemType == ItemType.Coin)
                NewPlayer.Instance.coinsCollected++;
            else if (itemType == ItemType.Health)
                NewPlayer.Instance.health += 10;
            else if (itemType == ItemType.Inventory)
            {
                Sprite itemSprite;
                itemSprite = inventoryIcon ? inventoryIcon : gameObject.GetComponent<SpriteRenderer>().sprite;
                NewPlayer.Instance.AddInventoryItem(inventoryItemName, itemSprite);
            }
            NewPlayer.Instance.UpdateUI();
            Destroy(gameObject);
        }
    }
}
