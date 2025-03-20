using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class ControlPlayer : MonoBehaviour
{
    private Vector2 input;
    private bool jump;

    private Rigidbody rb;
    private Transform mesh;

    [SerializeField] private float moveSpeed;
    private float accelation = 0;
    private bool isGround = false;
    private bool isRail = false;

    private IEnumerator Booster;

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

        Booster = MoveForwardBooster();
    }

    private void FixedUpdate()
    {
        if (isRail) MoveRail();
        else Move();
        Jump();
    }

    private void Move()
    {
        if (input.y > 0)
        {
            accelation = accelation < 20 ? accelation + 1 : 20;
        }
        else if (input.y < 0)
        {
            accelation = accelation > -10 ? accelation - 2 : -10;
        }
        else
        {
            //moveAccelation = moveAccelation > 10 ? moveAccelation - 1 : moveAccelation > 0 ? moveAccelation - 2 : 0;
            if (accelation > 0) accelation = accelation > 0 ? accelation - 1 : 0;
            else accelation = accelation < 0 ? accelation + 1 : 0;
        }

        if (input.x != 0)
        {
            Vector3 rot = Quaternion.Lerp(rb.rotation, Quaternion.Euler(Vector3.up * input.x * 60f), Time.deltaTime * 5f).eulerAngles;

            if (rot.y < 180f && rot.y > 45f) rot.y = 45f;
            else if (rot.y > 180f && rot.y < 315f) rot.y = -45f;

            rb.rotation = Quaternion.Euler(rot);
        }
        else
        {
            rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.identity, Time.deltaTime * 10f);
        }

        mesh.Rotate((500f + 5f * accelation) * Time.deltaTime * Vector3.right);

        transform.Translate(new Vector3(0f, 0f, 1 + accelation * 0.02f) * moveSpeed);
    }

    public void MoveRail_()
    {
        isRail = true;

        rb.linearVelocity = new(0, 0, rb.linearVelocity.z);
    }

    private void MoveRail()
    {
        mesh.Rotate(500f * Time.deltaTime * Vector3.right);

        transform.Translate(new Vector3(0f, 0f, 1 + accelation * 0.02f) * moveSpeed);

        return;
    }

    public void MoveForwardBooster_()
    {
        StopCoroutine(Booster);
        Booster = MoveForwardBooster();
        StartCoroutine(Booster);
    }

    private IEnumerator MoveForwardBooster()
    {
        rb.linearVelocity = new(0, rb.linearVelocity.y, 100f);

        while (rb.linearVelocity.z > 0f)
        {
            rb.linearVelocity = new(0, rb.linearVelocity.y, rb.linearVelocity.z - 1f);
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = new(0, rb.linearVelocity.y, 0f);

        yield break;
    }

    private void Jump()
    {
        if (jump && isGround)
        {
            if (isRail)
            {
                isRail = false;

                rb.position += Vector3.up;
            }
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

    public bool GetIsRail()
    {
        return isRail;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground") && !isGround){
            isGround = true;
        }
    }
}
