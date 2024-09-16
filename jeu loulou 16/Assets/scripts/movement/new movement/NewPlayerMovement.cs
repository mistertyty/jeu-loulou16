using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class NewPlayerMovement : MonoBehaviour
{
    public float maxspeed;
    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public float groundDrag;
 
    [Header("Speed Control")]
    public float currentSpeed;
    public float desiredMoveSpeed;
    public float lastDesiredMoveSpeed;
    public float moveSpeed;


    public float lastGroundSpeed;

    [Header("Jumping")]
    public bool landing;
    public bool leaving;
    public int maxAmountOfJumps;
    private int amountOfJumps;
    public float coyoteTime;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;
    private bool previousGrounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    public float slopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Particles")]

    public ParticleSystem jumpParticle;
    public ParticleSystem sprintParticle;
    public ParticleSystem explosionParticle;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    [HideInInspector] public Vector3 moveDirection;

    [HideInInspector] public Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        stationary,
        walking,
        sprinting,
        crouching,
        sliding,
        air
    }
    private bool sprinting;
    public bool sliding;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        amountOfJumps = maxAmountOfJumps;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        if (currentSpeed > maxspeed)
            maxspeed = currentSpeed;
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround) && slopeAngle < maxSlopeAngle;

        MyInput();
        StateHandler();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        
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
        if (Input.GetKeyDown(jumpKey) && amountOfJumps != 0)
        {
            amountOfJumps -= 1;
            Jump();
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        if (Input.GetKeyDown(sprintKey))
        {
            sprinting = !sprinting;
        }
    }

    private void StateHandler()
    {
        if (grounded || currentSpeed < sprintSpeed)
        {
            // Mode - Sliding
            if (sliding)
            {
                state = MovementState.sliding;

                if (OnSlope() && rb.velocity.y < 0.1f)
                    desiredMoveSpeed = slideSpeed;

                else
                    desiredMoveSpeed = sprintSpeed;
            }

            // Mode - stationary
            if (horizontalInput == 0 && verticalInput == 0)
            {
                state = MovementState.stationary;
                desiredMoveSpeed = 0f;
            }

            // Mode - Crouching
            else if (Input.GetKey(crouchKey))
            {
                state = MovementState.crouching;
                desiredMoveSpeed = crouchSpeed;
            }

            // Mode - Sprinting
            else if (sprinting)
            {
                state = MovementState.sprinting;
                desiredMoveSpeed = sprintSpeed;
            }

            // Mode - Walking
            else
            {
                state = MovementState.walking;
                desiredMoveSpeed = walkSpeed;
            }
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }

        // check if desiredMoveSpeed has changed drastically

        if (currentSpeed < sprintSpeed)
        {
            moveSpeed = desiredMoveSpeed;

        }

        else
        {
            if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 5f && moveSpeed != 0)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }
        }


        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // calculate actual movement
        currentSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground, or underneath sprint speed to be able to accelerate even in air
        else if (grounded || currentSpeed < sprintSpeed)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else
        Debug.Log("in air");
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();

        //able to jump?

        if (leaving)
            amountOfJumps -= 1;

        if (grounded && amountOfJumps != maxAmountOfJumps)
        {
            ResetJump();
        }

        Landingleaving();
    }

    private void SpeedControl()
    {
        if(grounded)
        {
            /*
            // limiting speed on slope
            if (OnSlope() && !exitingSlope)
            {
                if (rb.velocity.magnitude > moveSpeed)
                    rb.velocity = rb.velocity.normalized * moveSpeed;
            }

            // limiting speed on ground or in air
            else
            {
                Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                // limit velocity if needed
                if (flatVel.magnitude > moveSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * moveSpeed;
                    rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
                }
            }
            */
            if (sprinting)
                lastGroundSpeed = math.max(math.abs(currentSpeed), sprintSpeed);
            
            else
                lastGroundSpeed = math.max(math.abs(currentSpeed), walkSpeed);
        }
        
        else
        {
            if (currentSpeed > lastGroundSpeed)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z).normalized * lastGroundSpeed + new Vector3(0f, rb.velocity.y, 0f);
            }

        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        //if (!grounded && rb.velocity.y < 0)
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void Landingleaving()
    {
        if (grounded && !previousGrounded)
            landing = true;

        else if (!grounded && previousGrounded)
            leaving = true;

        else
        {
            landing = false;
            leaving = false;
        }

        previousGrounded = grounded;
    }
    
    private void ResetJump()
    {
        amountOfJumps = maxAmountOfJumps;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return slopeAngle < maxSlopeAngle && slopeAngle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}
