using UnityEngine;
using UnityEngine.UIElements;

public class moving_platform : MonoBehaviour
{
    private bool going = true;
    public Transform startPoint;
    public Transform endPoint;
    [SerializeField] float speed;
    public Vector3 vectorspeed;
    
    void Start()
    {
        transform.position = startPoint.position;
    }
    
    void FixedUpdate()
    {
        if (going == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPoint.position, speed * Time.deltaTime);
            vectorspeed = (endPoint.position - startPoint.position).normalized * speed;

            if (transform.position == endPoint.position)
            {
                going = !going;
            }
            
        }

        else
        {
            
            transform.position = Vector3.MoveTowards(transform.position, startPoint.position, speed * Time.deltaTime);
            vectorspeed = (startPoint.position - endPoint.position).normalized * speed;

            if (transform.position == startPoint.position)
            {
                going = !going;
            }
        }

    }
}