using UnityEngine;

public class Booster : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Character"))
        {
            StartCoroutine(collider.transform.GetComponent<ControlPlayer>().MoveForwardBooster());
        }
    }
}
