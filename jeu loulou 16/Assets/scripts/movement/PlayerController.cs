using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using Unity.Mathematics;

public class PlayerController : MonoBehaviour
{
    
    [Header("Animations")]
    public ParticleSystem jumpSmoke;
    public ParticleSystem runSmoke;
    public ParticleSystem dash;
    public ParticleSystem coin;
    int i;
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float walkSpeed;
    public float sprintSpeed;
    private bool isSprinting;
    public Transform orientation;
    private Vector3 oldPos;
    public Vector3 flatVel;

    [Header("jump")]
    public float jumpForce;
    public float airMultiplier;
    public float coyoteTime;
    public float coyoteTimeCounter;
    [SerializeField] bool jumpEnable = true;
    public int maxJumps;
    public int numberOfBonusJumps;


    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;
    public float speedOnGround;
    public Vector3 groundSpeed;
    private Transform previousParent;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    public float angle;
    private RaycastHit slopeHit;
    public float oldDistanceToGround;
    private float newDistanceToGround;
    public bool worked;


    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    public float actualspeed;
    public float actualFlatSpeed;

    [Header("power ups")]
    [SerializeField] bool doubleJumpPower;
    public AudioClip coinSound;
    public Shoot shoot;



    private void Start()
    {
        previousParent = transform.parent;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        moveSpeed = walkSpeed;

        oldPos = transform.position;

        animator.SetBool("is_walking",false); //iddle at the start
        animator.SetBool("is_idle",true);
        animator.SetBool("is_sprinting",false);
    }

    private void Update()
    {
        //update should be used for inputs
        
        actualspeed = rb.velocity.magnitude;
        flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        actualFlatSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        // ground check
        if ((moveSpeed == sprintSpeed) && grounded && ((horizontalInput != 0) || (verticalInput != 0)))
        {
            animator.SetBool("is_walking",false); //sprinting
            animator.SetBool("is_idle",false);
            animator.SetBool("is_sprinting",true);

            if (i > 10)
            {
                Instantiate(runSmoke,new Vector3 (transform.position.x, transform.position.y - 1, transform.position.z),transform.rotation); // smoke particles
                i = 0;
            }
            i++;
        }
        else if (grounded && ((horizontalInput != 0) || (verticalInput != 0)))
        {
            animator.SetBool("is_walking",true); //walking
            animator.SetBool("is_idle",false);
            animator.SetBool("is_sprinting",false);

        }

        else
        {
            animator.SetBool("is_walking",false); //iddle if nothing
            animator.SetBool("is_idle",true);
            animator.SetBool("is_sprinting",false);
        }

        oldPos = transform.position;
        if (!pauseTrigger.isPaused)
        {
            MyInput();
            dragManagment();
        }
    }

    private void FixedUpdate()
    {
        //fixedupdate should be used for physics
        MovePlayer();
        GroundPhysics();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKeyDown(jumpKey) && (numberOfBonusJumps!=0 || coyoteTimeCounter >= 0f) && jumpEnable)
        {
            jumpEnable = false;
            Invoke(nameof(ResetJump),0.1f);

            if (coyoteTimeCounter < 0f)
                numberOfBonusJumps = 0;

            coyoteTimeCounter = 0f;

            Jump();
        }

        Sprint();
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if(grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f , ForceMode.Force);
        }
        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier , ForceMode.Force);

        // turn gravity off while on slope can also be turned of while dash but remains to be seen

        //if (!isDashing)
        rb.useGravity = !OnSlope();
        
        //else
    }

    private void Jump()
    {   
        
        Instantiate(jumpSmoke,transform.position,transform.rotation);
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z) + groundSpeed;

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void dragManagment()
    {
        if (grounded)
        {
            rb.drag = groundDrag;
            if (doubleJumpPower && jumpEnable)
                numberOfBonusJumps = maxJumps;
            if (jumpEnable)
                coyoteTimeCounter = coyoteTime;
            
            speedOnGround = flatVel.magnitude + groundSpeed.magnitude;
        }
        else
            rb.drag = 0;
        
        if (!grounded)
        {
            coyoteTimeCounter -= Time.deltaTime;

            // limit air speed to last speed on ground
            if (speedOnGround >= sprintSpeed)
            {
                if (flatVel.magnitude > speedOnGround)
                {
                        flatVel = flatVel.normalized * speedOnGround;
                        rb.velocity = new Vector3(flatVel.x, rb.velocity.y, flatVel.z);
                }
            }

            else
            {
                if (isSprinting && flatVel.magnitude > sprintSpeed)
                {
                    flatVel = flatVel.normalized * sprintSpeed;
                    rb.velocity = new Vector3(flatVel.x, rb.velocity.y, flatVel.z);
                }

                else if (!isSprinting && flatVel.magnitude > walkSpeed)
                {
                    flatVel = flatVel.normalized * walkSpeed;
                    rb.velocity = new Vector3(flatVel.x, rb.velocity.y, flatVel.z);
                }
            }

        }
    }

    private void ResetJump()
    {
        jumpEnable = true;
    }
    
    private void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isSprinting = !isSprinting;

        if (isSprinting)
            moveSpeed = sprintSpeed;

        else
            moveSpeed = walkSpeed;
    }
    

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void movingPlatform(RaycastHit hit)
    {
        if (hit.transform != null)
        {
            // Parent the player to the object it is hitting
            transform.parent = hit.transform;

            moving_platform movingPlatform = hit.transform.GetComponent<moving_platform>();
            if (movingPlatform != null)
            {
                // If it has the Moving_Platform script, get the Vectorspeed
                groundSpeed = movingPlatform.vectorspeed;
            }
            else
            {
                // If it doesn't have the Moving_Platform script, set groundSpeed to Vector3.zero
                groundSpeed = Vector3.zero;
            }

        }
        
        else
        {
            // If the hit is null, parent the player to nothing
            transform.parent = null;
            groundSpeed = Vector3.zero;
        }

        if (transform.parent != previousParent)
        {
            // If the parent has changed, add groundSpeed to the player's velocity
            // Update the previous parent
            previousParent = transform.parent;
            rb.velocity += groundSpeed;
        }
    }

    private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("powerup"))
            {
                Instantiate(coin,collider.transform.position,quaternion.identity);
                Destroy(collider.gameObject);
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = coinSound;
                audioSource.volume = 100; // Set the volume
                audioSource.Play();
                Destroy(audioSource, coinSound.length);
                int randomIndex = UnityEngine.Random.Range(0, 2);
                if (randomIndex == 0)
                    ShootFaster();
                else
                    RunFaster();
            }
        }

    private void ShootFaster()
    {
        Debug.Log("shooty wooty");
        shoot.shootInterval = 0.1f;
        Invoke("NotShootFaster", 10);
    }

    private void RunFaster()
    {
        Debug.Log("runny");
        sprintSpeed = sprintSpeed * 1.5f;
        Invoke("NotRunFaster", 10);
    }

    private void NotShootFaster()
    {
        shoot.shootInterval = 0.15f;
    }

    private void NotRunFaster()
    {
        sprintSpeed = sprintSpeed / 1.5f;
    }

    private void GroundPhysics()
    {
        grounded = Physics.SphereCast(transform.position, 0.5f, Vector3.down, out RaycastHit hit,playerHeight * 0.5f - 0.3f, whatIsGround);


        Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f);
        angle = Vector3.Angle(Vector3.up, slopeHit.normal);

        if (angle > maxSlopeAngle)
        {
            Debug.Log("no really");
            grounded = false;
        }


        movingPlatform(hit);
    }
}