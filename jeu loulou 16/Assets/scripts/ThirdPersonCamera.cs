using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform orientation;
    public Transform player;
    public Transform playerObject;
    public Rigidbody rb;
    public float rotationspeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        //rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        //rotate player object
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticaltalInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = orientation.forward * verticaltalInput + orientation.right * horizontalInput;
        if (inputDir != Vector3.zero)
            playerObject.forward = Vector3.Slerp(playerObject.forward, inputDir.normalized, Time.deltaTime * rotationspeed);

    }

}
