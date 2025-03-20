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
        // isGrounded 따라 적용되는 값 변경(acc, dec, turn)
        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        deceleration = onGround ? maxDecceleration : maxAirDeceleration;
        turnSpeed = onGround ? maxTurnSpeed : maxAirTurnSpeed;

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

}
