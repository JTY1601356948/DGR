using UnityEngine;

public class TianGuiSpawnManager : MonoBehaviour
{
    public GameObject tianGuiPrefab;        // ���Ԥ����
    public float spawnYPosition = 3f;       // �̶���Y������λ��
    public float minXPosition = -10f;       // X����С����λ��
    public float maxXPosition = 10f;        // X���������λ��
    public float spawnInterval = 30f;       // ���ɼ��ʱ�䣨�룩
    public GameObject virtualCamera; // �����Ұ���

    private float nextSpawnTime = 0f;
    private float cameraHalfWidth;

    void Start()
    {
        // ���������Ұ��ȵ�һ�루�����ж�����λ���Ƿ�����Ұ�ڣ�
        if (virtualCamera != null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                cameraHalfWidth = mainCamera.aspect * mainCamera.orthographicSize;
            }
        }

        // ������������һ�����
        SpawnTianGui();

        // �����״��Զ�������ɵ�ʱ�䣨���뵱ǰʱ�� spawnInterval ���
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        // ����Ƿ����Զ��������ʱ��
        if (Time.time >= nextSpawnTime)
        {
            SpawnTianGui();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnTianGui()
    {
        // �������е����
        DestroyExistingTianGui();

        // ������Ч�����������ų������Ұ��Χ��
        float cameraX = virtualCamera.transform.position.x;
        float safeMinX = cameraX + cameraHalfWidth + 2f; // ����Ҳలȫ����
        float safeMaxX = cameraX - cameraHalfWidth - 2f; // �����లȫ����

        // ȷ������λ�ã�����Ч���������ѡ��
        float spawnX;
        if (Random.value > 0.5f)
        {
            // ������Ҳ�����
            spawnX = Random.Range(Mathf.Max(safeMinX, minXPosition), maxXPosition);
        }
        else
        {
            // ������������
            spawnX = Random.Range(minXPosition, Mathf.Min(safeMaxX, maxXPosition));
        }

        // ȷ������λ���������X�᷶Χ��
        spawnX = Mathf.Clamp(spawnX, minXPosition, maxXPosition);

        // �������
        Vector3 spawnPosition = new Vector3(spawnX, spawnYPosition, 0f);
        Instantiate(tianGuiPrefab, spawnPosition, Quaternion.identity);

        Debug.Log($"��λ�� ({spawnX}, {spawnYPosition}) ���������");
    }

    void DestroyExistingTianGui()
    {
        // ���Ҳ����������������
        GameObject[] existingTianGuis = GameObject.FindGameObjectsWithTag("TianGui");
        foreach (GameObject tianGui in existingTianGuis)
        {
            Destroy(tianGui);
        }

        if (existingTianGuis.Length > 0)
        {
            Debug.Log($"������ {existingTianGuis.Length} ���������");
        }
    }
}