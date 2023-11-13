using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class topdownmove : MonoBehaviour
{
    public float speed;
    public Vector2 move;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    public void MovePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);
        if(movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement),0.15f);
        }
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
        
    }

    void Update()
    {
        MovePlayer();
    }
}
