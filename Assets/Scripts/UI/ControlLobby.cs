using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlLobby : MonoBehaviour
{
    [SerializeField] GameObject warningWindow;
    [SerializeField] GameObject rushStartButton;
    [SerializeField] Button loadButton;

    private void Start()
    {
        if (GameManager.Instance.playerData.isOneCleared) rushStartButton.SetActive(true);
        if (GameManager.Instance.playerData.nowStage == 0) loadButton.interactable = false;
    }

    public void GameStart()
    {
        if (GameManager.Instance.playerData.nowStage != 0) GotoScene(2); // 2¹ø¾ÀÀº level1
        else warningWindow.SetActive(true);
    }

    public void NewStart()
    {
        GameManager.Instance.playerData.nowStage = 0;
        GotoScene(2);
    }

    public void GotoScene(int index)
    {
        GameManager.Instance.GotoScene(index);
    }

    public void QuitGame()
    {
        GameManager.Instance.Quit();
    }
}
