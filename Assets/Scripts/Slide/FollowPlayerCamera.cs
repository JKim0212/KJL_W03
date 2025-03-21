using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform Character;

    void Update()
    {
        transform.position = Character.position;
    }
}
