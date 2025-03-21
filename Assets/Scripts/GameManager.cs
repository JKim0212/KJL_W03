using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Temporary Manager (not singleton)

    private void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(2);
        }
    }
}
