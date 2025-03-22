using UnityEngine;

public class Bottom2 : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.CompareTag("Character")){
            
            GameManager.Instance.Respawn();
        }
    }

}
