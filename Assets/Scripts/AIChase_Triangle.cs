using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShooter : MonoBehaviour
{
    public float speed;
    public float retreatSpeed;
    public float safeDistance;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float shootCooldown = 2f;
    public AudioClip shootSound;
    public ParticleSystem explosionEffect; // Particle system for explosion effect
    public AudioClip destroySound; // Sound effect for when the triangle enemy is destroyed

    private GameObject player;
    private bool canShoot = true;
    private AudioSource audioSource;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player GameObject with tag 'Player' not found.");
        }

        if (shootSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            Vector2 direction = (player.transform.position - transform.position).normalized;

            if (distance < safeDistance)
            {
                // Retreat from player
                transform.position = Vector2.MoveTowards(transform.position, transform.position - (Vector3)direction, retreatSpeed * Time.deltaTime);
            }
            else
            {
                // Maintain distance
                transform.position = Vector2.MoveTowards(transform.position, transform.position, speed * Time.deltaTime);
            }

            if (canShoot)
            {
                StartCoroutine(Shoot());
            }
        }
    }

    private IEnumerator Shoot()
    {
        canShoot = false;

        // Create and shoot projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

        // Calculate direction to shoot the projectile towards the player
        Vector2 shootDirection = (player.transform.position - transform.position).normalized;
        projectileRb.linearVelocity = shootDirection * projectileSpeed;

        // Play shoot sound
        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Handle collision with the player, if necessary
            Debug.Log("Player hit by triangle enemy!");
        }
    }

    public void DestroyEnemy()
    {
        Debug.Log("DestroyEnemy method called."); // Debug log

        // Instantiate and play the explosion effect
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect.gameObject, transform.position, Quaternion.identity);
            ParticleSystem explosionParticles = explosion.GetComponent<ParticleSystem>();
            explosionParticles.Play();

            // Destroy the explosion effect after it finishes playing
            Destroy(explosion, explosionParticles.main.duration);
            Debug.Log("Explosion effect played."); // Debug log
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
