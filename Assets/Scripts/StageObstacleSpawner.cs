using UnityEngine;

public class StageObstacleSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ObstacleSpawnData
    {
        public Transform spawnPoint;
        public GameObject obstaclePrefab;
    }

    [Header("Obstacle Points")]
    public ObstacleSpawnData[] obstacles;

    [Header("Spawn Settings")]
    public bool spawnOnStart = true;
    public bool useSpawnPointRotation = true;

    private void Start()
    {
        if (spawnOnStart)
            SpawnAll();
    }

    public void SpawnAll()
    {
        foreach (ObstacleSpawnData data in obstacles)
        {
            if (data.spawnPoint == null || data.obstaclePrefab == null)
            {
                Debug.LogWarning("Spawn point or obstacle prefab is missing.");
                continue;
            }

            Quaternion rotation = useSpawnPointRotation
                ? data.spawnPoint.rotation
                : Quaternion.identity;

            Instantiate(
                data.obstaclePrefab,
                data.spawnPoint.position,
                rotation
            );
        }
    }
}