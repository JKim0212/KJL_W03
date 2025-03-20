using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [Header("Basic Components")]
    private Rigidbody rb;
    CharacterGround ground;
    public Vector3 velocity;
    public bool onGround = true;


    [Header("Running")]
    public float directionX; // Input �� Ȯ��(-1 ~ 1)
    public float directionZ; // Input �� Ȯ��(-1 ~ 1)
    public float maxSpeed = 14f; // �ְ� �ӵ�
    public float maxAcceleration = 85f; // ���ӵ�(�󸶳� ������ �ְ�ӵ� ����)
    public float maxDecceleration = 85; // ���ӵ�(�󸶳� ������ ���� ����)
    public float maxTurnSpeed = 260f; // ���� ��ȯ �ӵ�
    public float maxAirAcceleration = 50; // ���� ���ӵ�(���߿��� �󸶳� ������ �ְ�ӵ� ����)
    public float maxAirDeceleration = 50; // ���� ���ӵ�(���߿��� �󸶳� ������ ���� ����)
    public float maxAirTurnSpeed = 80f; // ���� ���� ��ȯ �ӵ�
    private float friction; // ������(������ ����)
    //------------------------
    private Vector3 desiredVelocity;
    private float maxSpeedChange;
    private float acceleration;
    private float deceleration;
    private float turnSpeed;

    


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ground = GetComponent<CharacterGround>();
    }


    private void FixedUpdate()
    {
        //Get Kit's current ground status from her ground script
        onGround = ground.GetOnGround();

        //Get velocity from Kit's Rigidbody 
        velocity = rb.linearVelocity;

        if (onGround)
        {
            Move();
        }
    }


    private void Move()
    {
        // isGrounded ���� ����Ǵ� �� ����(acc, dec, turn)
        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        deceleration = onGround ? maxDecceleration : maxAirDeceleration;
        turnSpeed = onGround ? maxTurnSpeed : maxAirTurnSpeed;

        // �¿�, ���� Input �� �ޱ�(-1 ~ 1 ����)
        directionX = Input.GetAxis("Horizontal");
        directionZ = Input.GetAxis("Vertical");

        // ī�޶��� ������ �������� �̵�
        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
        Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);

        forward.y = 0; // Y�� ������ ����
        right.y = 0; // Y�� ������ ����

        forward.Normalize();
        right.Normalize();

        // ���ϴ� �ӵ� ���
        desiredVelocity = (forward * directionZ + right * directionX) * maxSpeed;

        // Y�� �ӵ� �߰�
        desiredVelocity.y = rb.linearVelocity.y; // ���� Y�� �ӵ��� ����

        // X�� ������
        if (directionX != 0 || directionZ != 0)
        {
            // ���� ����� �Է� ������ �ٸ� ���, �ʹ����� �ν�
            if (Mathf.Sign(directionX) != Mathf.Sign(velocity.x) || Mathf.Sign(directionZ) != Mathf.Sign(velocity.z))
            {
                maxSpeedChange = turnSpeed * Time.deltaTime;
            }
            else
            {
                // ���� ����� �Է� ������ ���� ���, ���� ����
                maxSpeedChange = acceleration * Time.deltaTime;
            }
        }
        else
        {
            // �Է��� ���� ������, ����
            maxSpeedChange = deceleration * Time.deltaTime;
        }

        // maxSpeedChange ��ŭ X��, Z�� �ӵ� ���
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        // ���ο� �ӵ��� rigidbody�� ������Ʈ
        velocity.y = rb.linearVelocity.y; // Y�� �ӵ� ����
        rb.linearVelocity = velocity;

        // �÷��̾� ȸ��
        if (velocity.x != 0 || velocity.z != 0) // �̵��ϴ� ��쿡�� ȸ��
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(velocity.x, 0, velocity.z));
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        /*
        // �÷��̾� ȸ��
        if (velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        */


        Debug.Log("x�� �ӵ�: " + velocity.x + ", z�� �ӵ�: " + velocity.z);
    }

}
