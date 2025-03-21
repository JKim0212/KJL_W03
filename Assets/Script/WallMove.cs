using System.Collections;
using UnityEngine;

public class WallMove : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] Transform[] _positions;
    [SerializeField] float _moveSpeed, _waitTime;
    int movePos = 0;
    bool _canMove = true;
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
            _rb.MovePosition(_rb.position + (_positions[movePos].position-_rb.position).normalized * Time.deltaTime * _moveSpeed);
            if (Vector3.Distance(_rb.position, _positions[movePos].position) <= 0.1f)
            {
                StartCoroutine(Turn());
                _canMove = false;
                return;
            }
        }

    }

    IEnumerator Turn()
    {
        yield return new WaitForSeconds(_waitTime);
        movePos++;
        if (movePos >= _positions.Length)
        {
            movePos = 0;
        }
        _canMove = true;
    }
}
