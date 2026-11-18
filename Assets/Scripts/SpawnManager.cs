using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject collectiblePrefab;

    private Vector3 obstacleSpawnPos = new Vector3(25, 0, 0);
    private float startDelay = 2f;
    private float repeatRate = 2f;

    void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        // Offset startDelay by half the repeatRate (3f instead of 2f) with the same 2f interval
        InvokeRepeating("SpawnCollectible", startDelay + (repeatRate / 2f), repeatRate);
    }

    void SpawnObstacle()
    {
        if (!GameManager.Instance.gameOver)
        {
            Instantiate(obstaclePrefab, obstacleSpawnPos, obstaclePrefab.transform.rotation);
        }
        else
        {
            CancelInvoke();
        }
    }

    void SpawnCollectible()
    {
        if (!GameManager.Instance.gameOver && collectiblePrefab != null)
        {
            // Spawn at ground height (1.2f) or jump height (3.2f) halfway between barriers
            float randomY = Random.Range(0, 2) == 0 ? 1.2f : 3.2f;
            Vector3 collectiblePos = new Vector3(25, randomY, 0);
            Instantiate(collectiblePrefab, collectiblePos, collectiblePrefab.transform.rotation);
        }
    }
}