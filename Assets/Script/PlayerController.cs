using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float movespeed;
    [SerializeField] private bool isGround;
    private Vector2 input;
    private bool jump;

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        jump = value.Get<float>() != 0;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(0f, 0f, 1 + 0.5f * input.y);

        movement *= movespeed;

        //rb.AddForce(movement, ForceMode.Impulse);

        //rb.position += movement/4f;
        //rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, movement.z);
        transform.Translate(movement);

        rb.rotation = Quaternion.Euler(Vector3.zero);

        if (input.x != 0)
        {
            rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.Euler(Vector3.up * input.x * 45f), Time.deltaTime);
        }
        else
        {
            rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.identity, Time.deltaTime * 10f);
        }

            /*if (isGround)
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
            }*/

            /*float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 movement = new Vector3(horizontal * 5f * Time.deltaTime, 0f, vertical * 5f * Time.deltaTime);

            //_rb.MovePosition(transform.position+movement);

            cc.Move(movement);*/
            Debug.Log(jump);
        if (jump && isGround)
        {
            isGround = false;
            rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground") && !isGround){
            isGround = true;
        }
    }
}
