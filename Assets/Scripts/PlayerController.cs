using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField]
    bool _isGround = true;
    Vector3 _moveInput;
    float _speed = 7f;
    float _jumpForce = 8f;
    float _rotationSpeed;
    float _airSpeedMultiplier = 0.5f;
    Vector3 _initialJumpVelocity;

    [SerializeField]
    Transform _cameraTransform; // 카메라 Transform

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_cameraTransform == null)
        {
            _cameraTransform = Camera.main.transform; // 기본 메인 카메라 사용
        }
    }

    void FixedUpdate()
    {
        // 카메라 기준 이동 방향 계산
        Vector3 inputDirection = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;
        Vector3 moveDirection = _cameraTransform.TransformDirection(inputDirection);


        moveDirection.y = 0f; // Y축 무시
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
    }

    void OnCollisionEnter(Collision collision)
    {
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
}