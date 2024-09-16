using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    private NewPlayerMovement np;
    private Rigidbody rb;
    private Camera mainCamera;

    [Header("values")]
    public float dashforce;
    public float dashTime;
    public float dashCooldown;
    public float dashPercentageKept;

    [Header("Conditions")]
    public bool ready;
    public bool dashed;

    [Header("Keybinds")]
    public KeyCode dashKey = KeyCode.LeftControl;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        np = GetComponent<NewPlayerMovement>();
        ready = true;
        dashed = false;
    }

    void Update()
    {
        Vector3 cameraForward = mainCamera.transform.forward;

        // Remove the Y component
        cameraForward.y = 0;

        if (Input.GetKeyDown(dashKey) && ready)
        {
            if (np.moveDirection.magnitude != 0)//if direction is pressed go there
            {
                rb.AddForce(np.moveDirection * dashforce, ForceMode.Impulse);
            } 

            else
            {
                rb.AddForce(cameraForward * dashforce, ForceMode.Impulse);
            }

            if(!np.grounded)
                np.lastGroundSpeed += dashforce;
            
            Invoke("EndDash", dashTime);
            Invoke("ReadyAgain", dashCooldown);
            ready = false;
        }

        if (dashed)
        {
            if (np.grounded)
            {
                ready = true;
                dashed = false;
            }
        }
        
    }

    void EndDash()
    {
        rb.AddForce(np.moveDirection * ((dashPercentageKept - 1) * dashforce), ForceMode.Impulse);

        if(!np.grounded)
                np.lastGroundSpeed += ((dashPercentageKept - 1) * dashforce);

    }

    void ReadyAgain()
    {
        dashed = true;
    }

}
