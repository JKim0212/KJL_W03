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

        //while (true)
        //{

        //    _rend.material.color = new Color(_rend.material.color.r, _rend.material.color.g, _rend.material.color.b, _rend.material.color.a - (1 / 900f));
        //    Debug.Log(_rend.material.color);

        //    if (_rend.material.color.a < 0.1f) break;

        //    yield return new WaitForSeconds(_timeUntilDisappear / 900f);
        //}

        float elapsedTime = 0f;
        float startAlpha = _rend.material.color.a;

        while (elapsedTime < _timeUntilDisappear)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / _timeUntilDisappear);
            _rend.material.color = new Color(_rend.material.color.r, _rend.material.color.g, _rend.material.color.b, newAlpha);
            Debug.Log(_rend.material.color);
            if (newAlpha < 0.1f) break;
            yield return null; // Wait for next frame
        }


        _col.enabled = false;
        _rend.enabled = false;

        yield return new WaitForSeconds(_timeUntilReappear);

        _rend.material.color = new Color(_rend.material.color.r, _rend.material.color.g, _rend.material.color.b, 1f);
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
