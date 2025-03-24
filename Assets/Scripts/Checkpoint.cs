using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] int _checkpointNum;
    [SerializeField] ControlGame controlGame;
    [SerializeField] GameObject checkEffect;

    void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.CompareTag("Player") || controlGame.currentCheckpoint == _checkpointNum) return;

        Instantiate(checkEffect, transform.position, Quaternion.Euler(-90f, 0,0));
        controlGame.ChangeCheckpoint(_checkpointNum);
    }
}
