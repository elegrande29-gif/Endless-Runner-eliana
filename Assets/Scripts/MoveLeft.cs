using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private float speed = 10f;
    private float leftBound = -15f;

    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.gameOver)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
        }

        if (transform.position.x < leftBound && (gameObject.CompareTag("Obstacle") || gameObject.CompareTag("Collectible")))
        {
            Destroy(gameObject);
        }
    }
}