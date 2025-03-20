using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    Rigidbody _rb;
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
    {
        _dir = value.Get<Vector2>();
        _dirX = _dir.x;
        _dirZ = _dir.y;
    }
    private void Awake()
    {
        //Find the character's Rigidbody and ground detection script
        _rb = GetComponent<Rigidbody>();
    }

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
    }

    void FixedUpdate()
    {
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
}