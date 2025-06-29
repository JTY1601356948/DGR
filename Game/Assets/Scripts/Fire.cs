using UnityEngine;
using static EventController;

public class Fire : MonoBehaviour
{
    // 基础属性
    int water = 0;
    int firenumber = 0;
    bool OntriggerWater = false;
    private bool isGameOver = false;
    public GameObject GameOverPanel;
    public GameObject GameOverFire;
    public GameObject GameOverTianGui;

    // 火灾相关计时与状态
    private float resetTimer = 0f;
    private bool isResetting = false;
    private float LeaveFireTime = 0f;

    // 音效
    public AudioClip fireSound;
    public AudioClip waterSound;
    public AudioClip collectWaterSound; // 接水音效
    public AudioClip destroyXiaoTouSound; // 摧毁小偷音效
    private AudioSource fireAudioSource;
    private AudioSource waterAudioSource;
    private AudioSource collectWaterAudioSource;
    private AudioSource destroyXiaoTouAudioSource;

    // 天鬼相关逻辑
    private bool isInTianGuiCollision = false;
    private float stayTimeInTianGui = 0f;

    // 小偷相关逻辑
    private bool isInXiaoTouCollision = false; // 新增：是否在小偷碰撞区域
    private float stayTimeInXiaoTou = 0f;      // 新增：在小偷区域停留时间

    private void Start()
    {
        // 初始化音效组件
        SetupFireSound();
        SetupWaterSound();
        SetupCollectWaterSound();
        SetupDestroyXiaoTouSound(); // 新增：初始化摧毁小偷音效
    }

    private void SetupFireSound()
    {
        fireAudioSource = gameObject.AddComponent<AudioSource>();
        fireAudioSource.clip = fireSound;
        fireAudioSource.loop = true;
        fireAudioSource.playOnAwake = false;
    }

    private void SetupWaterSound()
    {
        waterAudioSource = gameObject.AddComponent<AudioSource>();
        waterAudioSource.clip = waterSound;
        waterAudioSource.loop = false;
        waterAudioSource.playOnAwake = false;
    }

    private void SetupCollectWaterSound()
    {
        collectWaterAudioSource = gameObject.AddComponent<AudioSource>();
        collectWaterAudioSource.clip = collectWaterSound;
        collectWaterAudioSource.loop = false;
        collectWaterAudioSource.playOnAwake = false;
    }

    // 新增：初始化摧毁小偷音效
    private void SetupDestroyXiaoTouSound()
    {
        destroyXiaoTouAudioSource = gameObject.AddComponent<AudioSource>();
        destroyXiaoTouAudioSource.clip = destroyXiaoTouSound;
        destroyXiaoTouAudioSource.loop = false;
        destroyXiaoTouAudioSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            OntriggerWater = true;
        }
        else if (collision.CompareTag("FireCollision"))
        {
            firenumber += 1;
            isResetting = true;
        }
        else if (collision.CompareTag("Fire"))
        {
            if (water > 0)
            {
                waterAudioSource.Play();
                Destroy(collision.gameObject);
                LeaveFireTime = 0;
                water -= 1;
                isResetting = false;
                firenumber -= 1;
            }
            else
            {
                Debug.Log("请寻找水源");
            }
        }
        else if (collision.CompareTag("TianGui"))
        {
            GameOverPanel.SetActive(true);
            GameOverTianGui.SetActive(true);
        }
        else if (collision.CompareTag("TianGuiCollision"))
        {
            isInTianGuiCollision = true;
            stayTimeInTianGui = 0f;
            Debug.Log("进入天鬼碰撞区域");
        }
        else if (collision.CompareTag("XiaoTouCollision"))
        {
            isInXiaoTouCollision = true; // 新增：标记进入小偷碰撞区域
            stayTimeInXiaoTou = 0f;      // 新增：重置小偷区域计时器
            Debug.Log("进入小偷碰撞区域");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            OntriggerWater = false;
        }
        else if (collision.CompareTag("TianGuiCollision"))
        {
            isInTianGuiCollision = false;
            stayTimeInTianGui = 0f;
        }
        else if (collision.CompareTag("XiaoTouCollision"))
        {
            isInXiaoTouCollision = false; // 新增：标记离开小偷碰撞区域
            stayTimeInXiaoTou = 0f;      // 新增：重置小偷区域计时器
            Debug.Log("离开小偷碰撞区域");
        }
    }

    void HandleResetTimer()
    {
        if (isResetting)
        {
            LeaveFireTime += Time.deltaTime;

            if (!fireAudioSource.isPlaying && LeaveFireTime > 0)
            {
                fireAudioSource.Play();
            }

            if (LeaveFireTime >= 20f && firenumber > 0)
            {
                chance -= 100;
            }
        }
        else if (fireAudioSource.isPlaying)
        {
            fireAudioSource.Stop();
        }
    }

    int chance = 3;
    void chanceCheck()
    {
        if (chance <= 0 && !isGameOver)
        {
            isGameOver = true;
            GameOver();
        }
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        Debug.Log("游戏失败！");
        ShowGameOverUI();
    }

    void ShowGameOverUI()
    {
        GameOverPanel.SetActive(true);
        GameOverFire.SetActive(true);
    }

    void Update()
    {
        if (OntriggerWater && Input.GetKeyDown(KeyCode.E))
        {
            water++;
            if (water > 1)
                water = 1;
            Debug.Log("接水");

            if (collectWaterAudioSource && collectWaterSound)
            {
                collectWaterAudioSource.Play();
            }
        }

        chanceCheck();
        HandleResetTimer();

        if (isInTianGuiCollision)
        {
            stayTimeInTianGui += Time.deltaTime;
            Debug.Log($"在天鬼区域停留时间: {stayTimeInTianGui:F1} 秒");

            if (stayTimeInTianGui >= 5f)
            {
                DestroyTianGui();
            }
        }

        // 新增：小偷摧毁逻辑
        if (isInXiaoTouCollision)
        {
            // 显示在小偷区域的停留时间（可选）
            stayTimeInXiaoTou += Time.deltaTime;
            Debug.Log($"在小偷区域停留时间: {stayTimeInXiaoTou:F1} 秒");

            // 检测F键按下
            if (Input.GetKeyDown(KeyCode.F))
            {
                DestroyXiaoTou();
            }
        }
    }

    void DestroyTianGui()
    {
        GameObject tianGui = GameObject.FindWithTag("TianGui");
        if (tianGui != null)
        {
            Destroy(tianGui);
            isInTianGuiCollision = false;
            stayTimeInTianGui = 0f;
            Debug.Log("成功摧毁标签为 'TianGui' 的对象！");
        }
        else
        {
            Debug.LogError("找不到标签为 'TianGui' 的对象！");
        }
    }

    // 新增：摧毁小偷的方法
    void DestroyXiaoTou()
    {
        GameObject xiaoTou = GameObject.FindWithTag("XiaoTou");
        if (xiaoTou != null)
        {
            // 播放摧毁音效（可选）
            if (destroyXiaoTouAudioSource && destroyXiaoTouSound)
            {
                destroyXiaoTouAudioSource.Play();
            }

            Destroy(xiaoTou);
            isInXiaoTouCollision = false;
            stayTimeInXiaoTou = 0f;
            Debug.Log("成功摧毁标签为 'XiaoTou' 的对象！");
        }
        else
        {
            Debug.LogError("找不到标签为 'XiaoTou' 的对象！");
        }
    }
}