using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField]
    bool _isGround = true;
    Vector3 _moveInput;
    float _speed = 10;
    float jumpForce = 5;
    float _rotationSpeed = 8f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = new Vector3(_moveInput.x,0f,_moveInput.y)* _speed + Vector3.up*_rb.linearVelocity.y;
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
        if (context.phase == InputActionPhase.Performed)
        {
            _isGround = false;
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }

    void LookForward()
    {
        if (_moveInput.magnitude > 0.1f) // 작은 입력은 무시
        {
            Vector3 moveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            _rb.rotation = Quaternion.Slerp(_rb.rotation, targetRotation, Time.fixedDeltaTime * _rotationSpeed);
        }
    }

}
