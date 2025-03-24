using UnityEngine;
using UnityEngine.SceneManagement;

public static class Constants
{
    public const float rad = Mathf.PI / 180f;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform[] _checkPoints;
    
    int _currentCheckpoint = 0;
    public static GameManager Instance { get; private set; } // ½Ì±ÛÅÏ

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
        else if (Input.GetKeyDown(KeyCode.Escape)) GotoScene(0);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GotoScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Respawn()
    {
        Debug.Log("entered");
        //Rigidbody rb = player.GetComponent<Rigidbody>();
        player.transform.position = _checkPoints[_currentCheckpoint].position + Vector3.up * 3;

    }

    public void ChangeCheckpoint(int checkpointNum)
    {
        _currentCheckpoint = checkpointNum;
    }
}
