using UnityEngine;

public class NextLevel : MonoBehaviour
{
    [SerializeField] SceneLoadEffect sle;
    [SerializeField] ControlGame controlGame;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance.playerData.isOneCleared = true;
        controlGame.ChangeCheckpoint(0);
        sle.StartTransition();
    }
}
