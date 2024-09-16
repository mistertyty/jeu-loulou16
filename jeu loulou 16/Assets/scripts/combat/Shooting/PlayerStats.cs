    using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public int speed;
    public ParticleSystem deathEffect;
    public bool ded;
    public GameObject player;

    [Header("Regeneration")]
    public float regenSpeed;
    private float regenSpeedTimer;
    public float regenRate;
    private float regenRateTimer;
    private bool regening;
    private bool fullHealth;
    public float invincibleDuration;
    public float invincibleTimer;
    public bool invincible;
    public GameObject deathscreen;
    public GameObject pauseMenu;
    public GameObject mainpause;
    public GameObject areyousure;
    public GameObject end;
    public GameObject upload;

    void Start()
    {
        ded = false;
        currentHealth = maxHealth;
        invincible = false;
    }

    void Update()
    {
        Regen();
        incincibility();
        if (transform.position.y <= -15)
        {
            currentHealth -= 1;
            Death();
        }
    }

    public void Death()
    {
        if (currentHealth <= 0)
            ded = true;

        if (ded)
        {
            deathscreen.SetActive(true);
            pauseMenu.SetActive(true);
            areyousure.SetActive(false);
            end.SetActive(false);
            upload.SetActive(false);
            mainpause.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            gameObject.SetActive(false);
            Instantiate(deathEffect, transform.position, transform.rotation);
            ded = true;
            player.SetActive(false);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("colission!!!" + collision.gameObject);
        if (collision.gameObject.tag == "Projectile" && !invincible)
        {
            invincibleTimer = invincibleDuration;
            currentHealth -= 1;
            Death();
            Debug.Log("HEALTH : " + currentHealth);
            regenSpeedTimer = regenSpeed;
        }
    }
    
    private void incincibility()
    {
        if (invincibleTimer >= 0)
        {
            invincible = true;
            invincibleTimer -= Time.deltaTime;
        }

        else
            invincible = false;
    }
    
    private void Regen()
    {
        //does he need healing
        if (currentHealth == maxHealth)
            fullHealth = true;

        else
            fullHealth = false;

        //heals if not full health
        if (regenSpeedTimer > 0 && !fullHealth)
        {    
            regenSpeedTimer -= Time.deltaTime;
            regening = false;
        }

        else if(!regening)
            regening = true;

        if (regening && !fullHealth)
        {    
            if (regenRateTimer > 0)
                regenRateTimer -= Time.deltaTime;
            else
            {
                currentHealth += 1;
                regenRateTimer = regenRate;
            }
        }

        

    }

    
}
