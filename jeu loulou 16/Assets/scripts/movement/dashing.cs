using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class dashing : MonoBehaviour
{
    [Header("Refenrences")]
    public Transform orientation;
    private Rigidbody rb;
    private PlayerController pc;

    [Header("Dashing")]
    public float dashForce;
    public float dashDuration;


    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("Input")]
    public KeyCode dashKey= KeyCode.LeftControl;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pc = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(dashKey))
        {
            Dash();
        }
    }

    private void Dash()
    {
        Vector3 forceToApply= orientation.forward * dashForce;
        rb.AddForce(forceToApply, ForceMode.Impulse);
        Invoke(nameof(ResetDash), dashDuration);
    }

    private void ResetDash()
    {
        
    }

}
