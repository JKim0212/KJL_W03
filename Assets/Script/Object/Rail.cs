using UnityEngine;

public class Rail : MonoBehaviour
{
    [SerializeField] private bool prohibitJump;
    private Vector3 thisRotation;
    private readonly float rad = Mathf.PI / 180f;

    private void Awake()
    {
        thisRotation = transform.rotation.eulerAngles;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Character"))
        {
            collider.transform.position = GetPosition(collider.transform.position);
            collider.transform.rotation = transform.rotation;

            collider.transform.GetComponent<ControlPlayer>().nowRail = gameObject;
            collider.transform.GetComponent<ControlPlayer>().MoveRail_();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            collision.transform.position = GetPosition(collision.transform.position);
            collision.transform.rotation = transform.rotation;

            collision.transform.GetComponent<ControlPlayer>().MoveRail_();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.GetComponent<ControlPlayer>().nowRail == gameObject && collision.transform.GetComponent<ControlPlayer>().GetIsRail())
        {
            collision.transform.position = GetPosition(collision.transform.position);
            collision.transform.rotation = transform.rotation;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.transform.GetComponent<ControlPlayer>().nowRail == gameObject && collider.gameObject.CompareTag("Character"))
        {
            collider.transform.GetComponent<ControlPlayer>().SetIsRail(false);
            collider.transform.GetComponent<ControlPlayer>().nowRail = null;
        }
    }

    private Vector3 GetPosition(Vector3 targetPosition)
    {
        Vector3 newPosition = transform.position;
        float dist = targetPosition.z - transform.position.z;
        newPosition.x += dist * Mathf.Tan(thisRotation.y * rad); // 코드 개더럽네
        newPosition.y += -dist / Mathf.Cos(thisRotation.y * rad) * Mathf.Tan(thisRotation.x * rad) + 1; // 레일이랑 캐릭터 y스케일이 둘 다 1이니까 보정치 1
        newPosition.z = targetPosition.z;

        return newPosition;
    }
}
