using UnityEngine;
using UnityEngine.SceneManagement;
using static EventController;

public class Fire : MonoBehaviour
{
    int fire = 0;
    int water = 0;
    int money = 0;
    int chance = 3;
    int firenumber = 0;
    bool OntriggerWater = false;
    private bool isGameOver = false; // 新增游戏状态标志
    public GameObject GameOverPanel;
    public GameObject GameOverFire;

    private float resetTimer = 0f;
    private bool isResetting = false;
    private float LeaveFireTime = 0f;

    // 音效相关变量
    public AudioClip fireSound;        // 着火音效，需在Unity编辑器中赋值
    public AudioClip waterSound;       // 泼水音效，需在Unity编辑器中赋值
    public float minVolume = 0.2f;     // 最小音量
    public float maxVolume = 1.0f;     // 最大音量
    public float maxDistance = 20f;    // 最大影响距离
    private AudioSource fireAudioSource;
    private AudioSource waterAudioSource;
    private Transform playerTransform; // 玩家位置（用于计算音量）

    private void Start()
    {
        // 初始化音效组件
        SetupFireSound();
        SetupWaterSound();

        // 获取玩家位置引用（假设玩家有"Player"标签）
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void SetupFireSound()
    {
        // 创建着火音效源组件
        fireAudioSource = gameObject.AddComponent<AudioSource>();
        fireAudioSource.clip = fireSound;
        fireAudioSource.loop = true;    // 循环播放
        fireAudioSource.volume = 0;     // 初始静音
        fireAudioSource.playOnAwake = false;
    }

    private void SetupWaterSound()
    {
        // 创建泼水音效源组件
        waterAudioSource = gameObject.AddComponent<AudioSource>();
        waterAudioSource.clip = waterSound;
        waterAudioSource.loop = false;  // 不循环
        waterAudioSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            OntriggerWater = true;
        }
        if (collision.CompareTag("FireCollision"))
        {
            firenumber += 1;
            isResetting = true;
        }
        if (collision.CompareTag("Fire"))
        {
            if (water > 0)
            {
                // 播放泼水音效
                PlayWaterSound();

                Destroy(collision.gameObject);
                LeaveFireTime = 0;
                water -= 1;
                isResetting = false;
                firenumber -= 1;
            }
            else if (water == 0)
            {
                Debug.Log("请寻找水源");
            }
        }
    }

    void HandleResetTimer()
    {
        if (isResetting)
        {
            LeaveFireTime += Time.deltaTime;

            // 控制着火音效播放
            ControlFireSound();

            if (LeaveFireTime >= 20f && firenumber > 0)
            {
                chance -= 100;
            }
        }
        else if (fireAudioSource != null && fireAudioSource.isPlaying)
        {
            // 停止着火音效
            fireAudioSource.Stop();
        }
    }

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
        // 暂停游戏（可选）
        Time.timeScale = 0f;
        Debug.Log("游戏失败！");
        ShowGameOverUI();
    }

    void ShowGameOverUI()
    {
        GameOverPanel.SetActive(true);
        GameOverFire.SetActive(true);
        
    }

    void ControlFireSound()
    {
        if (LeaveFireTime > 0)
        {
            // 计算音量（基于燃烧时间和距离）
            float volume = CalculateFireSoundVolume();

            // 播放音效（如果未播放）
            if (!fireAudioSource.isPlaying)
            {
                fireAudioSource.Play();
            }

            // 更新音量
            fireAudioSource.volume = volume;
        }
    }

    void PlayWaterSound()
    {
        if (waterAudioSource && waterSound)
        {
            // 计算音量（基于距离）
            float volume = 1f;
            if (playerTransform != null)
            {
                float distance = Vector3.Distance(transform.position, playerTransform.position);
                volume = Mathf.Clamp01(1 - (distance / maxDistance));
            }

            waterAudioSource.volume = volume;
            waterAudioSource.Play();
        }
    }

    float CalculateFireSoundVolume()
    {
        // 基础音量（随燃烧时间增长）
        float timeFactor = Mathf.Clamp01(LeaveFireTime / 20f);
        float baseVolume = Mathf.Lerp(minVolume, maxVolume, timeFactor);

        // 距离衰减（如果有玩家位置）
        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            float distanceFactor = Mathf.Clamp01(1 - (distance / maxDistance));
            return baseVolume * distanceFactor;
        }

        return baseVolume;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OntriggerWater = false;
    }

    void Update()
    {
        if (OntriggerWater && Input.GetKeyDown(KeyCode.E))
        {
            water++;
            if (water > 1)
                water = 1;
            Debug.Log("接水");
        }
        chanceCheck();
        HandleResetTimer();
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 30;
        GUI.Label(new Rect(20, 20, 500, 500), "剩余水源:" + water + "桶");
        GUI.Label(new Rect(20, 50, 500, 500), "resettime+chance" + LeaveFireTime + "+" + chance);
    }
}