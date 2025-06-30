using UnityEngine;
using UnityEngine.SceneManagement;

public class ReStartGame : MonoBehaviour
{
    public GameObject GameOver;
    public void RestartScene()
    {
        SceneManager.LoadScene("Menu");
        GameOver.SetActive(false);
    }
}
