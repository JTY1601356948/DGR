using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("�Ӳ�����")]
    [Range(0f, 2f)]
    [Tooltip("ǰ��:1.5, �о�:1.0, ����:0.5")]
    public float parallaxFactor = 1f;

    [Tooltip("�Ӳ��ƶ�����(0=����)")]
    public float movementClamp = 0f;

    [Header("���޹�������")]
    public bool infiniteScroll = false;
    public Transform backgroundPartner;

    private Camera mainCamera;
    private Vector3 cameraStartPos;
    private Vector3 layerStartPos;
    private Vector3 layerOffset;
    private Vector3 lastCameraPos;
    private float spriteWidth;

    void Start()
    {
        mainCamera = Camera.main;
        cameraStartPos = mainCamera.transform.position;
        layerStartPos = transform.position;
        layerOffset = layerStartPos - cameraStartPos;

        if (infiniteScroll)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                spriteWidth = sr.bounds.size.x;

                // �������ͼ��
                if (backgroundPartner == null)
                {
                    GameObject partner = Instantiate(gameObject, transform.parent);
                    backgroundPartner = partner.transform;

                    // λ�õ���
                    Vector3 partnerPos = transform.position;
                    partnerPos.x += spriteWidth;
                    backgroundPartner.position = partnerPos;
                }
            }
        }
    }

    void LateUpdate()
    {
        // �������λ��
        Vector3 cameraMovement = mainCamera.transform.position - lastCameraPos;
        cameraMovement.z = 0; // ����2Dƽ��

        // Ӧ���Ӳ�
        Vector3 newPosition = transform.position + cameraMovement * parallaxFactor;

        // Ӧ���ƶ�����
        if (movementClamp > 0)
        {
            float maxMovement = movementClamp;
            Vector3 allowedMovement = newPosition - layerStartPos;

            if (allowedMovement.magnitude > maxMovement)
            {
                allowedMovement = allowedMovement.normalized * maxMovement;
                newPosition = layerStartPos + allowedMovement;
            }
        }

        // ����λ��
        transform.position = newPosition;
        lastCameraPos = mainCamera.transform.position;

        // �������޹���
        if (infiniteScroll && backgroundPartner != null)
        {
            HandleInfiniteScrolling();
        }
    }

    private void HandleInfiniteScrolling()
    {
        float cameraDistance = mainCamera.transform.position.x - cameraStartPos.x;
        float layerDistance = transform.position.x - layerStartPos.x;

        // ��������ȫ�뿪��Ļʱ���¶�λ
        if (cameraDistance - layerDistance * parallaxFactor > spriteWidth)
        {
            Vector3 newPos = transform.position;
            newPos.x += spriteWidth * 2f;
            transform.position = newPos;
        }
        else if (cameraDistance - layerDistance * parallaxFactor < -spriteWidth)
        {
            Vector3 newPos = transform.position;
            newPos.x -= spriteWidth * 2f;
            transform.position = newPos;
        }

        // ȷ�����λ��
        Vector3 partnerPos = transform.position;
        partnerPos.x += (transform.position.x < backgroundPartner.position.x) ? spriteWidth : -spriteWidth;
        backgroundPartner.position = partnerPos;
    }
}