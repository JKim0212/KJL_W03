using UnityEngine;

public class Booster : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Character"))
        {
            collider.transform.GetComponent<ControlPlayer>().MoveForwardBooster_();
        }
    }
}
