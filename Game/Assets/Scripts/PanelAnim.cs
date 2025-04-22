using UnityEngine;
using System.Collections;

public class PanelAnim : MonoBehaviour
{
    public AnimationCurve showCurve;    // 显示动画曲线
    public AnimationCurve hideCurve;    // 隐藏动画曲线
    public float animationSpeed = 1f;   // 动画播放速度
    public GameObject panel;            // 要控制的面板对象
    private bool open=false;

    IEnumerator ShowPanel(GameObject gameObject)
    {
        float timer = 0f;
        

        while (timer <= 1f)
        {
            // 使用动画曲线插值计算当前缩放值
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
            // 使用隐藏曲线控制缩放
            gameObject.transform.localScale = Vector3.one * hideCurve.Evaluate(timer);

            timer += Time.deltaTime * animationSpeed;
            yield return null;
        }
    }

    private void Update()
    {
        // 按B键切换面板状态
        if (Input.GetKeyDown(KeyCode.B))
        {
            // 切换状态
            open = !open;

            // 根据当前状态启动对应协程
            if (open)
            {
                StartCoroutine(ShowPanel(panel));
            }
            else
            {
                StartCoroutine(HidePanel(panel));
            }
        }
    }

}
