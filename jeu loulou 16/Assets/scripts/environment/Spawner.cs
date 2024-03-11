using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    public string targetTag = "Projectile"; // The tag of the objects we want to collide with
    public GameObject spawn;
    public Transform spawnPosition;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
            Instantiate(spawn,spawnPosition.position,spawnPosition.rotation);
        }
    }
}
