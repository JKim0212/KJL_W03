using UnityEngine;

public class EditorStyleCamera : MonoBehaviour
{
    public float rotateSpeed = 2f; // 회전 속도
    public float moveSpeed = 5f; // 이동 속도
    public float zoomSpeed = 10f; // 줌 속도

    private Vector3 lastMousePosition; // 마우스 이동 계산용

    void Update()
    {
        // 오른쪽 클릭으로 회전
        if (Input.GetMouseButton(1)) // 오른쪽 버튼
        {
            float mouseX = Input.GetAxis("Mouse X") * rotateSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed;

            Vector3 rotation = transform.eulerAngles;
            rotation.y += mouseX; // 좌우 회전
            rotation.x -= mouseY; // 상하 회전
            rotation.x = Mathf.Clamp(rotation.x, -90f, 90f); // 상하 회전 제한
            transform.eulerAngles = rotation;
        }

        // 중간 버튼으로 이동 (패닝)
        if (Input.GetMouseButton(2)) // 중간 버튼
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 move = new Vector3(-delta.x, -delta.y, 0f) * moveSpeed * Time.deltaTime;
            transform.Translate(move);
        }
        lastMousePosition = Input.mousePosition;

        // 마우스 휠로 줌인/줌아웃
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.position += transform.forward * scroll * zoomSpeed;
    }
}