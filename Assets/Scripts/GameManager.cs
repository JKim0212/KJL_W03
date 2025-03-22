using UnityEngine;
using UnityEngine.SceneManagement;

public static class Constants
{
    public const float rad = Mathf.PI / 180f;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // ╫л╠шео

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) RestartScene();
        else if (Input.GetKeyDown(KeyCode.Escape)) QuitGame();
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
