using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class dino : MonoBehaviour
{
    public int health;
    public GameObject enemy;
    private Vector3 newpos;
    private Quaternion newrot;
    public float speed;
    public List<AudioClip> audioFiles;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        newrot = Quaternion.Lerp(transform.rotation, enemy.transform.rotation, Time.deltaTime * speed);
        newpos = Vector3.Lerp(transform.position, enemy.transform.position, Time.deltaTime * speed);

        transform.position = newpos;
        transform.rotation = newrot;
        
        

        if (health <= 0)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(1);
            hitSound();

        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void hitSound()
    {
        int randomIndex = UnityEngine.Random.Range(0, audioFiles.Count);
        AudioClip randomClip = audioFiles[randomIndex];

        // Play the randomly selected audio clip
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = randomClip;
            audioSource.volume = 100; // Set the volume
            audioSource.Play();

            // Destroy the AudioSource component when the clip finishes playing
            Destroy(audioSource, randomClip.length);
    }
}
