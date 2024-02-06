using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class moving_platform_up : MonoBehaviour
{
    private bool going = true;
    public Transform startPoint;
    public Transform endPoint;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        if (going == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPoint.position, speed * Time.deltaTime);

            if (transform.position == endPoint.position)
            {
                going = !going;
            }
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPoint.position, speed * Time.deltaTime);

            if (transform.position == startPoint.position)
            {
                going = !going;
            }
        }
    }
}
