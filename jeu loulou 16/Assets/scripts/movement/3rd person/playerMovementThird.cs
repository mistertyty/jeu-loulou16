using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementTutorial : MonoBehaviour
{
    [Header("Animations")]
    public ParticleSystem jumpSmoke;

    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float walkSpeed;
    public float sprintSpeed;
    [SerializeField] bool isSprinting;
    public Transform orientation;

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
    bool grounded;


    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    [Header("power ups")]
    [SerializeField] bool doubleJumpPower;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        moveSpeed = walkSpeed;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.SphereCast(transform.position,0.5f,Vector3.down, out RaycastHit yes,playerHeight * 0.5f - 0.3f, whatIsGround);
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

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
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {   
        jumpSmoke.Play();
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
}