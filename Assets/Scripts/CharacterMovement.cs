using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [Header("Basic Components")]
    public Vector3 velocity;
    public bool onGround = true;
    private Rigidbody rb;
    CharacterGround ground;



    [Header("Running")]
    //public float directionX; // Input 값 확인(-1 ~ 1)
    //public float directionZ; // Input 값 확인(-1 ~ 1)



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
    //------------------------
    private Vector3 smoothedInput; // 부드러운 입력값
    private Vector3 inputVelocity; // 입력 보간 속도
    private float smoothTime = 0.05f; // 입력 부드러움 정도 (조정 가능)
    private Vector3 _moveInput;





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

        Move();
    }





    private void Move()
    {
        // 땅 위/공중에 따라 값 설정
        float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        float deceleration = onGround ? maxDecceleration : maxAirDeceleration;
        float turnSpeed = onGround ? maxTurnSpeed : maxAirTurnSpeed;

        // 카메라 기준 방향 계산
        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
        Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // 입력을 부드럽게 보간
        Vector3 targetInput = new Vector3(_moveInput.x, 0, _moveInput.y).normalized;
        smoothedInput = Vector3.SmoothDamp(smoothedInput, targetInput, ref inputVelocity, smoothTime);

        // 원하는 속도 계산
        desiredVelocity = (forward * smoothedInput.z + right * smoothedInput.x) * maxSpeed;
        desiredVelocity.y = rb.linearVelocity.y; // Y축 속도는 유지

        // 속도 변화 계산 (가속/감속 동적 처리)
        float speedChange = (smoothedInput.magnitude > 0.1f) ? acceleration : deceleration;
        float maxSpeedChange = speedChange * Time.deltaTime;

        // 부드러운 속도 적용
        velocity.x = Mathf.Lerp(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.Lerp(velocity.z, desiredVelocity.z, maxSpeedChange);
        velocity.y = rb.linearVelocity.y; // Y축 유지

        // Rigidbody에 속도 적용
        rb.linearVelocity = velocity;

        // 플레이어 회전 (부드럽게)
        if (velocity.x != 0 || velocity.z != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(velocity.x, 0, velocity.z));
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }




}
