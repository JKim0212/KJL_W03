using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
    [SerializeField] Transform character;

    // ī�޶� ������ ������Ʈ
    void Update()
    {
        //transform.eulerAngles = new Vector3(10, 0, 0);
        transform.position = character.position;
    }
}
