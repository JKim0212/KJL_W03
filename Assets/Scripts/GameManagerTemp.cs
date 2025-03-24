using UnityEngine;

public class GameManagerTemp : MonoBehaviour
{
    public static GameManagerTemp instance;
    [SerializeField] Transform[] _checkPoints;
    [SerializeField] GameObject player;
    int _currentCheckpoint = 0;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void Respawn()
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.position = _checkPoints[_currentCheckpoint].position + Vector3.up * 3f;
    }

    public void ChangeCheckpoint(int checkpointNum)
    {
        _currentCheckpoint = checkpointNum;
    }
}
