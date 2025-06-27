using UnityEngine;

public class EventMore : MonoBehaviour
{

    public GameObject eventOptionPrefab;
    public Transform[] eventPoints;

    void Start()
    {
        eventPoints = new Transform[3];
        // 假设这三个事件点分别是场景中的三个物体，在这里手动指定它们的 Transform


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
