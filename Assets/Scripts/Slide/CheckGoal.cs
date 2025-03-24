using UnityEngine;

public class CheckGoal : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        StartCoroutine(collision.transform.GetComponent<ControlPlayer>().SetIsEndTrue());
    }
}