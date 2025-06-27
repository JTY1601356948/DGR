using UnityEngine;

public class EventMore : MonoBehaviour
{

    public GameObject eventOptionPrefab;
    public Transform[] eventPoints;

    void Start()
    {
        eventPoints = new Transform[3];
        // �����������¼���ֱ��ǳ����е��������壬�������ֶ�ָ�����ǵ� Transform


        SpawnEvents();
    }

    void SpawnEvents()
    {
        int numToSpawn = Random.Range(2, eventPoints.Length + 1);
        bool[] hasSpawned = new bool[eventPoints.Length];

        for (int i = 0; i < numToSpawn; i++)
        {
            int index;
            do
            {
                index = Random.Range(0, eventPoints.Length);
            } while (hasSpawned[index]);

            Instantiate(eventOptionPrefab, eventPoints[index].position, eventPoints[index].rotation);
            hasSpawned[index] = true;
            CanvasGroup canvasGroup = eventOptionPrefab.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                InvokeRepeating("FadeIn", 0f, 0.1f);
            }
            void FadeIn()
            {
                CanvasGroup cg = eventOptionPrefab.GetComponent<CanvasGroup>();
                if (cg != null)
                {
                    cg.alpha = Mathf.Clamp01(cg.alpha + 0.1f);
                    if (cg.alpha >= 1f)
                    {
                        CancelInvoke("FadeIn");
                    }
                }
            }
        }
    }
}
