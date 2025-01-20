using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : MonoBehaviour
{
    public float speed;
    public float damageAmount = 10f; // Amount of damage to inflict
    public ParticleSystem explosionEffect; // Particle system for explosion effect
    public AudioClip destroySound; // Sound effect for when the cube is destroyed

    private GameObject player;
    private float distance;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player GameObject with tag 'Player' not found.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            Vector2 direction = (player.transform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            
            Debug.Log("Chasing player. Current position: " + transform.position + " Target position: " + player.transform.position);
        }
    }

    // Method to handle collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            // Call the TakeDamage method on the player's health script
            Player_Health playerHealth = player.GetComponent<Player_Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }

    // Method to handle destruction
    public void DestroyEnemy()
    {
        Debug.Log("DestroyEnemy method called."); // Debug log

        // Instantiate and play the explosion effect
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect.gameObject, transform.position, Quaternion.identity);
            ParticleSystem explosionParticles = explosion.GetComponent<ParticleSystem>();
            explosionParticles.Play();
            Debug.Log("Explosion effect instantiated."); // Debug log

            // Destroy the explosion effect after it finishes playing
            Destroy(explosion, explosionParticles.main.duration);
            Debug.Log("Explosion effect played and set to destroy after duration."); // Debug log
        }
        else
        {
            Debug.LogWarning("Explosion effect is not assigned."); // Debug log
        }

        // Play the destroy sound effect
        if (destroySound != null)
        {
            // Create a temporary GameObject to play the sound
            GameObject soundObject = new GameObject("DestroySound");
            AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();
            tempAudioSource.clip = destroySound;
            tempAudioSource.Play();
            Debug.Log("Destroy sound instantiated and played."); // Debug log

            // Destroy the temporary GameObject after the clip has finished playing
            Destroy(soundObject, destroySound.length);
            Debug.Log("Destroy sound set to destroy after length."); // Debug log
        }
        else
        {
            Debug.LogWarning("Destroy sound is not assigned."); // Debug log
        }

        // Destroy the enemy GameObject
        Destroy(gameObject);
        Debug.Log("Enemy GameObject destroyed."); // Debug log
    }
}
