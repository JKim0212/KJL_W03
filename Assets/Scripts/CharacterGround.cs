
using UnityEngine;

public class CharacterGround : MonoBehaviour
{
    private bool onGround;

    [Header("Collider Settings")]
    [SerializeField][Tooltip("Length of the ground-checking collider")] private float groundLength = 0.95f;
    [SerializeField][Tooltip("Distance between the ground-checking colliders")] private Vector3 colliderOffset;

    [Header("Layer Masks")]
    [SerializeField][Tooltip("Which layers are read as the ground")] private LayerMask groundLayer;


    private void Update()
    {
        // colliderOffset 치환
        Vector3 offset1 = new Vector3(colliderOffset.x, 0, colliderOffset.z);
        Vector3 offset2 = new Vector3(colliderOffset.x, 0, -colliderOffset.z);
        Vector3 offset3 = new Vector3(-colliderOffset.x, 0, colliderOffset.z);
        Vector3 offset4 = new Vector3(-colliderOffset.z, 0, -colliderOffset.z);

        // 플레이어가 groundLayer에 위치하는가 확인
        // 기존코드는 2개 사용, 현재 코드는 4개 사용
        Collider[] cols = Physics.OverlapBox(transform.position + new Vector3 (0f, -0.5f, 0f), new Vector3(0.94f/2, 0.3f, 0.94f/2), Quaternion.identity, groundLayer);
        // bool _rayHitBox = Physics.BoxCast(transform.position, new Vector3 (0.95f, 0.6f, 0.95f), Vector3.down, transform.rotation, groundLength, groundLayer);
        // bool _rayHit1 = Physics.Raycast(transform.position + offset1, Vector3.down, groundLength, groundLayer);
        // bool _rayHit2 = Physics.Raycast(transform.position + offset2, Vector3.down, groundLength, groundLayer);
        // bool _rayHit3 = Physics.Raycast(transform.position + offset3, Vector3.down, groundLength, groundLayer);
        // bool _rayHit4 = Physics.Raycast(transform.position + offset4, Vector3.down, groundLength, groundLayer);

        // onGround = _rayHit1 || _rayHit2 || _rayHit3 || _rayHit4;
        onGround = cols.Length != 0;


    }

    private void OnDrawGizmos()
    {
        // // colliderOffset 치환
        Vector3 offset1 = new Vector3(colliderOffset.x, 0, colliderOffset.z);
        Vector3 offset2 = new Vector3(colliderOffset.x, 0, -colliderOffset.z);
        Vector3 offset3 = new Vector3(-colliderOffset.x, 0, colliderOffset.z);
        Vector3 offset4 = new Vector3(-colliderOffset.z, 0, -colliderOffset.z);


        // // 기즈모 그리기
        // // 닿으면 초록색, 닿지 않으면 빨간색
        if (onGround) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        // Gizmos.DrawLine(transform.position + offset1, transform.position + offset1 + Vector3.down * groundLength);
        // Gizmos.DrawLine(transform.position + offset2, transform.position + offset2 + Vector3.down * groundLength);
        // Gizmos.DrawLine(transform.position + offset3, transform.position + offset3 + Vector3.down * groundLength);
        // Gizmos.DrawLine(transform.position + offset4, transform.position + offset4 + Vector3.down * groundLength);
        Gizmos.DrawWireCube(transform.position + new Vector3(0f, -0.5f, 0f), new Vector3 (0.94f, 0.6f, 0.94f));
    }

    //Send ground detection to other scripts
    public bool GetOnGround() { return onGround; }
}
