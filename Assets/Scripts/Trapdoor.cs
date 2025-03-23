using System.Collections;

using UnityEngine;

public class Trapdoor : MonoBehaviour
{
    enum Axis{
        xAxis, zAxis
    }
    [SerializeField] Axis currentAxis = Axis.xAxis; 
    bool _isOpening = false;
    [SerializeField] float _timeUntilOpen;
    [SerializeField] float _timeUntilClose;
    [SerializeField] float _openingDuration;
    [SerializeField] float _closingDuration;
    [SerializeField] int rotateDirection;
    Collider _col;

    private void Start()
    {
        _col = GetComponent<Collider>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_isOpening)
        {
            StartCoroutine(Open());
        }
    }

    IEnumerator Open()
    {
        _isOpening = true;

        // Wait before opening
        yield return new WaitForSeconds(_timeUntilOpen);

        // Opening animation
        float duration = _openingDuration; // Time in seconds for the door to open
        float elapsedTime = 0f;
        Quaternion startRotation = Quaternion.identity;
        Quaternion targetRotation = Quaternion.identity;
        if (currentAxis == Axis.zAxis)
        {
            targetRotation = Quaternion.Euler(0, 0, rotateDirection * 90f);
        }
        else if (currentAxis == Axis.xAxis)
        {
            targetRotation = Quaternion.Euler(rotateDirection * 90f, 0, 0);
        }


        while (elapsedTime < _openingDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _openingDuration;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null; // Wait for next frame
        }
        _col.enabled = false;
        transform.rotation = targetRotation; // Ensure we end exactly at target

        // Wait while open
        yield return new WaitForSeconds(_timeUntilClose);

        // Closing animation
        elapsedTime = 0f;
        startRotation = Quaternion.identity;
        if (currentAxis == Axis.zAxis)
        {
            startRotation = Quaternion.Euler(0, 0, rotateDirection * 90f);
        }
        else if (currentAxis == Axis.xAxis)
        {
            startRotation = Quaternion.Euler(rotateDirection * 90f,0, 0);
        }
        targetRotation = Quaternion.identity;

        while (elapsedTime < _closingDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _closingDuration;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null; // Wait for next frame
        }
        _col.enabled = true;
        transform.rotation = targetRotation; // Ensure we end exactly at target

        _isOpening = false;
    }
}
