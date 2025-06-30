using UnityEngine;
using UnityEngine.SceneManagement; // ���볡�����������ռ�

public class SceneLoader1 : MonoBehaviour
{
    // �� Inspector ��ָ��Ŀ�곡�������ƣ���������
    public string targetSceneName = "Scene2"; // ���Ըĳ���Ŀ�곡��������
    // ����ʹ�����������֪�������� Build Settings �е�˳��
    // public int targetSceneIndex = 1;

    // ����������ڰ�ť�����ʱ����
    public void LoadScene()
    {
        // ʹ�ó������Ƽ���
        SceneManager.LoadScene(targetSceneName);

        // �����ʹ��������ʽ����������д��
        // SceneManager.LoadScene(targetSceneIndex);
    }
}