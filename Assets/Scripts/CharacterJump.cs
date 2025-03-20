using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class CharacterJump : MonoBehaviour
{
    [Header("Basic Components")]
    private Rigidbody rb;
    CharacterGround ground;
    public Vector3 velocity;
    public bool onGround = true;

    [Header("Jumping")]
    public float jumpHeight = 7.3f; // �ְ� ���� ����
    public float timeToJumpApex = 5f; // �ְ� ���̱��� �ɸ��� �ð�
    public float upwardMovementMultiplier = 1f; // ��� �߷� ���ϱ�
    public float downwardMovementMultiplier = 1f; // �ϰ� �߷� ���ϱ�
    public int maxAirJumps = 1; // �ִ� ���� Ƚ��
    public bool variablejumpHeight = false; // ����Ű�� �� ���� �������� �� ���ΰ�?
    public float jumpCutOff = 1; // ����Ű �� ���� �߷� ���ϱ�
    public float speedLimit = 20; // �ִ� ���� �ӵ� ����
    public float coyoteTime = 0.2f; // �ڿ��� Ÿ��
    public float jumpBuffer = 0.2f; // ���� ����
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
    private bool currentlyJumping;

    Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ground = GetComponent<CharacterGround>();
        defaultGravityScale = 1f;
        anim = GetComponent<Animator>();
    }



    private void Update()
    {
        setPhysics();

        // Check if we're on ground, using Kit's Ground script
        onGround = ground.GetOnGround();

        CheckJumpButton();

        CheckJumpBufferCoyoteTime();
        anim.SetBool("isOnGround", !currentlyJumping);
    }

    private void FixedUpdate()
    {
        // Get velocity from Kit's Rigidbody 
        velocity = rb.linearVelocity;

        // desiredJump�� true�� ���� DoAJump ����
        if (desiredJump)
        {
            DoAJump();
            rb.linearVelocity = velocity;

            // �� �����ӿ����� �߷� ��� ��ŵ. �ڿ��� Ÿ���� �ι� ���� �ʵ��� ��
            return;
        }

        calculateGravity();
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
        rb.linearVelocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, 100), velocity.z);
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


    private void CheckJumpButton()
    {
        // ���� ��ư �Է� Ȯ��
        if (Input.GetButtonDown("Jump"))
        {
            desiredJump = true;
            pressingJump = true;
        }

        if (Input.GetButtonUp("Jump"))
        {
            pressingJump = false;
        }
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
        onGround = ground.GetOnGround();

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
}
