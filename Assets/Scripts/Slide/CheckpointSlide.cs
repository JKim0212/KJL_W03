using UnityEngine;

public class CheckpointSlide : MonoBehaviour
{
    [SerializeField] int _checkpointNum;
    [SerializeField] ControlGameSlide controlGameSlide;

    void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        controlGameSlide.ChangeCheckpoint(_checkpointNum);
    }
}
