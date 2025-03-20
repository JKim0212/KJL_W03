using UnityEngine;

public class EditorStyleCamera : MonoBehaviour
{
    public float rotateSpeed = 2f; // ȸ�� �ӵ�
    public float moveSpeed = 5f; // �̵� �ӵ�
    public float zoomSpeed = 10f; // �� �ӵ�

    private Vector3 lastMousePosition; // ���콺 �̵� ����

    void Update()
    {
        // ������ Ŭ������ ȸ��
        if (Input.GetMouseButton(1)) // ������ ��ư
        {
            float mouseX = Input.GetAxis("Mouse X") * rotateSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed;

            Vector3 rotation = transform.eulerAngles;
            rotation.y += mouseX; // �¿� ȸ��
            rotation.x -= mouseY; // ���� ȸ��
            rotation.x = Mathf.Clamp(rotation.x, -90f, 90f); // ���� ȸ�� ����
            transform.eulerAngles = rotation;
        }

        // �߰� ��ư���� �̵� (�д�)
        if (Input.GetMouseButton(2)) // �߰� ��ư
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 move = new Vector3(-delta.x, -delta.y, 0f) * moveSpeed * Time.deltaTime;
            transform.Translate(move);
        }
        lastMousePosition = Input.mousePosition;

        // ���콺 �ٷ� ����/�ܾƿ�
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.position += transform.forward * scroll * zoomSpeed;
    }
}