using UnityEngine;

public class ControlCameraFollower : MonoBehaviour
{
[SerializeField] Transform character;
    // Update is called once per frame
    void Update()
    {
        transform.position = character.position;
    }
}
