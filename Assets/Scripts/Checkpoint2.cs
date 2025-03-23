using UnityEngine;

public class Checkpoint2 : MonoBehaviour
{
    [SerializeField] int _checkpointNum;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Character")){
            GameManager.Instance.ChangeCheckpoint(_checkpointNum);
        }
    }
}
