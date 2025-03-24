using UnityEngine;

public class ControlGame : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform[] _checkPoints;

    private int _currentCheckpoint = 0;
    public int currentCheckpoint => _currentCheckpoint;

    private void Awake()
    {
        _currentCheckpoint = GameManager.Instance.playerData.nowStage;
        Respawn();
    }

    public void Respawn()
    {
        Debug.Log("entered");
        //Rigidbody rb = player.GetComponent<Rigidbody>();
        player.transform.position = _checkPoints[_currentCheckpoint].position + Vector3.up * 3;

    }

    public void ChangeCheckpoint(int checkpointNum)
    {
        _currentCheckpoint = checkpointNum;
        GameManager.Instance.playerData.nowStage = checkpointNum;
    }
}
