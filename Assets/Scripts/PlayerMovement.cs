using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;
    public Rigidbody2D rb;
    public KeyCode dashKey = KeyCode.Space; // Key for dashing
    public string enemyTag = "Enemy"; // Tag for the enemies to be destroyed on dash

    public GameObject projectilePrefab; // Prefab for the projectile
    public float projectileSpeed = 10f; // Speed of the projectile
    public KeyCode shootKey = KeyCode.Mouse0; // Key for shooting (default: left mouse button)
    public float shootCooldown = 0.5f; // Cooldown between shots
    public AudioClip shootSound; // Sound for shooting
    public AudioClip dashSound; // Sound for dashing

    private Vector2 moveDirection;
    private bool isDashing = false;
    private bool canDash = true;
    private bool isImmune = false; // Flag for damage immunity
    private bool canShoot = true;

    private AudioSource audioSource;
    private bool facingRight = true;

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            return; // Stop all movement when the game is won
        }

        ProcessInputs();

        if (Input.GetKeyDown(dashKey) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(shootKey) && canShoot)
        {
            StartCoroutine(Shoot());
        }

        FlipSprite();
    }

    void FixedUpdate()
    {
        if (Time.timeScale == 0f)
        {
            return; // Stop all movement when the game is won
        }

        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY);
    }

    void Move()
    {
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        isImmune = true;
        rb.linearVelocity = moveDirection * dashSpeed;
        Debug.Log("Dashing!");

        // Play dash sound
        if (dashSound != null)
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.PlayOneShot(dashSound);
        }

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        isImmune = false;
        rb.linearVelocity = moveDirection * moveSpeed;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator Shoot()
    {
        canShoot = false;

        // Calculate the spawn position to be slightly ahead of the player to avoid immediate collision
        Vector2 shootDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        Vector2 spawnPosition = (Vector2)transform.position + shootDirection * 0.5f;

        // Create and shoot projectile
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.linearVelocity = shootDirection * projectileSpeed;

        // Play shoot sound
        if (shootSound != null)
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.PlayOneShot(shootSound);
        }

        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing && collision.gameObject.CompareTag(enemyTag))
        {
            AIChase enemy = collision.gameObject.GetComponent<AIChase>();
            if (enemy != null)
            {
                enemy.DestroyEnemy();
            }
            AIShooter triangleEnemy = collision.gameObject.GetComponent<AIShooter>();
            if (triangleEnemy != null)
            {
                triangleEnemy.DestroyEnemy();
            }
            Debug.Log("Enemy destroyed by dashing!");
        }
        else if (!isImmune && collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player hit by enemy!");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(enemyTag))
        {
            Debug.Log("Projectile collision detected with enemy");
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
            Destroy(collision.gameObject); // Destroy the enemy GameObject
            Destroy(gameObject); // Destroy the projectile
            Debug.Log("Enemy destroyed by projectile!");
        }
    }

    void FlipSprite()
    {
        if (moveDirection.x > 0 && !facingRight)
        {
            facingRight = true;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        else if (moveDirection.x < 0 && facingRight)
        {
            facingRight = false;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
