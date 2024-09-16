using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float explosionTime = 3f; // Time before the bullet explodes if it doesn't collide
    public float explosionRadius = 5f; // Radius of the explosion
    public float explosionForce = 0f; // Force of the explosion
    public ParticleSystem explosion;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Add velocity to the bullet on spawn
        // Assuming you've already set the velocity somewhere before this point
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    /*private void OnTriggerEnter(Collider collision)
    {
        Explode();
    }*/

    void Update()
    {
        explosionTime -= Time.deltaTime;
        if (explosionTime <= 0f)
        {
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        /*
        // Create explosion at bullet's position
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
        */

        // Spawn explosion effect

        // Destroy the bullet immediately
        Destroy(gameObject);
    }
}