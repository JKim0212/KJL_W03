using System.Collections;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    [SerializeField] SceneLoadEffect sle;
    [SerializeField] ControlGame controlGame;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance.playerData.isOneCleared = true;
        controlGame.ChangeCheckpoint(0);

        Rigidbody player = other.gameObject.GetComponent<Rigidbody>();
        player.AddForce(Vector3.up * 100, ForceMode.Impulse);

        StartCoroutine(Fadeout());
    }

    IEnumerator Fadeout()
    {
        yield return new WaitForSeconds(1f);

        sle.StartTransition();
    }

}

