using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    RawImage fade;
    float _changeSpeed = 1f;
    [SerializeField] GameObject endingUI, playUI;
    void Start()
    {
        fade = GetComponentInChildren<RawImage>();
    }
    public void StartTransition()
    {
        StartCoroutine(Fadeout());
    }

    IEnumerator Fadeout()
    {
        playUI.SetActive(false);
        while (fade.color.a < 1f)
        {
            float currentAlpha = fade.color.a;
            float newAlpha = Mathf.MoveTowards(currentAlpha, 1f, _changeSpeed * Time.deltaTime);
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, newAlpha);
            yield return null;
        }
        endingUI.SetActive(true);
    }


    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Lobby");
    }

}
