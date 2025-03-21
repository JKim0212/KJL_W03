using System.Collections;

using UnityEngine;

public class Trapdoor : MonoBehaviour
{
    bool _isOpening = false;
    [SerializeField] float _timeUntilOpen;
    [SerializeField] float _timeUntilClose;
    [SerializeField] float _openingDuration;
    [SerializeField] float _closingDuration;
    [SerializeField] int rotateDirection; 
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && !_isOpening){
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
        Quaternion targetRotation = Quaternion.Euler(0, 0, rotateDirection * 90f);

        while (elapsedTime < _openingDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _openingDuration;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null; // Wait for next frame
        }
        transform.rotation = targetRotation; // Ensure we end exactly at target

        // Wait while open
        yield return new WaitForSeconds(_timeUntilClose);
        
        // Closing animation
        elapsedTime = 0f;
        startRotation = Quaternion.Euler(0, 0, rotateDirection * 90f);
        targetRotation = Quaternion.identity;

        while (elapsedTime < _closingDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _closingDuration;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null; // Wait for next frame
        }
        transform.rotation = targetRotation; // Ensure we end exactly at target

        _isOpening = false;
    }
}
