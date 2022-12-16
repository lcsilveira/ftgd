using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PhysicsObject
{
    [Header("Attributes")]
    [SerializeField] private float attackPower = 10;
    private int direction = 1;
    [SerializeField] private float health = 10f;
    [SerializeField] private float maxSpeed;

    [Header("Raycast")]
    [SerializeField] private LayerMask raycastLayerMask; // Defines which layers do we want the raycast to interact with.
    [SerializeField] private float raycastLength = 2;
    [SerializeField] private Vector2 raycastOffset; // Offset from the center of the raycast origin.

    [Header("References")]
    [SerializeField] private Animator animator;
    private RaycastHit2D rightLedgeRaycastHit;
    private RaycastHit2D leftLedgeRaycastHit;
    private RaycastHit2D rightWallRaycastHit;
    private RaycastHit2D leftWallRaycastHit;

    [Header("SFX")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;
    [Range(0f, 1f)]
    [SerializeField] private float enemySoundVolume = 1f;

    void Update()
    {
        targetVelocity = new Vector2(maxSpeed * direction, 0);

        // Check for right ledge.
        rightLedgeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x + raycastOffset.x, transform.position.y), Vector2.down, raycastLength);
        Debug.DrawRay(new Vector2(transform.position.x + raycastOffset.x, transform.position.y), Vector2.down * raycastLength, Color.red);
        if (rightLedgeRaycastHit.collider == null)
            direction = -1;

        // Check for left ledge.
        leftLedgeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x - raycastOffset.x, transform.position.y), Vector2.down, raycastLength);
        Debug.DrawRay(new Vector2(transform.position.x - raycastOffset.x, transform.position.y), Vector2.down * raycastLength, Color.blue);
        if (leftLedgeRaycastHit.collider == null)
            direction = 1;

        // Check for right wall.
        rightWallRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x + raycastOffset.x, transform.position.y + raycastOffset.y), Vector2.right, raycastLength, raycastLayerMask);
        Debug.DrawRay(new Vector2(transform.position.x + raycastOffset.x, transform.position.y + raycastOffset.y), Vector2.right * raycastLength, Color.red);
        if (rightWallRaycastHit.collider != null)
            direction = -1;

        // Check for left wall.
        leftWallRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x - raycastOffset.x, transform.position.y + raycastOffset.y), Vector2.left, raycastLength, raycastLayerMask);
        Debug.DrawRay(new Vector2(transform.position.x - raycastOffset.x, transform.position.y + raycastOffset.y), Vector2.left * raycastLength, Color.blue);
        if (leftWallRaycastHit.collider != null)
            direction = 1;

        if (health <= 0)
        {
            if (deathSound)
                NewPlayer.Instance.sfxAudioSource.PlayOneShot(deathSound, enemySoundVolume);
            Destroy(this.gameObject);
        }
    }

    // Enemy hurt.
    public void Hurt(float hurtAmount = 5f)
    {
        animator.SetTrigger("hurt");
        if (hurtSound)
            NewPlayer.Instance.sfxAudioSource.PlayOneShot(hurtSound, enemySoundVolume * Random.Range(0.5f, 1f));
        health -= hurtAmount;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == NewPlayer.Instance.gameObject)
            NewPlayer.Instance.Hurt(attackPower);
        // If it collides with another enemy, turn around
        // Since the raycast is ignoring "Enemy" layer in order to prevent raycast hit with itself
        else if (collision.gameObject.CompareTag("Enemy"))
            direction *= -1;
    }
}
