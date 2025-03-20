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
        // colliderOffset ġȯ
        Vector3 offset1 = new Vector3(colliderOffset.x, 0, colliderOffset.z);
        Vector3 offset2 = new Vector3(colliderOffset.x, 0, -colliderOffset.z);
        Vector3 offset3 = new Vector3(-colliderOffset.x, 0, colliderOffset.z);
        Vector3 offset4 = new Vector3(-colliderOffset.z, 0, -colliderOffset.z);

        // �÷��̾ groundLayer�� ��ġ�ϴ°� Ȯ��
        // �����ڵ�� 2�� ���, ���� �ڵ�� 4�� ���
        bool _rayHit1 = Physics.Raycast(transform.position + offset1, Vector3.down, groundLength, groundLayer);
        bool _rayHit2 = Physics.Raycast(transform.position + offset2, Vector3.down, groundLength, groundLayer);
        bool _rayHit3 = Physics.Raycast(transform.position + offset3, Vector3.down, groundLength, groundLayer);
        bool _rayHit4 = Physics.Raycast(transform.position + offset4, Vector3.down, groundLength, groundLayer);

        onGround = _rayHit1 || _rayHit2 || _rayHit3 || _rayHit4;


    }

    private void OnDrawGizmos()
    {
        // colliderOffset ġȯ
        Vector3 offset1 = new Vector3(colliderOffset.x, 0, colliderOffset.z);
        Vector3 offset2 = new Vector3(colliderOffset.x, 0, -colliderOffset.z);
        Vector3 offset3 = new Vector3(-colliderOffset.x, 0, colliderOffset.z);
        Vector3 offset4 = new Vector3(-colliderOffset.z, 0, -colliderOffset.z);


        // ����� �׸���
        // ������ �ʷϻ�, ���� ������ ������
        if (onGround) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + offset1, transform.position + offset1 + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position + offset2, transform.position + offset2 + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position + offset3, transform.position + offset3 + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position + offset4, transform.position + offset4 + Vector3.down * groundLength);
    }

    //Send ground detection to other scripts
    public bool GetOnGround() { return onGround; }
}
