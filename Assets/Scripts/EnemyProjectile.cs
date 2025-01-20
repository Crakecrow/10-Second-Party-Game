using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public string playerTag = "Player"; // Tag for the player
    public float damageAmount = 10f; // Amount of damage to inflict on the player
    public float lifetime = 2f; // Time before the projectile is destroyed

    void Start()
    {
        // Destroy the projectile after 'lifetime' seconds
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            // Inflict damage on the player
            Player_Health playerHealth = collision.GetComponent<Player_Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("Player hit by enemy projectile!");
            }
            Destroy(gameObject); // Destroy the projectile
        }
    }
}
