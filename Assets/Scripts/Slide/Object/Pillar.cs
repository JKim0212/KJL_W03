using UnityEngine;

public class Pillar : MonoBehaviour
{
    [SerializeField] private float pillarMinusSpeed;
    [SerializeField] private float bumperCofficient;
    [SerializeField] private float tick;

    private float velocity=0;

    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Player")) return;

        velocity = collider.attachedRigidbody.linearVelocity.z;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Vector3 v = Vector3.Reflect(Vector3.forward, collision.contacts[0].normal);
        v = new(v.x, 0, v.z);

        collision.transform.rotation = Quaternion.LookRotation(v);

        float reflect = collision.transform.rotation.eulerAngles.y;

        if (reflect > 180f) reflect = 360f - reflect; // 전진벡터와의 각도 절대값
        if (reflect > 50f) reflect = 50f;

        reflect /= 50f;

        float speed = velocity > 0 ? pillarMinusSpeed + velocity * bumperCofficient : pillarMinusSpeed;

        collision.transform.GetComponent<ControlPlayer>().MoveForwardPillar_(speed * reflect, tick);
    }
}
