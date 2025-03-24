using UnityEngine;

public class ControlSetting : MonoBehaviour
{
    public void GotoScene(int index)
    {
        GameManager.Instance.GotoScene(index);
    }
}
