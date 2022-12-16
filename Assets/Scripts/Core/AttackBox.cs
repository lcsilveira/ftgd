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
            NewPlayer.Instance.BecomeInvulnerable();
            //enemy.health -= NewPlayer.Instance.attackPower;
            enemy.Hurt(NewPlayer.Instance.attackPower);
        }
    }
}
