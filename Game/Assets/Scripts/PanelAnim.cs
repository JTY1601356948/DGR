using UnityEngine;
using System.Collections;

public class PanelAnim : MonoBehaviour
{
    public AnimationCurve showCurve;    // ��ʾ��������
    public AnimationCurve hideCurve;    // ���ض�������
    public float animationSpeed = 1f;   // ���������ٶ�
    public GameObject panel;            // Ҫ���Ƶ�������
    private bool open=false;

    IEnumerator ShowPanel(GameObject gameObject)
    {
        float timer = 0f;
        

        while (timer <= 1f)
        {
            // ʹ�ö������߲�ֵ���㵱ǰ����ֵ
            gameObject.transform.localScale = Vector3.one*showCurve.Evaluate(timer);

            timer += Time.deltaTime * animationSpeed;
            yield return null;
        }
    }

    IEnumerator HidePanel(GameObject gameObject)
    {
        float timer = 0f;
        
        while (timer <= 1f)
        {
            // ʹ���������߿�������
            gameObject.transform.localScale = Vector3.one * hideCurve.Evaluate(timer);

            timer += Time.deltaTime * animationSpeed;
            yield return null;
        }
    }

    private void Update()
    {
        // ������������ʾ����
        if (Input.GetKeyDown(KeyCode.B)&&!open)
        {
            open = true;
            StartCoroutine(ShowPanel(panel));
        }
        // �Ҽ�����������ض���
        else if (Input.GetKeyDown(KeyCode.N)&&open)
        {
            open= false;
            StartCoroutine(HidePanel(panel));
        }
    }
}
