using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class spawndinos : MonoBehaviour
{
    public GameObject[] dinosaures;
    public ParticleSystem spawnParticles;
    public GameObject childObject;
    

    // Update is called once per frame
    void Start()
    {
        childObject.SetActive(false);
        float randomTime = Random.Range(0,3);
        Invoke("spawn", randomTime);
    }

    private void spawn()
    {
        float randomTime = Random.Range(1,30);
        
        // Cast a ray from this object downwards
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            // Check if the ray hits the ground
            if (hit.collider.CompareTag("Ground"))
            {
                int randomIndex = Random.Range(0, dinosaures.Length);

                // Spawn particle effect
                Instantiate(spawnParticles, hit.point, Quaternion.identity);

                // Instantiate the random dinosaur at hit location
                Instantiate(dinosaures[randomIndex], hit.point, Quaternion.identity);
            }
        }
 
        Invoke("spawn", randomTime);
    }
}
