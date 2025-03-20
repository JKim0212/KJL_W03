using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class JumpPad : MonoBehaviour
{
    struct JumpPadTarget
    {
        public float ContactTime;
        public Vector3 ContactVelocity;

    }

    [SerializeField] float launchDelay = 0.1f;
    [SerializeField] float launchForce = 100f;
    [SerializeField] ForceMode LaunchMode = ForceMode.Impulse;
    [SerializeField] float playerLaunchForceMultiplier;
    [SerializeField] float ImpactVelocityScale = 0.05f;
    [SerializeField] float MaxImpactVelocityScale = 2f;
    [SerializeField] float MaxDistortionWeight = 0.25f;
    Dictionary<Rigidbody, JumpPadTarget> Targets = new Dictionary<Rigidbody, JumpPadTarget>();
    List<Rigidbody> targetsToClear = new List<Rigidbody>();
    void FixedUpdate()
    {

        //점프시킬 타겟들 확인
        float thresholdTime = Time.timeSinceLevelLoad - launchDelay;
        foreach (var kvp in Targets)
        {
            if (kvp.Value.ContactTime >= thresholdTime)
            {
                Launch(kvp.Key, kvp.Value.ContactVelocity);
                targetsToClear.Add(kvp.Key);
            }
        }

        foreach (var target in targetsToClear)
        {
            Targets.Remove(target);
        }

        targetsToClear.Clear();


    }
    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb;
        if (collision.gameObject.TryGetComponent<Rigidbody>(out rb))
        {
            Debug.Log($"Rb velocity = {rb.linearVelocity} Impact = {collision.relativeVelocity}");
            Targets[rb] = new JumpPadTarget()
            {
                ContactTime = Time.timeSinceLevelLoad,
                ContactVelocity = collision.relativeVelocity
            };
        }
    }

    void OnCollisionExit(Collision collision)
    {

    }

    void Launch(Rigidbody targetRb, Vector3 contactVelocity)
    {
        Vector3 launchVector = transform.up;

        Vector3 distortionVector = transform.forward * Vector3.Dot(contactVelocity, transform.forward)
                                    + transform.right * Vector3.Dot(contactVelocity, transform.right);

launchVector = (launchVector + MaxDistortionWeight *distortionVector.normalized).normalized;
        float contactProjection = Vector3.Dot(contactVelocity, transform.up);
        if (contactProjection < 0)
        {
            //낙하속도에 따른 발사 속도 조절
            launchVector *= Mathf.Min(1f + Mathf.Abs(contactProjection * ImpactVelocityScale), MaxImpactVelocityScale);
        }

        if (targetRb.CompareTag("Player"))
        {
            launchVector *= playerLaunchForceMultiplier;
        }

        targetRb.AddForce(launchVector * launchForce, LaunchMode);
    }
}
