using UnityEngine;

public class Projectile : MonoBehaviour
{
    public string enemyTag = "Enemy"; // Tag for the enemies to be destroyed
    public float lifetime = 2f; // Time before the projectile is destroyed

    void Start()
    {
        // Destroy the projectile after 'lifetime' seconds
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(enemyTag))
        {
            AIChase enemy = collision.GetComponent<AIChase>();
            if (enemy != null)
            {
                enemy.DestroyEnemy();
            }
            
            AIShooter triangleEnemy = collision.GetComponent<AIShooter>();
            if (triangleEnemy != null)
            {
                triangleEnemy.DestroyEnemy();
            }
            
            Destroy(gameObject); // Destroy the projectile
            Debug.Log("Enemy hit by projectile!");
        }
    }
}
