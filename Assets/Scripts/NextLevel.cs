using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] SceneLoadEffect sle;
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        sle.StartTransition();
    }
}
