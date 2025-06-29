using UnityEngine;
using static EventController;

public class Fire : MonoBehaviour
{
    // ��������
    int water = 0;
    int firenumber = 0;
    bool OntriggerWater = false;
    private bool isGameOver = false;
    public GameObject GameOverPanel;
    public GameObject GameOverFire;
    public GameObject GameOverTianGui;

    // ������ؼ�ʱ��״̬
    private float resetTimer = 0f;
    private bool isResetting = false;
    private float LeaveFireTime = 0f;

    // ��Ч
    public AudioClip fireSound;
    public AudioClip waterSound;
    public AudioClip collectWaterSound; // ��ˮ��Ч
    public AudioClip destroyXiaoTouSound; // �ݻ�С͵��Ч
    private AudioSource fireAudioSource;
    private AudioSource waterAudioSource;
    private AudioSource collectWaterAudioSource;
    private AudioSource destroyXiaoTouAudioSource;

    // �������߼�
    private bool isInTianGuiCollision = false;
    private float stayTimeInTianGui = 0f;

    // С͵����߼�
    private bool isInXiaoTouCollision = false; // �������Ƿ���С͵��ײ����
    private float stayTimeInXiaoTou = 0f;      // ��������С͵����ͣ��ʱ��

    private void Start()
    {
        // ��ʼ����Ч���
        SetupFireSound();
        SetupWaterSound();
        SetupCollectWaterSound();
        SetupDestroyXiaoTouSound(); // ��������ʼ���ݻ�С͵��Ч
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

    // ��������ʼ���ݻ�С͵��Ч
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
                Debug.Log("��Ѱ��ˮԴ");
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
            Debug.Log("���������ײ����");
        }
        else if (collision.CompareTag("XiaoTouCollision"))
        {
            isInXiaoTouCollision = true; // ��������ǽ���С͵��ײ����
            stayTimeInXiaoTou = 0f;      // ����������С͵�����ʱ��
            Debug.Log("����С͵��ײ����");
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
            isInXiaoTouCollision = false; // ����������뿪С͵��ײ����
            stayTimeInXiaoTou = 0f;      // ����������С͵�����ʱ��
            Debug.Log("�뿪С͵��ײ����");
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
        Debug.Log("��Ϸʧ�ܣ�");
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
            Debug.Log("��ˮ");

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
            Debug.Log($"���������ͣ��ʱ��: {stayTimeInTianGui:F1} ��");

            if (stayTimeInTianGui >= 5f)
            {
                DestroyTianGui();
            }
        }

        // ������С͵�ݻ��߼�
        if (isInXiaoTouCollision)
        {
            // ��ʾ��С͵�����ͣ��ʱ�䣨��ѡ��
            stayTimeInXiaoTou += Time.deltaTime;
            Debug.Log($"��С͵����ͣ��ʱ��: {stayTimeInXiaoTou:F1} ��");

            // ���F������
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
            Debug.Log("�ɹ��ݻٱ�ǩΪ 'TianGui' �Ķ���");
        }
        else
        {
            Debug.LogError("�Ҳ�����ǩΪ 'TianGui' �Ķ���");
        }
    }

    // �������ݻ�С͵�ķ���
    void DestroyXiaoTou()
    {
        GameObject xiaoTou = GameObject.FindWithTag("XiaoTou");
        if (xiaoTou != null)
        {
            // ���Ŵݻ���Ч����ѡ��
            if (destroyXiaoTouAudioSource && destroyXiaoTouSound)
            {
                destroyXiaoTouAudioSource.Play();
            }

            Destroy(xiaoTou);
            isInXiaoTouCollision = false;
            stayTimeInXiaoTou = 0f;
            Debug.Log("�ɹ��ݻٱ�ǩΪ 'XiaoTou' �Ķ���");
        }
        else
        {
            Debug.LogError("�Ҳ�����ǩΪ 'XiaoTou' �Ķ���");
        }
    }
}