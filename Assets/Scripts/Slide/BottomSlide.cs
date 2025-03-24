using UnityEngine;

public class BottomSlide : MonoBehaviour
{
    [SerializeField] ControlGameSlide controlGameSlide;

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        controlGameSlide.Respawn();
    }
}
