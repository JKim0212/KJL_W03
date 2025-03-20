using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
    [SerializeField] Transform character;

    // 카메라 시점용 오브젝트
    void Update()
    {
        //transform.eulerAngles = new Vector3(10, 0, 0);
        transform.position = character.position;
    }
}
