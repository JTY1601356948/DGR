using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineEndTrigger : MonoBehaviour
{
    [Header("Scene Transition")]
    public string nextSceneName;
    public bool useFadeTransition = true;

    private PlayableDirector director;

    void Start()
    {
        director = GetComponent<PlayableDirector>();

        if (director != null)
        {
            director.stopped += OnTimelineFinished;
        }
    }

    void OnDestroy()
    {
        if (director != null)
        {
            director.stopped -= OnTimelineFinished;
        }
    }

    private void OnTimelineFinished(PlayableDirector pd)
    {
        if (pd == director && !string.IsNullOrEmpty(nextSceneName))
        {
            // ʹ�ô�����Ч���ĳ�������
            if (useFadeTransition && SceneTransitionManager.Instance != null)
            {
                SceneTransitionManager.Instance.LoadSceneWithFade(nextSceneName);
            }
            else
            {
                // ֱ�Ӽ��س���
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}