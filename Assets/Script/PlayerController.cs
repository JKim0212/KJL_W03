using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rb;
    bool _isGround = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGround)
        {
            float _horizontal = Input.GetAxisRaw("Horizontal");
            float _vertical = Input.GetAxisRaw("Vertical");
            Vector3 moveDir = new Vector3(_horizontal, 0, _vertical);
            _rb.linearVelocity = moveDir * 10;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isGround = false;
                _rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
            }
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground") && !_isGround){
            _isGround = true;
        }
    }
}
