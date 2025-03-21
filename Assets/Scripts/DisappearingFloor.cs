using System.Collections;
using UnityEngine;

public class DisappearingFloor : MonoBehaviour
{
    bool _isDisappearing;
    Collider _col;
    Renderer _rend;
    [SerializeField] float _timeUntilDisappear, _timeUntilReappear;

    void Awake()
    {
        _col = GetComponent<BoxCollider>();
        _rend = GetComponent<Renderer>();

    }

    IEnumerator Disappear()
    {
        while (true)
        {
            _rend.material.color = new Color(_rend.material.color.r, _rend.material.color.g, _rend.material.color.b, _rend.material.color.a-0.01f);
            Debug.Log(_rend.material.color);
            if (_rend.material.color.a < 0.1f) break;

            yield return new WaitForSeconds(_timeUntilDisappear/90f);
        }

        _col.enabled = false;
        _rend.enabled = false;

        yield return new WaitForSeconds(_timeUntilReappear);

        _col.enabled = true;
        _rend.enabled = true;
        _isDisappearing = false;

        yield break;
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
