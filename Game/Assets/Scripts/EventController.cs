using UnityEngine;

public class EventController : MonoBehaviour
{
    public enum EventType { Fire, Bandit, Treasure, None }

    [System.Serializable]
    public class SpawnPointConfig
    {
        [Header("λ������")]
        public Transform position;

        [Header("�¼�����")]
        public EventType eventType;

        [Header("����Ԥ����")]
        public GameObject prefab;
    }

    [Header("�¼�����")]
    public SpawnPointConfig[] spawnPoints;

    [Header("���ɲ���")]
    public float spawnRadius = 1f;
    public LayerMask obstacleLayer;

    [Header("ˢ������")]
    public float refreshInterval = 60f; // ˢ�¼�����룩
    private float timer = 0f;           // ��ʱ��
    private GameObject currentEvent;    // ��ǰ�¼�ʵ��

    void Start()
    {
        // ��ʼ�����¼�
        SpawnEvent();

        // ��ʼ����ʱ��
        timer = refreshInterval;
    }

    void Update()
    {
        // ���¼�ʱ��
        timer -= Time.deltaTime;

        // ʱ�䵽��ˢ���¼�
        if (timer <= 0)
        {
            RefreshEvent();
        }
    }

    private void RefreshEvent()
    {
        // �����ǰ�¼�
        ClearCurrentEvent();

        // �������¼�
        SpawnEvent();

        // ���ü�ʱ��
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

        // �����Ҫ�����¼��߼������ڴ���ӣ�
        switch (config.eventType)
        {
            case EventType.Fire:
                Debug.Log("���������¼�");
                break;
            case EventType.Bandit:
                Debug.Log("���������¼�");
                break;
            case EventType.Treasure:
                Debug.Log("���������¼�");
                break;
        }

        return instance;
    }
}