using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 2f;  // Delay before the grenade explodes
    public float blastRadius = 5f;  // Radius of the explosion effect
    public float explosionForce = 700f;  // Force of the explosion
    public GameObject explosionEffect;  // Optional: Prefab for visual explosion effects

    public bool isRocket;

    [SerializeField] private AudioSource explosionSound;

    float countdown;  // Internal countdown timer
    bool hasExploded = false;  // Flag to ensure it only explodes once

    void Start()
    {
        countdown = delay;  // Initialize countdown timer
    }

    void Update()
    {
        if (!isRocket) 
        {
            countdown -= Time.deltaTime;  // Decrement the timer
            if (countdown <= 0f && !hasExploded)
            {
                Explode();  // Trigger the explosion
                hasExploded = true;  // Set the flag so it doesn't explode multiple times
            }
        }     
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isRocket && !hasExploded) // Check if it's a rocket and hasn't exploded yet
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        // Show explosion effect (if assigned)
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        //if (explosionSound != null && !explosionSound.isPlaying) explosionSound.Play();
        explosionSound.Play();

        // Find all colliders within the blast radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, blastRadius);
            }

            // Optionally, apply damage to wall pieces
            WallPieceScript wallPiece = nearbyObject.GetComponent<WallPieceScript>();
            if (wallPiece != null)
            {
                wallPiece.TakeDamageFromGrenade(transform.position);  // Apply damage if it's a wall piece
            }

            DestructibleObject destructible = nearbyObject.GetComponent<DestructibleObject>();
            if (destructible != null)
            {
                destructible.Shatter(); // Directly shatter the object
            }
        }

        // Destroy the grenade object itself
        Destroy(gameObject);
    }
}
