using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform Character;

    private void Update()
    {
        transform.position = Character.position;
    }
}
