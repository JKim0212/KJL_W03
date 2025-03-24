using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] int _checkpointNum;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            GameManager.Instance.ChangeCheckpoint(_checkpointNum);
        }
    }
}
