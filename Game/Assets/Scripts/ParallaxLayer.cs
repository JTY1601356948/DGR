using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("视差设置")]
    [Range(0f, 2f)]
    [Tooltip("前景:1.5, 中景:1.0, 背景:0.5")]
    public float parallaxFactor = 1f;

    [Tooltip("视差移动限制(0=无限)")]
    public float movementClamp = 0f;

    [Header("无限滚动设置")]
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

                // 创建伙伴图层
                if (backgroundPartner == null)
                {
                    GameObject partner = Instantiate(gameObject, transform.parent);
                    backgroundPartner = partner.transform;

                    // 位置调整
                    Vector3 partnerPos = transform.position;
                    partnerPos.x += spriteWidth;
                    backgroundPartner.position = partnerPos;
                }
            }
        }
    }

    void LateUpdate()
    {
        // 计算相机位移
        Vector3 cameraMovement = mainCamera.transform.position - lastCameraPos;
        cameraMovement.z = 0; // 保持2D平面

        // 应用视差
        Vector3 newPosition = transform.position + cameraMovement * parallaxFactor;

        // 应用移动限制
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

        // 更新位置
        transform.position = newPosition;
        lastCameraPos = mainCamera.transform.position;

        // 处理无限滚动
        if (infiniteScroll && backgroundPartner != null)
        {
            HandleInfiniteScrolling();
        }
    }

    private void HandleInfiniteScrolling()
    {
        float cameraDistance = mainCamera.transform.position.x - cameraStartPos.x;
        float layerDistance = transform.position.x - layerStartPos.x;

        // 当背景完全离开屏幕时重新定位
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

        // 确保伙伴位置
        Vector3 partnerPos = transform.position;
        partnerPos.x += (transform.position.x < backgroundPartner.position.x) ? spriteWidth : -spriteWidth;
        backgroundPartner.position = partnerPos;
    }
}