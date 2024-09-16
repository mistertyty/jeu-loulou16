using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class NewThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;

    public CinemachineFreeLook thirdPersonFreeLook;
    public CinemachineFreeLook combatFreeLook;
    public CinemachineFreeLook topDownFreeLook;

    public CameraStyle currentStyle;

    [Header("ChangeTimer")]
    public float switchModeDelay;
    public float switchModeTimer;
    public bool holdingGun;
    public enum CameraStyle
    {
        Basic,
        Combat,
        Topdown
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // switch styles
        conditionState();
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraStyle(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraStyle(CameraStyle.Combat);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCameraStyle(CameraStyle.Topdown);

        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // rotate player object
        if(currentStyle == CameraStyle.Basic || currentStyle == CameraStyle.Topdown)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        else if(currentStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        thirdPersonFreeLook.Priority = 10;
        combatFreeLook.Priority = 10;
        topDownFreeLook.Priority = 10;

        if (newStyle == CameraStyle.Basic) thirdPersonFreeLook.Priority = 11;
        if (newStyle == CameraStyle.Combat) combatFreeLook.Priority = 11;
        if (newStyle == CameraStyle.Topdown) topDownFreeLook.Priority = 11;

        currentStyle = newStyle;
    }

    private void conditionState()
    {
        if (holdingGun)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                //player.rotation = quaternion.Euler(player.rotation.x, transform.rotation.y, player.rotation.z);
                switchModeTimer = switchModeDelay;
                SwitchCameraStyle(CameraStyle.Combat);
            }

            if (switchModeTimer > 0)
            {
                switchModeTimer -= Time.deltaTime;
            }

            else if (currentStyle != CameraStyle.Basic)
                SwitchCameraStyle(CameraStyle.Basic);
        }
    }
}