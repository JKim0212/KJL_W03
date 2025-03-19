using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class YR_PlayerController : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField]
    bool _isGround = true;
    Vector3 _moveInput;
    float _speed = 7;
    float jumpForce = 8;
    float _rotationSpeed;
    float _airSpeedMultiplier = 0.5f;
    Vector3 _initialJumpVelocity;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        Vector3 moveVelocity = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;
        if (_isGround)
        {
            _rb.linearVelocity = moveVelocity * _speed + Vector3.up * _rb.linearVelocity.y;


        }
        else
        {
            float airSpeed = _speed * _airSpeedMultiplier;
            Vector3 targetHorizontalVelocity = _initialJumpVelocity + moveVelocity * airSpeed;
            Vector3 currentHorizontalVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            Vector3 newHorizontalVelocity = Vector3.Lerp(currentHorizontalVelocity, targetHorizontalVelocity, Time.fixedDeltaTime * 5f);
            _rb.linearVelocity = newHorizontalVelocity + Vector3.up * _rb.linearVelocity.y;
        }
        LookForward();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground") && !_isGround){
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
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }

    void LookForward()
    {

        if (_moveInput.magnitude > 0.1f)
        {
            if (_isGround) _rotationSpeed = 8f;
            else _rotationSpeed = 3f;

            Vector3 moveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            _rb.rotation = Quaternion.Slerp(_rb.rotation, targetRotation, Time.fixedDeltaTime * _rotationSpeed);
        }
    }

}
