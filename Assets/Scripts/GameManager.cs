using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Constants
{
    public const float rad = Mathf.PI / 180f;
}

[System.Serializable]
public class PlayerData
{
    public bool[] stage = new bool[11];
    public int nowStage = 0; // 1스테이지 현재 저장 스테이지
    public bool isOneCleared; // 1스테이지 클리어 여부 (2스테이지 진입 가능 여부)
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // singleton

    private string filePath;
    [HideInInspector]
    public PlayerData playerData;

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

        playerData = LoadData();
        playerData ??= new PlayerData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) RestartScene();
        else if (Input.GetKeyDown(KeyCode.Escape)) GotoScene(0);
    }

    private void OnApplicationQuit()
    {
        SaveData(playerData);
    }

    private void SaveData(PlayerData data)
    {
        filePath = Path.Combine(Application.persistentDataPath, "playerData.bin");

        BinaryFormatter formatter = new();
        using FileStream stream = new(filePath, FileMode.Create);
        formatter.Serialize(stream, data);
    }

    private PlayerData LoadData()
    {
        filePath = Path.Combine(Application.persistentDataPath, "playerData.bin");

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new();
            using FileStream stream = new(filePath, FileMode.Open);
            return (PlayerData)formatter.Deserialize(stream);
        }

        return null;
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
}
