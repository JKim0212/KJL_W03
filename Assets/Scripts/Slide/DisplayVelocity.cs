using TMPro;
using UnityEngine;
using System.Collections;

public class DisplayVelocity : MonoBehaviour
{
    [SerializeField] private ControlPlayer controlPlayer;
    private Transform niddle;
    private TextMeshProUGUI textMesh;

    private float speed;
    private float shake = 0f;

    private void Awake()
    {
        Debug.Log(Time.deltaTime);
        textMesh = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        niddle = transform.GetChild(1).GetComponent<RectTransform>();
    }

    private void Start()
    {
        StartCoroutine(ShakeNiddle());
    }

    private void Update()
    {
        SetNiddle();
    }

    private void SetNiddle()
    {
        // yello 135/180, red 165/180

        speed = controlPlayer.GetVelocity();

        textMesh.text = speed.ToString("0.00") + " km/h";

        speed += shake;

        if (speed < 0f) speed = 0f;
        else if (speed < 100f) speed = speed / 100f * 135f;
        else if (speed < 200f) speed = 105f + speed * 0.3f; // 135f + (speed - 100f) / 100f * 30f
        else if (speed < 500f) speed = 155f + speed * 0.05f; // 165f + (speed - 200f) / 300f * 15f
        else speed = 180f;

        niddle.rotation = Quaternion.Euler(0, 0, -speed);
    }

    private IEnumerator ShakeNiddle()
    {
        while (true)
        {
            shake = Random.Range(-1f, 1f);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
