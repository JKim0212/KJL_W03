using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
    [SerializeField] Transform character;

    // Update is called once per frame
    void Update()
    {
        transform.position = character.position;
    }
}
