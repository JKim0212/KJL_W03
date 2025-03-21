using System.Collections;
using UnityEngine;

public class DisappearingFloor : MonoBehaviour
{
    bool _isDisappearing;
    Collider _col;
    Renderer _rend;
    Color _default;
    [SerializeField] Color _target;
    [SerializeField] float _timeUntilDisappear, _timeUntilReappear;
    void Start()
    {
        _col = GetComponent<BoxCollider>();
        _rend = GetComponent<Renderer>();

    }

    IEnumerator Disappear()
    {
        for (int i = 0; i < _timeUntilDisappear; i++)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("alpha decreased by 1");
        }
        _col.enabled = false;
        _rend.enabled = false;
        yield return new WaitForSeconds(_timeUntilReappear);
        _col.enabled = true;
        _rend.enabled = true;
        _isDisappearing = false;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_isDisappearing)
        {
            StartCoroutine(Disappear());
            _isDisappearing = true;
        }
    }
}
