using UnityEngine;
using UnityEngine.SceneManagement; // 引入场景管理命名空间

public class SceneLoader1 : MonoBehaviour
{
    // 在 Inspector 中指定目标场景的名称（或索引）
    public string targetSceneName = "Scene2"; // 可以改成你目标场景的名字
    // 或者使用索引（如果知道场景在 Build Settings 中的顺序）
    // public int targetSceneIndex = 1;

    // 这个方法会在按钮被点击时调用
    public void LoadScene()
    {
        // 使用场景名称加载
        SceneManager.LoadScene(targetSceneName);

        // 如果你使用索引方式，可以这样写：
        // SceneManager.LoadScene(targetSceneIndex);
    }
}