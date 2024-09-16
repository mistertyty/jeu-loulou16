using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public bool isAlive;
    public ParticleSystem explosionParticle;
    public LevelLoader ll;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth < 0)
         currentHealth = 0;

        if (currentHealth == 0 && isAlive)
        {
            isAlive = false;
            Instantiate(explosionParticle, transform.position, quaternion.identity);
            ll.LoadSameLevel();
        }
        
    }

    public void SetHealthTo(int amountToSet)
    {
        currentHealth = amountToSet;
    }

    public void AddHealth(int amountToAdd)
    {
        currentHealth += amountToAdd;
    }
}
