using UnityEngine;

public class UpwardForcePlatform : MonoBehaviour
{
    public float upwardForce;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.up * upwardForce + new Vector3 (rb.velocity.x, 0f, rb.velocity.z);
        }
    }
}