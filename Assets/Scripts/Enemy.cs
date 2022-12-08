using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PhysicsObject
{
    [SerializeField] private float maxSpeed;
    [SerializeField] public float health = 10f;

    private int direction = 1;

    private RaycastHit2D rightLedgeRaycastHit;
    private RaycastHit2D leftLedgeRaycastHit;
    private RaycastHit2D rightWallRaycastHit;
    private RaycastHit2D leftWallRaycastHit;
    [SerializeField] private Vector2 raycastOffset;
    [SerializeField] private float raycastLength = 2;
    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private float attackPower = 10;

    void Update()
    {
        targetVelocity = new Vector2(maxSpeed * direction, 0);

        // Check for right ledge.
        rightLedgeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x + raycastOffset.x, transform.position.y + raycastOffset.y), Vector2.down, raycastLength);
        Debug.DrawRay(new Vector2(transform.position.x + raycastOffset.x, transform.position.y), Vector2.down * raycastLength, Color.red);

        // Check for left ledge.
        leftLedgeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x - raycastOffset.x, transform.position.y + raycastOffset.y), Vector2.down, raycastLength);
        Debug.DrawRay(new Vector2(transform.position.x - raycastOffset.x, transform.position.y), Vector2.down * raycastLength, Color.blue);

        if (rightLedgeRaycastHit.collider == null)
            direction = -1;
        else if (leftLedgeRaycastHit.collider == null)
            direction = 1;

        // Check for right wall.
        rightWallRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right, raycastLength, raycastLayerMask);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.right * raycastLength, Color.red);

        // Check for left wall.
        leftWallRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.left, raycastLength, raycastLayerMask);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.left * raycastLength, Color.blue);

        if (rightWallRaycastHit.collider != null)
            direction = -1;
        else if (leftWallRaycastHit.collider != null)
            direction = 1;

        if (health <= 0)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == NewPlayer.Instance.gameObject)
        {
            NewPlayer.Instance.health -= attackPower;
            NewPlayer.Instance.UpdateUI();
        }
    }
}
