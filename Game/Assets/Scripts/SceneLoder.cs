using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Collections;


public class SceneLoder : MonoBehaviour

{
    public GameObject eventObject;
    public Button btnA;

    [SerializeField] private Animator animator; // 建议通过Inspector赋值

    void Start()
    {
        //GameObject.DontDestroyOnLoad(this.gameObject);
        if (eventObject != null) // 防止空引用
            GameObject.DontDestroyOnLoad(eventObject);

        btnA.onClick.AddListener(LoadSceneA); // 修正方法名拼写
    }

    private void LoadSceneA()
    {
        StartCoroutine(LoadScene(1)); // 实际调用场景加载协程
    }

    IEnumerator LoadScene(int index)
    {
        animator.SetBool("FadeIn", true);
        animator.SetBool("FadeOut", false);

        yield return new WaitForSeconds(1f); // 明确使用float类型

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        // 等待异步加载完成
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        animator.SetBool("FadeIn", false);
        animator.SetBool("FadeOut", true);
    }

    // Update方法已空，可删除
}