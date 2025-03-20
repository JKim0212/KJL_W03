using UnityEngine;
using UnityEngine.InputSystem;

public class KimYuRim_TestPlayer_Script : MonoBehaviour
{
    [Header("Basic Components")]
    private Rigidbody rb;
    // �׶���
    public bool isGrounded;


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
    public Vector3 velocity;
    private float maxSpeedChange;
    private float acceleration;
    private float deceleration;
    private float turnSpeed;



    [Header("Jumping")]
    public float jumpHeight = 7.3f; // �ְ� ���� ����
    public float timeToJumpApex; // �ְ� ���̱��� �ɸ��� �ð�
    public float upwardMovementMultiplier = 1f; // ��� �߷� ���ϱ�
    public float downwardMovementMultiplier = 6.17f; // �ϰ� �߷� ���ϱ�
    public int maxAirJumps = 0; // �ִ� ���� Ƚ��
    public bool variablejumpHeight; // ����Ű�� �� ���� �������� �� ���ΰ�?
    public float jumpCutOff; // ����Ű �� ���� �߷� ���ϱ�
    public float speedLimit; // �ִ� ���� �ӵ� ����
    public float coyoteTime = 0.15f; // �ڿ��� Ÿ��
    public float jumpBuffer = 0.15f; // ���� ����
    //------------------------
    public float jumpSpeed;
    private float defaultGravityScale;
    private Vector3 customGravity;
    public float gravMultiplier;
    public bool canJumpAgain = false;
    private bool desiredJump;
    private float jumpBufferCounter;
    private float coyoteTimeCounter = 0;
    private bool pressingJump;
    public bool onGround;
    private bool currentlyJumping;




    [Header("Assists")]




    [Header("Juice")]
    public bool inginging;







    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        defaultGravityScale = 1f;
    }



    private void Update()
    {
        setPhysics();

        CheckJumpBufferCoyoteTime();
    }

    private void FixedUpdate()
    {
        Move();

        //Get velocity from Kit's Rigidbody 
        velocity = rb.linearVelocity;

        //Keep trying to do a jump, for as long as desiredJump is true
        if (desiredJump)
        {
            DoAJump();
            rb.linearVelocity = velocity;

            // �� �����ӿ����� �߷� ��� ��ŵ. �ڿ��� Ÿ���� �ι� ���� �ʵ��� ��
            return;
        }

        calculateGravity();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //This function is called when one of the jump buttons (like space or the A button) is pressed.

        //When we press the jump button, tell the script that we desire a jump.
        //Also, use the started and canceled contexts to know if we're currently holding the button
        if (context.started)
        {
            desiredJump = true;
            pressingJump = true;
        }

        if (context.canceled)
        {
            pressingJump = false;
        }
    }


    private void calculateGravity()
    {
        // Y ���⿡ ���� ĳ������ �߷� �ٲٱ�

        // ĳ���Ͱ� ��� ���̸�
        if (rb.linearVelocity.y > 0.01f)
        {
            if (onGround)
            {
                // ĳ���Ͱ� �����̴� �÷��� �� (����) ������ �߷� �� �ٲٱ� 
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                // variable jump height�� ����Ѵٸ�
                if (variablejumpHeight)
                {
                    // �÷��̾ ��� ���̰�, ����Ű ������ �߷� ���ϱ�
                    if (pressingJump && currentlyJumping)
                    {
                        gravMultiplier = upwardMovementMultiplier;
                    }
                    // �÷��̾ ����Ű ���� �ϰ��ϱ�
                    else
                    {
                        gravMultiplier = jumpCutOff;
                    }
                }
                else
                {
                    gravMultiplier = upwardMovementMultiplier;
                }
            }
        }

        // ĳ���Ͱ� �ϰ� ���̸�
        else if (rb.linearVelocity.y < -0.01f)
        {

            if (onGround)
            // ĳ���Ͱ� �����̴� �÷��� �� (����) ������ �߷� �� �ٲٱ� 
            {
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                // �׷��� �ʴٸ�, �߷� ���ϱ�
                gravMultiplier = downwardMovementMultiplier;
            }

        }
        // ���� �������� �ʴ´ٸ�, currentlyJumping�� ���ϰ�
        else
        {
            if (onGround)
            {
                currentlyJumping = false;
            }

            gravMultiplier = defaultGravityScale;
        }

        // ĳ���� �ӵ� ����
        // speed limit�� ������ y�� �ϰ� �ӵ� ����
        rb.linearVelocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, 100));
    }


    private void setPhysics()
    {
        // ���ο� �߷� ���
        Vector2 newGravity = new Vector2(0, (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex));

        // Ŀ���� �߷� ����
        customGravity = new Vector3(0, newGravity.y, 0) * gravMultiplier;

        // �� �����Ӹ��� Ŀ���� �߷� ����
        rb.AddForce(customGravity, ForceMode.Acceleration);
    }

    private void Move()
    {
        // isGrounded ���� ����Ǵ� �� ����(acc, dec, turn)
        acceleration = isGrounded ? maxAcceleration : maxAirAcceleration;
        deceleration = isGrounded ? maxDecceleration : maxAirDeceleration;
        turnSpeed = isGrounded ? maxTurnSpeed : maxAirTurnSpeed;

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



    private void DoAJump()
    {
        // ���� �ְų�, �ڿ��� Ÿ�� ���̰ų�, ���� ������ �����ϴٸ�
        if (onGround || (coyoteTimeCounter > 0.03f && coyoteTimeCounter < coyoteTime) || canJumpAgain)
        {
            desiredJump = false;
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;

            // ���� ���� ��� �� �ѹ� �� �����ϵ��� ��
            canJumpAgain = (maxAirJumps == 1 && canJumpAgain == false);

            // ������ �������� ���� �Ŀ� ���ϱ�
            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * Vector3.Magnitude(customGravity) * jumpHeight);

            // ���� �߿� ĳ���Ͱ� �� Ȥ�� �Ʒ��� �̵��Ѵٸ�(���� ���� ��), ���� ���ǵ� ����
            // ���� �ӵ��� ������� ���� ���� �����ϵ��� ��
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(rb.linearVelocity.y);
            }

            // ���ο� ���� ���ǵ带 ���ϱ�
            velocity.y += jumpSpeed;
            currentlyJumping = true;
        }

        if (jumpBuffer == 0)
        {
            // ���� ���۰� ������, ����Ű �����ڸ��� desiredJump ����
            desiredJump = false;
        }
    }


    private void CheckJumpBufferCoyoteTime()
    {
        // Check if we're on ground, using Kit's Ground script
        // onGround = ground.GetOnGround();

        // ���� ���۰� 0���� ũ�� ���� �Է��� �ǵ���
        if (jumpBuffer > 0)
        {
            // disireJump�� ���� �� ī��Ʈ, ī��Ʈ ������ DoAJump �Լ� ����
            if (desiredJump)
            {
                jumpBufferCounter += Time.deltaTime;

                if (jumpBufferCounter > jumpBuffer)
                {
                    // ī��Ʈ�� ���� ���ۺ��� ũ�� disireJump�� ����.
                    desiredJump = false;
                    jumpBufferCounter = 0;
                }
            }
        }

        // ���� ���� �ƴѵ�, ������ ������ ���, �ڿ��� Ÿ�� ����
        if (!currentlyJumping && !onGround)
        {
            coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            // ���� ��ų� �����ϸ� �ڿ��� Ÿ�� �ʱ�ȭ
            coyoteTimeCounter = 0;
        }
    }



    /*

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jum = 0; // �ٴڿ� ������ ���� Ƚ�� �ʱ�ȭ
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    */
}