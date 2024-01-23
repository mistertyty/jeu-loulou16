using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletDestroy : MonoBehaviour
{
    public ParticleSystem Explosion;
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Projectile"))
            {
            Instantiate(Explosion,transform.position,transform.rotation);
            Destroy(gameObject);
            }
        }
}
