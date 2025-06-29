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

    [Header("刷新设置")]
    public float refreshInterval = 60f; // 刷新间隔（秒）
    private float timer = 0f;           // 计时器
    private GameObject currentEvent;    // 当前事件实例

    void Start()
    {
        // 初始触发事件
        SpawnEvent();

        // 初始化计时器
        timer = refreshInterval;
    }

    void Update()
    {
        // 更新计时器
        timer -= Time.deltaTime;

        // 时间到则刷新事件
        if (timer <= 0)
        {
            RefreshEvent();
        }
    }

    private void RefreshEvent()
    {
        // 清除当前事件
        ClearCurrentEvent();

        // 生成新事件
        SpawnEvent();

        // 重置计时器
        timer = refreshInterval;
    }

    private void SpawnEvent()
    {
        SpawnPointConfig selectedPoint = GetRandomValidPoint();
        if (selectedPoint != null && selectedPoint.eventType != EventType.None)
        {
            currentEvent = ExecuteEvent(selectedPoint);
        }
    }

    private void ClearCurrentEvent()
    {
        if (currentEvent != null)
        {
            Destroy(currentEvent);
            currentEvent = null;
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

    private GameObject ExecuteEvent(SpawnPointConfig config)
    {
        GameObject instance = null;

        if (config.prefab != null)
        {
            instance = Instantiate(config.prefab, config.position.position, Quaternion.identity);
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
            case EventType.Treasure:
                Debug.Log("触发宝藏事件");
                break;
        }

        return instance;
    }
}