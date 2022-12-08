using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private string requiredInventoryItem;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == NewPlayer.Instance.gameObject)
        {
            if (NewPlayer.Instance.inventory.ContainsKey(requiredInventoryItem))
            {
                Destroy(this.gameObject);
                NewPlayer.Instance.ClearInventory();
            }
        }
    }
}
