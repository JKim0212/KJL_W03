using UnityEngine;

public class Web : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Character"))
        {
            collider.transform.GetComponent<ControlPlayer>().SetIsWeb(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Character"))
        {
            collider.transform.GetComponent<ControlPlayer>().SetIsWeb(false);
        }
    }
}
