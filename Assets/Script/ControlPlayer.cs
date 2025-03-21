using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using static UnityEngine.Rendering.DebugUI.Table;

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
    private bool isWeb = false;

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
        if (isWeb)
        {
            accelation = -40;
        }
        else if (input.y > 0)
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

        transform.Translate(new Vector3(0f, 0f, 1 + accelation * 0.02f * moveSpeed));

        mesh.Rotate((500f + 10f * accelation) * Time.deltaTime * Vector3.right); // 가속 1단위는 기본 속도의 2%

        // 위는 전진, 아래는 좌우이동

        if (input.x != 0) rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.Euler(Vector3.up * input.x * 60f), Time.deltaTime * 5f);
        else rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.identity, Time.deltaTime * 10f);

        // 좌우 각도가 너무 틀어지지 않도록 보정
        Vector3 nowRotation = rb.rotation.eulerAngles;
        if (nowRotation.y < 180f && nowRotation.y > 45f) nowRotation.y = 45f;
        else if (nowRotation.y > 180f && nowRotation.y < 315f) nowRotation.y = -45f;
        rb.rotation = Quaternion.Euler(nowRotation);
    }
    public void MoveRail_()
    {
        isRail = true;

        rb.linearVelocity = new(0, 0, rb.linearVelocity.z);
    }

    private void MoveRail()
    {
        transform.Translate(Vector3.forward * moveSpeed * 1.2f);

        mesh.Rotate(600f * Time.deltaTime * Vector3.right); // 속도 1.2배니 회전도 1.2배

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
            mesh.Rotate(20f*rb.linearVelocity.z * Time.deltaTime * Vector3.right);
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = new(0, rb.linearVelocity.y, 0f);

        yield break;
    }

    private void Jump()
    {
        if (jump && isGround)
        {
            isGround = false;
            isRail = false;
            rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        }
        else if (!isGround)
        {
            Vector3 nowVelocity = rb.linearVelocity;

            if (nowVelocity.y < -40f)
            {
                nowVelocity.y = -40f;
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

            Debug.Log(rb.linearVelocity.y);
        }
    }

    public bool GetIsRail()
    {
        return isRail;
    }

    public void SetIsWeb(bool newState)
    {
        isWeb = newState;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground") && !isGround){
            isGround = true;
        }
    }
}
