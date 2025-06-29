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
    private bool isGameOver = false; // ������Ϸ״̬��־
    public GameObject GameOverPanel;
    public GameObject GameOverFire;

    private float resetTimer = 0f;
    private bool isResetting = false;
    private float LeaveFireTime = 0f;

    // ��Ч��ر���
    public AudioClip fireSound;        // �Ż���Ч������Unity�༭���и�ֵ
    public AudioClip waterSound;       // ��ˮ��Ч������Unity�༭���и�ֵ
    public float minVolume = 0.2f;     // ��С����
    public float maxVolume = 1.0f;     // �������
    public float maxDistance = 20f;    // ���Ӱ�����
    private AudioSource fireAudioSource;
    private AudioSource waterAudioSource;
    private Transform playerTransform; // ���λ�ã����ڼ���������

    private void Start()
    {
        // ��ʼ����Ч���
        SetupFireSound();
        SetupWaterSound();

        // ��ȡ���λ�����ã����������"Player"��ǩ��
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void SetupFireSound()
    {
        // �����Ż���ЧԴ���
        fireAudioSource = gameObject.AddComponent<AudioSource>();
        fireAudioSource.clip = fireSound;
        fireAudioSource.loop = true;    // ѭ������
        fireAudioSource.volume = 0;     // ��ʼ����
        fireAudioSource.playOnAwake = false;
    }

    private void SetupWaterSound()
    {
        // ������ˮ��ЧԴ���
        waterAudioSource = gameObject.AddComponent<AudioSource>();
        waterAudioSource.clip = waterSound;
        waterAudioSource.loop = false;  // ��ѭ��
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
                // ������ˮ��Ч
                PlayWaterSound();

                Destroy(collision.gameObject);
                LeaveFireTime = 0;
                water -= 1;
                isResetting = false;
                firenumber -= 1;
            }
            else if (water == 0)
            {
                Debug.Log("��Ѱ��ˮԴ");
            }
        }
    }

    void HandleResetTimer()
    {
        if (isResetting)
        {
            LeaveFireTime += Time.deltaTime;

            // �����Ż���Ч����
            ControlFireSound();

            if (LeaveFireTime >= 20f && firenumber > 0)
            {
                chance -= 100;
            }
        }
        else if (fireAudioSource != null && fireAudioSource.isPlaying)
        {
            // ֹͣ�Ż���Ч
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
        // ��ͣ��Ϸ����ѡ��
        Time.timeScale = 0f;
        Debug.Log("��Ϸʧ�ܣ�");
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
            // ��������������ȼ��ʱ��;��룩
            float volume = CalculateFireSoundVolume();

            // ������Ч�����δ���ţ�
            if (!fireAudioSource.isPlaying)
            {
                fireAudioSource.Play();
            }

            // ��������
            fireAudioSource.volume = volume;
        }
    }

    void PlayWaterSound()
    {
        if (waterAudioSource && waterSound)
        {
            // �������������ھ��룩
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
        // ������������ȼ��ʱ��������
        float timeFactor = Mathf.Clamp01(LeaveFireTime / 20f);
        float baseVolume = Mathf.Lerp(minVolume, maxVolume, timeFactor);

        // ����˥������������λ�ã�
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
            Debug.Log("��ˮ");
        }
        chanceCheck();
        HandleResetTimer();
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 30;
        GUI.Label(new Rect(20, 20, 500, 500), "ʣ��ˮԴ:" + water + "Ͱ");
        GUI.Label(new Rect(20, 50, 500, 500), "resettime+chance" + LeaveFireTime + "+" + chance);
    }
}