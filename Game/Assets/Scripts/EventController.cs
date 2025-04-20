using UnityEngine;

public class EventController : MonoBehaviour
{
    public enum EventType { Fire, Bandit, Treasure, None }

    [System.Serializable]
    public class SpawnPointConfig
    {
        [Header("位置配置")]
        public Transform position;

        [Header("事件类型")]
        public EventType eventType;

        [Header("关联预制体")]
        public GameObject prefab;
    }

    [Header("事件配置")]
    public SpawnPointConfig[] spawnPoints;

    [Header("生成参数")]
    public float spawnRadius = 1f;
    public LayerMask obstacleLayer;

    void Start()
    {
        SpawnPointConfig selectedPoint = GetRandomValidPoint();
        if (selectedPoint != null && selectedPoint.eventType != EventType.None)
        {
            ExecuteEvent(selectedPoint);
        }
    }

    private SpawnPointConfig GetRandomValidPoint()
    {
        var validPoints = new System.Collections.Generic.List<SpawnPointConfig>();
        foreach (var point in spawnPoints)
        {
            if (point.position != null && !CheckObstacleAtPosition(point.position.position))
            {
                validPoints.Add(point);
            }
        }
        return validPoints.Count > 0 ? validPoints[Random.Range(0, validPoints.Count)] : null;
    }

    private bool CheckObstacleAtPosition(Vector2 position)
    {
        Collider2D hit = Physics2D.OverlapCircle(position, spawnRadius, obstacleLayer);
        return hit != null;
    }

    private void ExecuteEvent(SpawnPointConfig config)
    {
        if (config.prefab != null)
        {
            Instantiate(config.prefab, config.position.position, Quaternion.identity);
        }

        // 如果需要独立事件逻辑，可在此添加：
        switch (config.eventType)
        {
            case EventType.Fire:
                Debug.Log("触发火灾事件");
                break;
            case EventType.Bandit:
                Debug.Log("触发盗贼事件");
                break;
        }
    }
}
