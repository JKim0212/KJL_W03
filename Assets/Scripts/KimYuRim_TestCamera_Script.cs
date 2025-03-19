using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public float distance = 5f; // ī�޶�� �÷��̾� ���� �Ÿ�
    public float height = 2f; // ī�޶� ����
    public float mouseSensitivity = 100f; // ���콺 ����
    public float verticalAngleLimit = 80f; // ���� ȸ�� ����

    private float yaw; // ���� ȸ�� ����
    private float pitch; // ���� ȸ�� ����

    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ���� ȭ�� �߾ӿ� ����
    }

    void Update()
    {
        // ���콺 �Է����� ���� ����
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ���� ���� ����
        pitch = Mathf.Clamp(pitch, -verticalAngleLimit, verticalAngleLimit);
    }

    void LateUpdate()
    {
        // ī�޶� ��ġ ���
        Vector3 direction = new Vector3(0, height, -distance);
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = player.position + rotation * direction;
        transform.LookAt(player.position); // ī�޶� �׻� �÷��̾ �ٶ󺸵��� ����
    }
}
