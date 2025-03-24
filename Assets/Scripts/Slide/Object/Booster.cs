using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] private Color boosterColor;
    [SerializeField] private float boosterSpeed;
    [SerializeField] private float boosterTick;

    private void Awake()
    {
        transform.GetComponent<Renderer>().material.color = boosterColor;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Character"))
        {
            collider.transform.GetComponent<ControlPlayer>().MoveForwardBooster_(boosterSpeed, boosterTick);
        }
    }
}
