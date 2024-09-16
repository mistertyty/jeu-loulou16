using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Assuming the player has a PlayerHealth component that manages their health
            PlayerHealth ph = other.GetComponent<PlayerHealth>();

            // If a PlayerHealth component is found, remove all health
            if (ph != null)
            {
                ph.AddHealth(-ph.currentHealth);
            }
        }
    }
    
}
