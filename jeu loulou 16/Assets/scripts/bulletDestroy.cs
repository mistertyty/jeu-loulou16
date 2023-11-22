using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletDestroy : MonoBehaviour
{
    public ParticleSystem jumpSmoke;
        private void OnCollisionEnter()
        {
            Instantiate(jumpSmoke,transform.position,transform.rotation);
            Destroy(gameObject);
        }
}
