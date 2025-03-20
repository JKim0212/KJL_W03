using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public float distance = 5f; // 카메라와 플레이어 간의 거리
    public float height = 2f; // 카메라 높이
    public float mouseSensitivity = 100f; // 마우스 감도
    public float verticalAngleLimit = 80f; // 수직 회전 제한

    private float yaw; // 수평 회전 각도
    private float pitch; // 수직 회전 각도

    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 화면 중앙에 고정
    }

    void Update()
    {
        // 마우스 입력으로 각도 변경
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 수직 각도 제한
        pitch = Mathf.Clamp(pitch, -verticalAngleLimit, verticalAngleLimit);
    }

    void LateUpdate()
    {
        // 카메라 위치 계산
        Vector3 direction = new Vector3(0, height, -distance);
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = player.position + rotation * direction;
        transform.LookAt(player.position); // 카메라가 항상 플레이어를 바라보도록 설정
    }
}
