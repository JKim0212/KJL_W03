using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlTower : MonoBehaviour
{
    public void GotoScene(int index)
    {
        GameManager.Instance.GotoScene(index);
    }

    public void RestartScene()
    {
        GameManager.Instance.GotoScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        GameManager.Instance.Quit();
    }
}
