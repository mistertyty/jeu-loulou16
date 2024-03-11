using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class calculateVelocity : MonoBehaviour
{
    [SerializeField] float WaitTime;
    private Vector3 previous;
    public Vector3 speed;
    void Start()
    {
        previous = transform.position;
    }
    void Update()
    {
        speed = (transform.position - previous)/Time.deltaTime;
        //speed = new Vector3(40,0,0);
        previous = transform.position;
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {

            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            PlayerMovementTutorial playerMovementTutorialScript = collision.gameObject.GetComponent<PlayerMovementTutorial>();
            
            Vector3 newVel = new Vector3 (playerRb.velocity.x + speed.x ,playerRb.velocity.y, playerRb.velocity.z + speed.z );

            playerRb.velocity = newVel;
            playerMovementTutorialScript.overallMax = speed.magnitude + new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z).magnitude;
            Debug.Log(newVel.magnitude);
            playerRb.velocity = newVel;

            collision.transform.SetParent(null);
            
        }
    }*/

}
