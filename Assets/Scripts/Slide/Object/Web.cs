using UnityEngine;

public class Web : MonoBehaviour
{
    [SerializeField] private Color webColor;
    [SerializeField] private float webSpeedRate;
    [SerializeField] private float webTick;

    private void Awake()
    {
        transform.GetComponent<Renderer>().material.color = webColor;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Player")) return;

        collider.transform.GetComponent<ControlPlayer>().MoveForwardWeb_(webSpeedRate, webTick, true);
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Player")) return;
        
        collider.transform.GetComponent<ControlPlayer>().MoveForwardWeb_(webSpeedRate, webTick, false);
    }
}
