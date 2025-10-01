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

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput > 0)
            spriteRenderer.flipX = false;
        else if (moveInput < 0)
            spriteRenderer.flipX = true;

        animator.SetBool("IsRunning", Mathf.Abs(moveInput) > 0.01f);

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

            if (audioSource != null && fruitCollectSFX != null)
                audioSource.PlayOneShot(fruitCollectSFX);

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Bomb"))
        {
            if (audioSource != null && gameOverSFX != null)
                audioSource.PlayOneShot(gameOverSFX);

            Debug.Log("Player hit the bomb!");
            StartCoroutine(HandleGameOver());
        }
    }

    private IEnumerator HandleGameOver()
    {
        this.enabled = false;
        rb.simulated = false;
        spriteRenderer.enabled = false;

        float delay = (gameOverSFX != null) ? gameOverSFX.length : 0.5f;
        yield return new WaitForSecondsRealtime(delay);

        gameManager.GameOver();
    }
}
    