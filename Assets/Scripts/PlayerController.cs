using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravityModifier = 1.5f;
    [SerializeField] private float runAnimSpeed = 0.8f;
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private ParticleSystem dirtParticle;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip crashSound;

    private Animator playerAnimator;
    private AudioSource playerAudio;
    private Rigidbody playerRb;
    private bool isOnGround = true;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        Physics.gravity = new Vector3(0, -9.81f * gravityModifier, 0);

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed_f", 1.0f);
            playerAnimator.speed = runAnimSpeed;
        }
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.gameOver) return;

        if (GameManager.Instance != null && GameManager.Instance.startGame)
        {
            StartGame();
        }

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TryJump();
        }
    }

    private void StartGame()
    {
        GameManager.Instance.startGame = false;
        isOnGround = true;

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed_f", 1.0f);
            playerAnimator.speed = runAnimSpeed;
        }

        if (dirtParticle != null) dirtParticle.Play();
    }

    private void TryJump()
    {
        // Backup check in case OnCollisionEnter missed the floor, without allowing mid-air double jumps
        if (!isOnGround && playerRb != null && Mathf.Abs(playerRb.linearVelocity.y) < 0.05f)
        {
            if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.25f))
            {
                isOnGround = true;
            }
        }

        if (isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;

            if (playerAnimator != null) playerAnimator.SetTrigger("Jump_trig");
            if (dirtParticle != null) dirtParticle.Stop();
            if (playerAudio != null && jumpSound != null) playerAudio.PlayOneShot(jumpSound, 1.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool isGameOver = GameManager.Instance != null && GameManager.Instance.gameOver;

        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            if (dirtParticle != null && !isGameOver) dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle") && !isGameOver)
        {
            if (GameManager.Instance != null) GameManager.Instance.gameOver = true;

            if (playerAnimator != null)
            {
                playerAnimator.speed = 1.0f;
                playerAnimator.SetFloat("Speed_f", 0f);
                playerAnimator.SetBool("Death_b", true);
                playerAnimator.SetInteger("DeathType_int", 1);
            }

            if (dirtParticle != null) dirtParticle.Stop();
            if (explosionParticle != null) explosionParticle.Play();
            if (playerAudio != null && crashSound != null) playerAudio.PlayOneShot(crashSound, 1.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isGameOver = GameManager.Instance != null && GameManager.Instance.gameOver;

        if ((other.gameObject.CompareTag("Collectible") || other.gameObject.CompareTag("Scoreable")) && !isGameOver)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(5);
            }
            Destroy(other.gameObject);
        }
    }
}