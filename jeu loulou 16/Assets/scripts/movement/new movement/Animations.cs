using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Animations : MonoBehaviour
{
    private Animator animator;
    private NewPlayerMovement np;
    NewPlayerMovement.MovementState state;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        np = GetComponent<NewPlayerMovement>();  
        rb = np.rb;
    }

    // Update is called once per frame
    void Update()
    {
        float flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;

        if (np.grounded && flatVel >= 0.1f && np.moveDirection != Vector3.zero)
        {
            if (np.state == NewPlayerMovement.MovementState.sprinting)
            {
                animator.SetBool("is_walking",false); //sprinting
                animator.SetBool("is_idle",false);
                animator.SetBool("is_sprinting",true);

            }

            if (np.state == NewPlayerMovement.MovementState.walking)
            {
                animator.SetBool("is_walking",true); //walking
                animator.SetBool("is_idle",false);
                animator.SetBool("is_sprinting",false);

            }
        }

        else
        {
            animator.SetBool("is_walking",false); //iddle if nothing
            animator.SetBool("is_idle",true);
            animator.SetBool("is_sprinting",false);

        }
        
    }
}
