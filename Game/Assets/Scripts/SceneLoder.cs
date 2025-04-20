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

    [SerializeField] private Animator animator; // ����ͨ��Inspector��ֵ

    void Start()
    {
        //GameObject.DontDestroyOnLoad(this.gameObject);
        if (eventObject != null) // ��ֹ������
            GameObject.DontDestroyOnLoad(eventObject);

        btnA.onClick.AddListener(LoadSceneA); // ����������ƴд
    }

    private void LoadSceneA()
    {
        StartCoroutine(LoadScene(1)); // ʵ�ʵ��ó�������Э��
    }

    IEnumerator LoadScene(int index)
    {
        animator.SetBool("FadeIn", true);
        animator.SetBool("FadeOut", false);

        yield return new WaitForSeconds(1f); // ��ȷʹ��float����

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        // �ȴ��첽�������
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        animator.SetBool("FadeIn", false);
        animator.SetBool("FadeOut", true);
    }

    // Update�����ѿգ���ɾ��
}