using TMPro;
using UnityEngine;

public class DisplayVelocity : MonoBehaviour
{
    [SerializeField] private ControlPlayer controlPlayer;
    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        textMesh.text = controlPlayer.GetVelocity().ToString("0.00") + " km/h";
    }
}
