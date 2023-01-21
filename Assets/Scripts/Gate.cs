using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private string requiredInventoryItem;
    [SerializeField] private Animator animator;

    [Header("SFX")]
    [SerializeField] private AudioClip openSound;
    [Range(0f, 1f)]
    [SerializeField] private float openSoundVolume = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == NewPlayer.Instance.gameObject)
        {
            if (NewPlayer.Instance.inventory.ContainsKey(requiredInventoryItem))
            {
                animator.SetTrigger("open");
                if (openSound)
                    NewPlayer.Instance.sfxAudioSource.PlayOneShot(openSound, openSoundVolume);
                //Destroy(this.gameObject);
                NewPlayer.Instance.ClearInventory();
            }
        }
    }
}
