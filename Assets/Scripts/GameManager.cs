using UnityEngine;
using UnityEngine.SceneManagement;

public static class Constants
{
    public const float rad = Mathf.PI / 180f;
}

public class GameManager : MonoBehaviour
{
    // Temporary Manager (not singleton)

    private void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
