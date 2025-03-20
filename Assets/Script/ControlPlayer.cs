using UnityEngine.InputSystem;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    private Vector2 input;
    private bool jump;

    private Rigidbody rb;
    private Transform mesh;

    [SerializeField] private float moveSpeed;
    private float accelation = 0;
    private bool isGround = false;

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
        mesh = transform.GetChild(0).GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        MoveForward();
        MoveSide();
        Jump();
    }

    private void MoveForward()
    {
        if (input.y > 0)
        {
            accelation = accelation < 20 ? accelation + 1 : 20;
        }
        else if (input.y < 0)
        {
            accelation = accelation > -20 ? accelation - 2 : -20;
        }
        else
        {
            //moveAccelation = moveAccelation > 10 ? moveAccelation - 1 : moveAccelation > 0 ? moveAccelation - 2 : 0;
            if (accelation > 0) accelation = accelation > 0 ? accelation - 1 : 0;
            else accelation = accelation < 0 ? accelation + 1 : 0;
        }

        mesh.Rotate((500f + 5f * accelation) * Time.deltaTime * Vector3.right);

        transform.Translate(new Vector3(0f, 0f, 1 + accelation * 0.02f) * moveSpeed);

        //rb.AddForce(movement, ForceMode.Impulse);

        //rb.position += movement/4f;
        //rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, movement.z);
    }

    private void MoveSide()
    {
        //rb.rotation = Quaternion.Euler(Vector3.zero);

        if (input.x != 0)
        {
            //rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.Euler(Vector3.up * input.x * 45f), Time.deltaTime * 5f);

            Vector3 rot = Quaternion.Lerp(rb.rotation, Quaternion.Euler(Vector3.up * input.x * 60f), Time.deltaTime * 5f).eulerAngles;

            if (rot.y < 180f && rot.y > 45f) rot.y = 45f;
            else if (rot.y > 180f && rot.y < 315f) rot.y = -45f;

            rb.rotation = Quaternion.Euler(rot);
        }
        else
        {
            rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.identity, Time.deltaTime * 10f);
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
    }

    private void Jump()
    {
        if (jump && isGround)
        {
            isGround = false;
            rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        }
        else if (!isGround)
        {
            Vector3 nowVelocity = rb.linearVelocity;

            if (nowVelocity.y < -12f)
            {
                nowVelocity.y = -12f;
                rb.linearVelocity = nowVelocity;
            }
            else if (nowVelocity.y < 0f)
            {
                rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
            }
            else if (nowVelocity.y < 7f)
            {
                nowVelocity.y = -0.2f;
                rb.linearVelocity = nowVelocity;
            }
        }
    }

    public float GetAccelation()
    {
        return accelation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground") && !isGround){
            isGround = true;
        }
    }
}
