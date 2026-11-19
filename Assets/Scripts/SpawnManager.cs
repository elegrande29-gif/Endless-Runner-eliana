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
        InvokeRepeating("SpawnCollectible", startDelay + (repeatRate / 2f), repeatRate);
    }

    private float GetRoadHeight(float xPos, float zPos)
    {
        RaycastHit[] hits = Physics.RaycastAll(new Vector3(xPos, 8f, zPos), Vector3.down, 20f);
        float topRoadY = 0f;
        bool foundGround = false;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                if (!foundGround || hit.point.y > topRoadY)
                {
                    topRoadY = hit.point.y;
                    foundGround = true;
                }
            }
        }

        return topRoadY;
    }

    void SpawnObstacle()
    {
        if ((GameManager.Instance == null || !GameManager.Instance.gameOver) && obstaclePrefab != null)
        {
            float roadY = GetRoadHeight(obstacleSpawnPos.x, obstacleSpawnPos.z);
            Vector3 spawnPos = new Vector3(obstacleSpawnPos.x, roadY, obstacleSpawnPos.z);

            GameObject obstacle = Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
            if (obstacle.GetComponent<MoveLeft>() == null)
            {
                obstacle.AddComponent<MoveLeft>();
            }
        }
    }

    void SpawnCollectible()
    {
        if ((GameManager.Instance == null || !GameManager.Instance.gameOver) && collectiblePrefab != null)
        {
            // Spawn either just above the road (+1.2f) or at jump height (+3.0f)
            float randomHeightOffset = Random.Range(0, 2) == 0 ? 1.2f : 3.0f;
            float groundY = GetRoadHeight(obstacleSpawnPos.x, obstacleSpawnPos.z);

            Vector3 collectiblePos = new Vector3(obstacleSpawnPos.x, groundY + randomHeightOffset, obstacleSpawnPos.z);
            GameObject collectible = Instantiate(collectiblePrefab, collectiblePos, collectiblePrefab.transform.rotation);
            if (collectible.GetComponent<MoveLeft>() == null)
            {
                collectible.AddComponent<MoveLeft>();
            }
        }
    }
}