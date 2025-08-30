using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class playerCon : MonoBehaviour
{
    [SerializeField] public float jumpForce = 10f;
    [SerializeField] public float moveSpeed = 5f;
    public Animator animator;
    public GameManager gameManager;

    public AudioSource audioSource;
    public AudioClip fruitCollectSFX;
    public AudioClip gameOverSFX;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool IsGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        // Move the player using Rigidbody velocity
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Flip sprite
        if (moveInput > 0)
            spriteRenderer.flipX = false;
        else if (moveInput < 0)
            spriteRenderer.flipX = true;

        // Running animation
        animator.SetBool("IsRunning", Mathf.Abs(moveInput) > 0.01f);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            IsGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            IsGrounded = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FallingObject"))
        {
            gameManager.IncreaseScore(1);

            // Play fruit collect sound
            if (audioSource != null && fruitCollectSFX != null)
                audioSource.PlayOneShot(fruitCollectSFX);

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Bomb"))
        {
            // Play game over sound
            if (audioSource != null && gameOverSFX != null)
                audioSource.PlayOneShot(gameOverSFX);

            Debug.Log("Player hit the bomb!");

            StartCoroutine(HandleGameOver());
        }
    }


    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("FallingObject"))
    //    {
    //        gameManager.IncreaseScore(1);


    //        // Play fruit collect sound
    //        if (audioSource != null && fruitCollectSFX != null)
    //            audioSource.PlayOneShot(fruitCollectSFX);

    //        Destroy(other.gameObject);
    //    }
    //    else if (other.CompareTag("Bomb"))
    //    {
    //        // Play game over sound
    //        if (audioSource != null && gameOverSFX != null)
    //            audioSource.PlayOneShot(gameOverSFX);

    //        Debug.Log("Player hit the bomb!");

    //        // Disable the player instead of destroying
    //        StartCoroutine(HandleGameOver());
    //    }
    //}

    private IEnumerator HandleGameOver()
    {
        // Disable player movement and physics immediately
        this.enabled = false;          // disables this script (no more Update)
        rb.simulated = false;          // stop physics
        spriteRenderer.enabled = false; // hide sprite

        // Wait for sound to finish
        float delay = (gameOverSFX != null) ? gameOverSFX.length : 0.5f;
        yield return new WaitForSecondsRealtime(delay);

        // Now trigger the GameManager’s GameOver method
        gameManager.GameOver();
    }
}
