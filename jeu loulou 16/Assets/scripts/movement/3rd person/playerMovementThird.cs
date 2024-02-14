using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using Unity.Mathematics;

public class PlayerMovementTutorial : MonoBehaviour
{
    
    [Header("Animations")]
    public ParticleSystem jumpSmoke;
    public ParticleSystem runSmoke;
    int i;
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float walkSpeed;
    public float sprintSpeed;
    [SerializeField] bool isSprinting;
    public Transform orientation;
    private Vector3 oldPos;
    [SerializeField] Vector3 groundVelocity;
    public float dashForce;

    [Header("slow")]
    public float groundSlow;
    public float airSlow;

    [Header("jump")]
    public float jumpForce;
    public float airMultiplier;
    public float coyoteTime;
    public float coyoteTimeCounter;
    [SerializeField] bool jumpEnable = true;
    public int maxJumps;
    public int numberOfBonusJumps;
    public float maxAirSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    public float oldDistanceToGround;
    private float newDistanceToGround;
    public bool worked;


    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    [Header("power ups")]
    [SerializeField] bool doubleJumpPower;
    public float actualspeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        moveSpeed = walkSpeed;

        oldPos = transform.position;

        animator.SetBool("is_sprinting",false); //iddle at the start
        animator.SetBool("is_walking",false);
        animator.SetBool("is_idle",true);
    }

    private void Update()
    {
        
        actualspeed = rb.velocity.magnitude;
        // ground check
        grounded = Physics.SphereCast(transform.position, 0.5f, Vector3.down, out RaycastHit hit,playerHeight * 0.5f - 0.3f, whatIsGround);
        OnMoving(hit);
        if (((oldPos.x != transform.position.x) || (oldPos.z != transform.position.z)) & (moveSpeed == sprintSpeed) & (grounded) & ((horizontalInput != 0) || (verticalInput != 0)))
        {
            animator.SetBool("is_sprinting",true); //sprinting
            animator.SetBool("is_walking",false);
            animator.SetBool("is_idle",false);

            if (i > 10)
            {
                Instantiate(runSmoke,new Vector3 (transform.position.x, transform.position.y - 1, transform.position.z),transform.rotation); // smoke particles
                i = 0;
            }
            i++;
        }
        else if ((grounded) & ((horizontalInput != 0) || (verticalInput != 0)))
        {
            animator.SetBool("is_sprinting",false); //iddle animation
            animator.SetBool("is_walking",true);
            animator.SetBool("is_idle",false);

        }

        else
        {
            animator.SetBool("is_sprinting",false); //iddle animation
            animator.SetBool("is_walking",false);
            animator.SetBool("is_idle",true);
        }

        oldPos = transform.position;
        MyInput();
        Sprint();
        SpeedControl();

        // handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
            if (doubleJumpPower && jumpEnable)
                numberOfBonusJumps = maxJumps;
            if (jumpEnable)
                coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            rb.drag = 0;
        }
            AntiFlyOff();
        
    }

    private void FixedUpdate()
    {
        MovePlayer();
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
                numberOfBonusJumps-=1;
            coyoteTimeCounter = 0f;
            Jump();
        }
    }

    private void MovePlayer()
    {
        float maxAirSpeed = Mathf.Max(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude, sprintSpeed);
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if(grounded)
        {
            //rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            rb.AddForce( new Vector3 (((moveDirection.normalized.x * moveSpeed ) + groundVelocity.x) * 10 , 0f , ((moveDirection.normalized.z * moveSpeed) + groundVelocity.z) * 10 ), ForceMode.Force);
        }
        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();

        // used to not slide when you stop
        if (grounded && (moveDirection == new Vector3(0,0,0)) )
        {
            //rb.velocity = new Vector3(0,rb.velocity.y,0) + groundVelocity;
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3 (rb.velocity.x, 0, rb.velocity.z);
        Vector3 playerDirection = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized;

        // limiting speed on slope
        if (OnSlope())
        {
            if (rb.velocity.magnitude > moveSpeed + groundVelocity.magnitude)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground
        else if (grounded)
        {
            if (flatVel.magnitude > sprintSpeed)
            {
                if (grounded && flatVel.magnitude > sprintSpeed)
                {
                    float decayFactor = Mathf.Exp(-groundSlow * Time.deltaTime);
                    float decelerationForce = (1 - decayFactor) * rb.mass / Time.deltaTime;

                    Vector3 decelerationForceVector = -playerDirection * decelerationForce;

                    rb.AddForce(decelerationForceVector, ForceMode.Force);
                }
            }
            //set maxvelocity before jumping to current speed
        }

        //limit speed in the air
        if (!grounded)
        {
            if (flatVel.magnitude > maxAirSpeed)
            {
                Vector3 newVelocity = rb.velocity.normalized * maxAirSpeed;

                rb.velocity = new Vector3(newVelocity.x, rb.velocity.y, newVelocity.y);
            } 
        }
    }

    private void Jump()
    {   
        Instantiate(jumpSmoke,transform.position,transform.rotation);
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
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
    
    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.A))
            rb.AddForce(new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * dashForce, ForceMode.Impulse);
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void AntiFlyOff()
    {
        /*/ set y velocity to zero when grounded to not fly off
        //first case: going up
        if (grounded && !OnSlope() && jumpEnable)
        {
            Vector3 vel  = rb.velocity;
            vel.y = 0f;
            rb.velocity = vel;
        }
        */
    }

    private void OnMoving(RaycastHit ground)
    {

        if (ground.collider != null && ground.collider.tag == "moving")
        {
            moving_platform_up movingScript = ground.collider.GetComponent<moving_platform_up>();
            //groundVelocity = movingScript.vectorspeed;
            groundVelocity = Vector3.zero;
        }

        else if (ground.collider != null)
        {
            groundVelocity = Vector3.zero;
        }
        
        
    }

}