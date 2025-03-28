using UnityEngine;

public class Bottom : MonoBehaviour
{
    [SerializeField] ControlGame controlGame;

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        controlGame.Respawn();
    }
}
