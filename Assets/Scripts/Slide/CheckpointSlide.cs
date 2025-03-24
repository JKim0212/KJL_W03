using System.Collections;
using UnityEngine;

public class CheckpointSlide : MonoBehaviour
{
    [SerializeField] int _checkpointNum;
    [SerializeField] ControlGameSlide controlGameSlide;
    [SerializeField] RectTransform saveAlarm;

    private bool isSavedFirst = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        controlGameSlide.ChangeCheckpoint(_checkpointNum);
        if(!isSavedFirst) StartCoroutine(DisplayAlarm());
    }

    private IEnumerator DisplayAlarm()
    {
        isSavedFirst = false;

        for (int i = 0; i < 40; i++)
        {
            saveAlarm.position += Vector3.down * 5f;

            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < 40; i++)
        {
            saveAlarm.position += Vector3.up * 5f;

            yield return new WaitForFixedUpdate();
        }

        yield break;
    }
}
