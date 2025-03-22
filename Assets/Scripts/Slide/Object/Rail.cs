using UnityEngine;

public class Rail : MonoBehaviour
{
    [SerializeField] private float railSpeedRatePercent;
    [SerializeField] private bool prohibitJump;

    private Vector3 thisRotation;
    private Vector3 enterVector;
    private float sinY, sinZ, cosY, cosZ, tanX, tanY;

    public void Init(float railSpeedRatePercent, bool prohibitJump)
    {
        this.railSpeedRatePercent = railSpeedRatePercent;
        this.prohibitJump = prohibitJump;
    }

    private void Start()
    {
        thisRotation = transform.rotation.eulerAngles;
        sinY = Mathf.Sin(thisRotation.y * Constants.rad);
        sinZ = Mathf.Sin(thisRotation.z * Constants.rad);
        cosY = Mathf.Cos(thisRotation.y * Constants.rad);
        cosZ = Mathf.Cos(thisRotation.z * Constants.rad);
        tanX = Mathf.Tan(thisRotation.x * Constants.rad);
        tanY = Mathf.Tan(thisRotation.y * Constants.rad);
        enterVector = new Vector3(1.5f * cosY, 0, 1.5f * sinY);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Character"))
        {
            collider.transform.position = GetPosition(collider.transform.position + enterVector);

            collider.transform.GetComponent<ControlPlayer>().SetNowRail(gameObject);
            collider.transform.GetComponent<ControlPlayer>().MoveRailEnter(railSpeedRatePercent);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            collision.transform.position = GetPosition(collision.transform.position);
            
            collision.transform.GetComponent<ControlPlayer>().SetNowRail(gameObject);
            collision.transform.GetComponent<ControlPlayer>().MoveRailEnter(railSpeedRatePercent);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.GetComponent<ControlPlayer>().GetNowRail() == gameObject && collision.transform.GetComponent<ControlPlayer>().GetIsRail())
        {
            collision.transform.position = GetPosition(collision.transform.position);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.transform.GetComponent<ControlPlayer>().GetNowRail() == gameObject && collider.gameObject.CompareTag("Character"))
        {
            collider.transform.GetComponent<ControlPlayer>().MoveRailExit();
            StartCoroutine(collider.transform.GetComponent<ControlPlayer>().SetIsRail(false));
            collider.transform.GetComponent<ControlPlayer>().SetNowRail(null);
        }
    }

    private Vector3 GetPosition(Vector3 targetPosition)
    {
        float dist = targetPosition.z - transform.position.z;
        Vector3 newPosition = transform.position;
        newPosition.x += dist * tanY - sinZ;
        newPosition.y += -dist / cosY * tanX + cosZ;
        newPosition.z = targetPosition.z;

        return newPosition;
    }

    public bool GetProhibitJump()
    {
        return prohibitJump;
    }
}
