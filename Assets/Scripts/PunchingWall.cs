using System.Collections;
using UnityEngine;

public class PunchingWall : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] Transform[] _positions = new Transform[2];
    [SerializeField] float _punchSpeed, _moveSpeed, _punchWaitTime, _idleWaitTime;
    int movePos = 0;
    bool _canMove = true;
    float currentSpeed;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.position = _positions[movePos].position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_canMove)
        {
            if (movePos == 1)
            {
                _rb.position = Vector3.Lerp(_rb.position, _positions[movePos].position, currentSpeed * Time.deltaTime);
            }
            else
            {
                _rb.MovePosition(_rb.position + (_positions[movePos].position - _rb.position).normalized * Time.deltaTime * currentSpeed);
            }


            if (Vector3.Distance(_rb.position, _positions[movePos].position) <= 0.1f)
            {
                _rb.position = _positions[movePos].position;
                StartCoroutine(Turn(movePos));
                _canMove = false;
                return;
            }
        }

    }

    IEnumerator Turn(int currentmovePos)
    {
        if (currentmovePos == 1)
        {
            currentSpeed = _moveSpeed;
            yield return new WaitForSeconds(_punchWaitTime);
        }
        else
        {
            currentSpeed = _punchSpeed;
            yield return new WaitForSeconds(_idleWaitTime);
        }


        movePos++;
        if (movePos >= _positions.Length)
        {
            movePos = 0;
        }
        _canMove = true;
    }


}