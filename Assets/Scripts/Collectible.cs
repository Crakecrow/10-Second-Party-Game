using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public AudioClip collectSound; // Sound effect for collecting the shard

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Play the collect sound
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            // Call the GameManager to update the shard count
            GameManager.instance.CollectShard();

            // Destroy the collectible object
            Destroy(gameObject);
        }
    }
}
