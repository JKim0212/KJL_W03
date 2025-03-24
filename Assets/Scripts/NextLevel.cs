using UnityEngine;

public class NextLevel : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            GameManager.Instance.ToNextLevel();
        }
    }
}
