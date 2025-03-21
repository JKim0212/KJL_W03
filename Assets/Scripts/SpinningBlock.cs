using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class SpinningBlock : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float[] _degrees = { 0, 90, 180, 270 };
    int _degreePos = 0;
    bool _isRotating = false;
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, _degrees[_degreePos]);
    }
    void Update()
    {
        if (!_isRotating)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, _degrees[_degreePos]), 10 * Time.deltaTime);
            if(Quaternion.Angle(transform.rotation, Quaternion.Euler(0, 0, _degrees[_degreePos])) <= 0.01f){
                _isRotating = true;
                StartCoroutine(NextSpin());
            }
        }

    }

    IEnumerator NextSpin(){
        yield return new WaitForSeconds(1f);
        _degreePos++;
        if(_degreePos >= _degrees.Length){
            _degreePos = 0;
        }
        _isRotating = false;
    }

}
