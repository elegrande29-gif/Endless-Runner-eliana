using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f, gravityModifier = 1.5f;
    [SerializeField] private ParticleSystem explosionParticle, dirtParticle;
    [SerializeField] private AudioClip jumpSound, crashSound;

    private Animator playerAnimator;
    private AudioSource playerAudio;
    private Rigidbody playerRb;
    private bool isOnGround;
    private float animationSpeed;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        isOnGround = true;
        Physics.gravity = new Vector3(0, -9.81f * gravityModifier, 0);

        animationSpeed = playerAnimator.GetFloat("Speed_f");
        if (animationSpeed <= 0f) animationSpeed = 1f; // Fallback so she always runs
        playerAnimator.SetFloat("Speed_f", animationSpeed);
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.gameOver)
        {
            playerAnimator.SetFloat("Speed_f", 0);
            if (dirtParticle != null) dirtParticle.Stop();
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.startGame)
        {
            StartGame();
        }

        // Direct Spacebar fallback so jumping always works
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TryJump();
        }
    }

    private void StartGame()
    {
        GameManager.Instance.startGame = false;
        playerAnimator.SetFloat("Speed_f", animationSpeed);
        isOnGround = true;
        if (dirtParticle != null) dirtParticle.Play();
    }

    // Matches Player Input -> Send Messages behavior
    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            TryJump();
        }
    }

    private void TryJump()
    {
        bool isGameOver = GameManager.Instance != null && GameManager.Instance.gameOver;
        if (isOnGround && !isGameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnimator.SetTrigger("Jump_trig");
            if (dirtParticle != null) dirtParticle.Stop();
            if (playerAudio != null && jumpSound != null) playerAudio.PlayOneShot(jumpSound, 1.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool isGameOver = GameManager.Instance != null && GameManager.Instance.gameOver;

        if ((collision.gameObject.name == "Ground" || collision.gameObject.CompareTag("Ground")) && !isGameOver)
        {
            isOnGround = true;
            if (dirtParticle != null) dirtParticle.Play();
        }

        if (collision.gameObject.CompareTag("Obstacle") && !isGameOver)
        {
            if (GameManager.Instance != null) GameManager.Instance.gameOver = true;
            playerAnimator.SetBool("Death_b", true);
            playerAnimator.SetInteger("DeathType_int", 1);
            if (dirtParticle != null) dirtParticle.Stop();
            if (explosionParticle != null) explosionParticle.Play();
            if (playerAudio != null && crashSound != null) playerAudio.PlayOneShot(crashSound, 1.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Scoreable"))
        {
            Debug.Log("Received a Score");
        }
    }
}