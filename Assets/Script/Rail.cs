using System;
using Unity.VisualScripting;
using UnityEngine;

public class Rail : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        collider.transform.position = new(transform.position.x, transform.position.y + 1f, collider.transform.position.z);
        collider.transform.rotation = transform.rotation;

        if (collider.gameObject.CompareTag("Character"))
        {
            collider.transform.GetComponent<ControlPlayer>().MoveRail_();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.GetComponent<ControlPlayer>().GetIsRail())
        {
            collision.transform.position = new(transform.position.x, transform.position.y + 1f, collision.transform.position.z);
            collision.transform.rotation = transform.rotation;
        }
    }
}
