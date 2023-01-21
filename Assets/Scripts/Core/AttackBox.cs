using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy;
        if (enemy = collision.gameObject.GetComponentInParent<Enemy>())
        {
            NewPlayer.Instance.BecomeInvulnerable(0.2f); // Become invulnerable for 0.2s after hurting an enemy.
            enemy.Hurt(NewPlayer.Instance.attackPower);
        }
    }
}
