using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlGameSlide : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform[] _checkPoints;

    private int _currentCheckpoint = 0;

    public void Respawn()
    {
        Debug.Log("entered");
        //Rigidbody rb = player.GetComponent<Rigidbody>();
        player.transform.position = _checkPoints[_currentCheckpoint].position + Vector3.up * 3;

    }

    public void ChangeCheckpoint(int checkpointNum)
    {
        _currentCheckpoint = checkpointNum;
    }
}
