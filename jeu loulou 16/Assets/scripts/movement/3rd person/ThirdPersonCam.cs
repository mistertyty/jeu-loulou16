using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    [SerializeField] private CinemachineVirtualCamera CombatFreelookCamera;

    public float rotationSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CombatFreelookCamera.gameObject.SetActive(false);
    }

    private void Update()
    {

    // rotate orientation
    Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
    orientation.forward = viewDir.normalized;

    // roate player object
    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");
    Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

    if (inputDir != Vector3.zero)
        playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);

    if (Input.GetMouseButton(1))
        CombatFreelookCamera.gameObject.SetActive(true);
    else
        CombatFreelookCamera.gameObject.SetActive(false);
    
    }
}
