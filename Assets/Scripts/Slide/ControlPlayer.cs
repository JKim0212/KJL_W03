using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class ControlPlayer : MonoBehaviour
{
    private Vector2 input;
    private bool jump;

    private Rigidbody _rb;
    private Transform _mesh;

    [SerializeField] private float moveSpeed;
    private float defaultMoveSpeed;
    private float webMoveSpeed;
    private float accelation = 0;
    private bool isGround = false;
    private bool isRail = false;

    private GameObject nowRail = null;
    private IEnumerator booster;
    private IEnumerator web;

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
        _rb = GetComponent<Rigidbody>();
        _mesh = transform.GetChild(0).GetComponent<Transform>();
    }

    private void Start()
    {
        booster = MoveForwardBooster(0f, 0f);
        web = MoveForwardWeb(0f, 0f, true);
        SetWebMoveSpeed(0.6f);
        defaultMoveSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        if (isRail) MoveRail();
        else Move();
        if (nowRail == null || (nowRail != null && !nowRail.GetComponent<Rail>().GetProhibitJump())) Jump();
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

        transform.Translate(new Vector3(0f, 0f, 1 + accelation * 0.02f) * Time.deltaTime * moveSpeed);

        _mesh.Rotate((500f + 10f * accelation) * Time.deltaTime * Vector3.right); // 가속 1단위는 기본 속도의 2%

        // 위는 전진, 아래는 좌우이동

        if (input.x != 0)
        {
            _rb.rotation = Quaternion.Lerp(_rb.rotation, Quaternion.Euler(Vector3.up * input.x * 60f), Time.deltaTime * 5f);
        }
        else
        {
            _rb.rotation = Quaternion.Lerp(_rb.rotation, Quaternion.identity, Time.deltaTime * 10f);
        }

        // 좌우 각도 보정
        Vector3 nowRotation = _rb.rotation.eulerAngles;
        if (nowRotation.y < 180f && nowRotation.y > 45f) nowRotation.y = 45f;
        else if (nowRotation.y > 180f && nowRotation.y < 315f) nowRotation.y = -45f;
        _rb.rotation = Quaternion.Euler(nowRotation);
    }
    public void MoveRail_()
    {
        isRail = true;

        _rb.rotation = Quaternion.identity;

        accelation = 10;

        _rb.linearVelocity = new(0, 0, _rb.linearVelocity.z);
    }

    private void MoveRail()
    {
        transform.Translate(new Vector3(0f, 0f, 1.2f) * Time.deltaTime * moveSpeed); // 1 + 0.02 * a(=10)

        _mesh.Rotate(600f * Time.deltaTime * Vector3.right); // 속도 1.2배니 회전도 1.2배

        return;
    }

    public void MoveForwardBooster_(float boosterSpeed, float tick)
    {
        StopCoroutine(booster);
        booster = MoveForwardBooster(boosterSpeed, boosterSpeed / tick);
        StartCoroutine(booster);
    }

    private IEnumerator MoveForwardBooster(float boosterSpeed, float rate)
    {
        _rb.linearVelocity = new(0, _rb.linearVelocity.y, boosterSpeed);

        while (_rb.linearVelocity.z > 0f)
        {
            _rb.linearVelocity = new(0, _rb.linearVelocity.y, _rb.linearVelocity.z - rate);
            _mesh.Rotate(20f*_rb.linearVelocity.z * Time.deltaTime * Vector3.right);
            yield return new WaitForFixedUpdate();
        }

        _rb.linearVelocity = new(0, _rb.linearVelocity.y, 0f);

        yield break;
    }

    public void MoveForwardWeb_(float webSpeedRate, float tick, bool enter)
    {
        StopCoroutine(web);
        web = MoveForwardWeb(webSpeedRate, defaultMoveSpeed * (1f - webSpeedRate) / tick, enter);
        StartCoroutine(web);
    }

    private IEnumerator MoveForwardWeb(float webSpeedRate, float rate, bool enter)
    {
        float finalWebSpeed = defaultMoveSpeed * webSpeedRate;

        if (enter)
        {
            while (moveSpeed > finalWebSpeed)
            {
                moveSpeed -= rate;
                yield return new WaitForFixedUpdate();
            }

            moveSpeed = finalWebSpeed;
        }
        else
        {
            while (moveSpeed < defaultMoveSpeed)
            {
                moveSpeed += rate;
                yield return new WaitForFixedUpdate();
            }

            moveSpeed = defaultMoveSpeed;
        }

        yield break;
    }

    private void Jump()
    {
        // 점프 직후 지상판정이 나지 않도록 보정
        if (_rb.linearVelocity.y < 9f)
        {
            isGround = Physics.Raycast(transform.position, Vector3.down, 0.51f);
        }

        if (jump && isGround)
        {
            isGround = false;
            isRail = false;
            _rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        }
        else if (!isGround)
        {
            Vector3 nowVelocity = _rb.linearVelocity;

            if (nowVelocity.y < -40f)
            {
                nowVelocity.y = -40f;
                _rb.linearVelocity = nowVelocity;
            }
            else if (nowVelocity.y < 0f)
            {
                _rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
            }
            else if (nowVelocity.y < 7f)
            {
                nowVelocity.y = -0.1f;
                _rb.linearVelocity = nowVelocity;
            }
        }
    }

    public bool GetIsRail()
    {
        return isRail;
    }

    public IEnumerator SetIsRail(bool newState)
    {
        yield return new WaitForSeconds(0.1f);

        isRail = newState;

        yield break;
    }

    public void SetWebMoveSpeed(float rate)
    {
        webMoveSpeed = moveSpeed * rate;
    }

    public void SetNowRail(GameObject newRail)
    {
        nowRail = newRail;
    }

    public GameObject GetNowRail()
    {
        return nowRail;
    }

    public float GetVelocity()
    {
        return _rb.linearVelocity.z + (1 + accelation * 0.02f) * moveSpeed;
    }
}
