using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;
using static UnityEngine.Rendering.DebugUI;

public class YRplayercontrol : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]bool isGround = true;
    Vector2 input;
    float jump;

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        jump = value.Get<float>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = 10;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(0f, 0f, input.y);

        movement.Normalize();
        movement /= 3f;

        //rb.AddForce(movement, ForceMode.Impulse);

        //rb.position += movement/4f;
        //rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, movement.z);
        transform.Translate(movement);
        //transform.Rotate(Vector3.up * Time.deltaTime * input.x * 90f);
        if (isGround)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * input.x * 90f);
        }
        else if(input.y!=0)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * input.x * 60f);
        }
        else
        {
            transform.Rotate(Vector3.up * Time.deltaTime * input.x * 30f);
        }

            /*float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 movement = new Vector3(horizontal * 5f * Time.deltaTime, 0f, vertical * 5f * Time.deltaTime);

            //_rb.MovePosition(transform.position+movement);

            cc.Move(movement);*/
            Debug.Log(jump);
        if (jump>0.5f && isGround)
        {
            isGround = false;
            rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground") && !isGround){
            isGround = true;
        }
    }
}
