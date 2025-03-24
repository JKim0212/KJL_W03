using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] SceneLoadEffect sle;
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

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

