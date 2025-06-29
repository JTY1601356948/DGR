using UnityEngine;

public class TianGuiSpawnManager : MonoBehaviour
{
    public GameObject tianGuiPrefab;        // 天鬼预制体
    public float spawnYPosition = 3f;       // 固定的Y轴生成位置
    public float minXPosition = -10f;       // X轴最小生成位置
    public float maxXPosition = 10f;        // X轴最大生成位置
    public float spawnInterval = 30f;       // 生成间隔时间（秒）
    public GameObject virtualCamera; // 玩家视野相机

    private float nextSpawnTime = 0f;
    private float cameraHalfWidth;

    void Start()
    {
        // 计算相机视野宽度的一半（用于判断生成位置是否在视野内）
        if (virtualCamera != null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                cameraHalfWidth = mainCamera.aspect * mainCamera.orthographicSize;
            }
        }

        // 开局立即生成一个天鬼
        SpawnTianGui();

        // 设置首次自动间隔生成的时间（距离当前时间 spawnInterval 秒后）
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        // 检查是否到了自动间隔生成时间
        if (Time.time >= nextSpawnTime)
        {
            SpawnTianGui();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnTianGui()
    {
        // 清理现有的天鬼
        DestroyExistingTianGui();

        // 计算有效的生成区域（排除相机视野范围）
        float cameraX = virtualCamera.transform.position.x;
        float safeMinX = cameraX + cameraHalfWidth + 2f; // 相机右侧安全距离
        float safeMaxX = cameraX - cameraHalfWidth - 2f; // 相机左侧安全距离

        // 确定生成位置（在有效区域内随机选择）
        float spawnX;
        if (Random.value > 0.5f)
        {
            // 在相机右侧生成
            spawnX = Random.Range(Mathf.Max(safeMinX, minXPosition), maxXPosition);
        }
        else
        {
            // 在相机左侧生成
            spawnX = Random.Range(minXPosition, Mathf.Min(safeMaxX, maxXPosition));
        }

        // 确保生成位置在允许的X轴范围内
        spawnX = Mathf.Clamp(spawnX, minXPosition, maxXPosition);

        // 生成天鬼
        Vector3 spawnPosition = new Vector3(spawnX, spawnYPosition, 0f);
        Instantiate(tianGuiPrefab, spawnPosition, Quaternion.identity);

        Debug.Log($"在位置 ({spawnX}, {spawnYPosition}) 生成新天鬼");
    }

    void DestroyExistingTianGui()
    {
        // 查找并销毁所有现有天鬼
        GameObject[] existingTianGuis = GameObject.FindGameObjectsWithTag("TianGui");
        foreach (GameObject tianGui in existingTianGuis)
        {
            Destroy(tianGui);
        }

        if (existingTianGuis.Length > 0)
        {
            Debug.Log($"销毁了 {existingTianGuis.Length} 个现有天鬼");
        }
    }
}