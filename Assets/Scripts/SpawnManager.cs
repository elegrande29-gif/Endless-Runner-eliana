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
            Vector3 spawnPos = obstacleSpawnPos;

            // Detect the exact road height at X = 25 so obstacles sit on top of the sloped street
            if (Physics.Raycast(new Vector3(spawnPos.x, 50f, spawnPos.z), Vector3.down, out RaycastHit hit, 100f))
            {
                spawnPos.y = hit.point.y;
            }

            Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
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
            // Offset above the road: ground height (+1.2f) or jump height (+3.2f)
            float randomHeightOffset = Random.Range(0, 2) == 0 ? 1.2f : 3.2f;
            float groundY = 0f;

            if (Physics.Raycast(new Vector3(25f, 50f, 0f), Vector3.down, out RaycastHit hit, 100f))
            {
                groundY = hit.point.y;
            }

            Vector3 collectiblePos = new Vector3(25, groundY + randomHeightOffset, 0);
            Instantiate(collectiblePrefab, collectiblePos, collectiblePrefab.transform.rotation);
        }
    }
}