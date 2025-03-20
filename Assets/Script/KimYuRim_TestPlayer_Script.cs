using UnityEngine;
using UnityEngine.InputSystem;

public class KimYuRim_TestPlayer_Script : MonoBehaviour
{
    [Header("Basic Components")]
    private Rigidbody rb;
    // 그라운드
    public bool isGrounded;


    [Header("Running")]
    public float directionX; // Input 값 확인(-1 ~ 1)
    public float directionZ; // Input 값 확인(-1 ~ 1)
    public float maxSpeed = 14f; // 최고 속도
    public float maxAcceleration = 85f; // 가속도(얼마나 빠르게 최고속도 도달)
    public float maxDecceleration = 85; // 감속도(얼마나 빠르게 정지 도달)
    public float maxTurnSpeed = 260f; // 방향 전환 속도
    public float maxAirAcceleration = 50; // 공중 가속도(공중에서 얼마나 빠르게 최고속도 도달)
    public float maxAirDeceleration = 50; // 공중 감속도(공중에서 얼마나 빠르게 정지 도달)
    public float maxAirTurnSpeed = 80f; // 공중 방향 전환 속도
    private float friction; // 마찰력(쓰이지 않음)
    //------------------------
    private Vector3 desiredVelocity;
    public Vector3 velocity;
    private float maxSpeedChange;
    private float acceleration;
    private float deceleration;
    private float turnSpeed;



    [Header("Jumping")]
    public float jumpHeight = 7.3f; // 최고 점프 높이
    public float timeToJumpApex; // 최고 높이까지 걸리는 시간
    public float upwardMovementMultiplier = 1f; // 상승 중력 곱하기
    public float downwardMovementMultiplier = 6.17f; // 하강 중력 곱하기
    public int maxAirJumps = 0; // 최대 점프 횟수
    public bool variablejumpHeight; // 점프키에 손 떼면 떨어지게 할 것인가?
    public float jumpCutOff; // 점프키 손 떼면 중력 곱하기
    public float speedLimit; // 최대 낙하 속도 제한
    public float coyoteTime = 0.15f; // 코요테 타임
    public float jumpBuffer = 0.15f; // 점프 버퍼
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

            // 이 프레임에서는 중력 계산 스킵. 코요테 타임이 두번 되지 않도록 함
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
        rb.linearVelocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, 100));
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

    private void Move()
    {
        // isGrounded 따라 적용되는 값 변경(acc, dec, turn)
        acceleration = isGrounded ? maxAcceleration : maxAirAcceleration;
        deceleration = isGrounded ? maxDecceleration : maxAirDeceleration;
        turnSpeed = isGrounded ? maxTurnSpeed : maxAirTurnSpeed;

        // 좌우, 전후 Input 값 받기(-1 ~ 1 사이)
        directionX = Input.GetAxis("Horizontal");
        directionZ = Input.GetAxis("Vertical");

        // 카메라의 방향을 기준으로 이동
        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
        Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);

        forward.y = 0; // Y축 방향은 무시
        right.y = 0; // Y축 방향은 무시

        forward.Normalize();
        right.Normalize();

        // 원하는 속도 계산
        desiredVelocity = (forward * directionZ + right * directionX) * maxSpeed;

        // Y축 속도 추가
        desiredVelocity.y = rb.linearVelocity.y; // 현재 Y축 속도를 유지

        // X축 움직임
        if (directionX != 0 || directionZ != 0)
        {
            // 현재 방향과 입력 방향이 다른 경우, 터닝으로 인식
            if (Mathf.Sign(directionX) != Mathf.Sign(velocity.x) || Mathf.Sign(directionZ) != Mathf.Sign(velocity.z))
            {
                maxSpeedChange = turnSpeed * Time.deltaTime;
            }
            else
            {
                // 현재 방향과 입력 방향이 같은 경우, 가속 시작
                maxSpeedChange = acceleration * Time.deltaTime;
            }
        }
        else
        {
            // 입력이 전혀 없으면, 감속
            maxSpeedChange = deceleration * Time.deltaTime;
        }

        // maxSpeedChange 만큼 X축, Z축 속도 계산
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        // 새로운 속도를 rigidbody에 업데이트
        velocity.y = rb.linearVelocity.y; // Y축 속도 유지
        rb.linearVelocity = velocity;

        // 플레이어 회전
        if (velocity.x != 0 || velocity.z != 0) // 이동하는 경우에만 회전
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(velocity.x, 0, velocity.z));
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        /*
        // 플레이어 회전
        if (velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        */


        Debug.Log("x축 속도: " + velocity.x + ", z축 속도: " + velocity.z);
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
        // onGround = ground.GetOnGround();

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



    /*

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jum = 0; // 바닥에 닿으면 점프 횟수 초기화
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