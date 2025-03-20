using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class KYR_PlayerController : MonoBehaviour
{
    [Header("Components")]
    Rigidbody _rb;
<<<<<<< HEAD
    [SerializeField]
    bool _isGround = true;
    Vector3 _moveInput;
    float _speed = 7f;
    float _jumpForce = 8f;
    float _rotationSpeed;
    float _airSpeedMultiplier = 0.5f;
    Vector3 _initialJumpVelocity;

    [SerializeField]
    Transform _cameraTransform; // 카메라 Transform (Inspector에서 할당)

    void Start()
=======
    [Header("Movement Stats")]
    [SerializeField] float maxSpeed;
    [SerializeField] float friction;
    [SerializeField] float maxAcceleration;
    [SerializeField] float maxAirAcceleration;
    [SerializeField] float maxDecceleration;
    [SerializeField] float maxAirDeceleration;
    [SerializeField] float maxTurnSpeed;
    [SerializeField] float maxAirTurnSpeed;

    [Header("Calculations")]
    Vector2 _dir;
    float _dirX;
    float _dirZ;
    Vector3 desiredVelocity;
    public Vector3 velocity;
    private float maxSpeedChange;
    private float acceleration;
    private float deceleration;
    private float turnSpeed;

    [Header("Current State")]
    [SerializeField] bool _pressingKey;
    [SerializeField] bool _isOnGround = true;

    public void OnMove(InputValue value)
>>>>>>> origin/Minkyum
    {
        _dir = value.Get<Vector2>();
        _dirX = _dir.x;
        _dirZ = _dir.y;
    }
    private void Awake()
    {
        //Find the character's Rigidbody and ground detection script
        _rb = GetComponent<Rigidbody>();
        if (_cameraTransform == null)
        {
            _cameraTransform = Camera.main.transform; // 기본 메인 카메라 사용
        }
    }

<<<<<<< HEAD
    void FixedUpdate()
    {
        // 카메라 기준 이동 방향 계산
        Vector3 inputDirection = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;
        Vector3 moveDirection = _cameraTransform.TransformDirection(inputDirection);
        moveDirection.y = 0f; // Y축 이동 제거
        moveDirection.Normalize();

        if (_isGround)
        {
            _rb.linearVelocity = moveDirection * _speed + Vector3.up * _rb.linearVelocity.y;
        }
        else
        {
            float airSpeed = _speed * _airSpeedMultiplier;
            Vector3 targetHorizontalVelocity = _initialJumpVelocity + moveDirection * airSpeed;
            Vector3 currentHorizontalVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            Vector3 newHorizontalVelocity = Vector3.Lerp(currentHorizontalVelocity, targetHorizontalVelocity, Time.fixedDeltaTime * 5f);
            _rb.linearVelocity = newHorizontalVelocity + Vector3.up * _rb.linearVelocity.y;
        }

        LookForward(moveDirection); // 이동 방향을 LookForward에 전달
=======
    void Update()
    {
        if (_dirX != 0 || _dirZ != 0)
        {
            _pressingKey = true;
        }
        else
        {
            _pressingKey = false;
        }
        desiredVelocity = new Vector3(_dirX, 0f, _dirZ) * maxSpeed;
>>>>>>> origin/Minkyum
    }

    void FixedUpdate()
    {
<<<<<<< HEAD
        if (collision.gameObject.CompareTag("Ground") && !_isGround)
        {
            _isGround = true;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && _isGround)
        {
            _isGround = false;
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _initialJumpVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z); // 점프 시 초기 속도 저장
        }
    }

    void LookForward(Vector3 moveDirection)
    {
        if (_moveInput.magnitude > 0.1f)
        {
            if (_isGround) _rotationSpeed = 8f;
            else _rotationSpeed = 3f;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            _rb.rotation = Quaternion.Slerp(_rb.rotation, targetRotation, Time.fixedDeltaTime * _rotationSpeed);
        }
    }
=======
        velocity = _rb.linearVelocity;
        RunWithAcceleration();
    }

    void RunWithAcceleration()
    {
        acceleration = _isOnGround ? maxAcceleration : maxAirAcceleration;
        deceleration = _isOnGround ? maxDecceleration : maxAirDeceleration;
        if (_pressingKey)
        {
            Debug.Log("Pressing");
            // if (Mathf.Sign(_dirX) != Mathf.Sign(velocity.x) || Mathf.Sign(_dirZ) != Mathf.Sign(velocity.z))
            // {
            //     maxSpeedChange = turnSpeed * Time.deltaTime;
            // }
            // else
            // {
            //     //If they match, it means we're simply running along and so should use the acceleration stat
            //     maxSpeedChange = acceleration * Time.deltaTime;
            // }
            maxSpeedChange = acceleration * Time.deltaTime;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

            _rb.linearVelocity = new Vector3(velocity.x, _rb.linearVelocity.y, velocity.z);
        }
    }
>>>>>>> origin/Minkyum
}