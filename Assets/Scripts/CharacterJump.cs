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
    public float jumpHeight = 7.3f; // 최고 점프 높이
    public float timeToJumpApex = 5f; // 최고 높이까지 걸리는 시간
    public float upwardMovementMultiplier = 1f; // 상승 중력 곱하기
    public float downwardMovementMultiplier = 1f; // 하강 중력 곱하기
    public int maxAirJumps = 1; // 최대 점프 횟수
    public bool variablejumpHeight = false; // 점프키에 손 떼면 떨어지게 할 것인가?
    public float jumpCutOff = 1; // 점프키 손 떼면 중력 곱하기
    public float speedLimit = 20; // 최대 낙하 속도 제한
    public float coyoteTime = 0.2f; // 코요테 타임
    public float jumpBuffer = 0.2f; // 점프 버퍼
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

        // desiredJump가 true인 동안 DoAJump 실행
        if (desiredJump)
        {
            DoAJump();
            rb.linearVelocity = velocity;

            // 이 프레임에서는 중력 계산 스킵. 코요테 타임이 두번 되지 않도록 함
            return;
        }

        calculateGravity();
    }


    private void calculateGravity()
    {
        // Y 방향에 따라 캐릭터의 중력 바꾸기

        // 캐릭터가 상승 중이면
        if (rb.linearVelocity.y > 0.01f)
        {
            if (onGround)
            {
                // 캐릭터가 움직이는 플랫폼 등 (땅에) 있으면 중력 안 바꾸기 
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                // variable jump height을 사용한다면
                if (variablejumpHeight)
                {
                    // 플레이어가 상승 중이고, 점프키 누르면 중력 곱하기
                    if (pressingJump && currentlyJumping)
                    {
                        gravMultiplier = upwardMovementMultiplier;
                    }
                    // 플레이어가 점프키 떼면 하강하기
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

        // 캐릭터가 하강 중이면
        else if (rb.linearVelocity.y < -0.01f)
        {

            if (onGround)
            // 캐릭터가 움직이는 플랫폼 등 (땅에) 있으면 중력 안 바꾸기 
            {
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                // 그렇지 않다면, 중력 곱하기
                gravMultiplier = downwardMovementMultiplier;
            }

        }
        // 전혀 움직이지 않는다면, currentlyJumping을 안하게
        else
        {
            if (onGround)
            {
                currentlyJumping = false;
            }

            gravMultiplier = defaultGravityScale;
        }

        // 캐릭터 속도 조절
        // speed limit이 있으면 y축 하강 속도 제한
        rb.linearVelocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, 100), velocity.z);
    }


    private void setPhysics()
    {
        // 새로운 중력 계산
        Vector2 newGravity = new Vector2(0, (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex));

        // 커스텀 중력 설정
        customGravity = new Vector3(0, newGravity.y, 0) * gravMultiplier;

        // 매 프레임마다 커스텀 중력 적용
        rb.AddForce(customGravity, ForceMode.Acceleration);
    }


    private void CheckJumpButton()
    {
        // 점프 버튼 입력 확인
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
        // 땅에 있거나, 코요테 타임 중이거나, 더블 점프가 가능하다면
        if (onGround || (coyoteTimeCounter > 0.03f && coyoteTimeCounter < coyoteTime) || canJumpAgain)
        {
            desiredJump = false;
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;

            // 더블 점프 허용 시 한번 더 점프하도록 함
            canJumpAgain = (maxAirJumps == 1 && canJumpAgain == false);

            // 값들을 바탕으로 점프 파워 정하기
            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * Vector3.Magnitude(customGravity) * jumpHeight);

            // 점프 중에 캐릭터가 위 혹은 아래로 이동한다면(더블 점프 등), 점프 스피드 변경
            // 현재 속도에 관계없이 점프 힘이 일정하도록 함
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(rb.linearVelocity.y);
            }

            // 새로운 점프 스피드를 더하기
            velocity.y += jumpSpeed;
            currentlyJumping = true;
        }

        if (jumpBuffer == 0)
        {
            // 점프 버퍼가 없으면, 점프키 누르자마자 desiredJump 끄기
            desiredJump = false;
        }
    }


    private void CheckJumpBufferCoyoteTime()
    {
        // Check if we're on ground, using Kit's Ground script
        onGround = ground.GetOnGround();

        // 점프 버퍼가 0보다 크면 예비 입력이 되도록
        if (jumpBuffer > 0)
        {
            // disireJump를 끄기 전 카운트, 카운트 끝나면 DoAJump 함수 실행
            if (desiredJump)
            {
                jumpBufferCounter += Time.deltaTime;

                if (jumpBufferCounter > jumpBuffer)
                {
                    // 카운트가 점프 버퍼보다 크면 disireJump를 끈다.
                    desiredJump = false;
                    jumpBufferCounter = 0;
                }
            }
        }

        // 점프 중이 아닌데, 땅에서 떨어진 경우, 코요테 타임 측정
        if (!currentlyJumping && !onGround)
        {
            coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            // 땅에 닿거나 점프하면 코요테 타임 초기화
            coyoteTimeCounter = 0;
        }
    }
}
