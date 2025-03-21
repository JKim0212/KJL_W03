using UnityEngine;

public class Bottom : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
            GameManagerTemp.instance.Respawn();
        }
    }

}
