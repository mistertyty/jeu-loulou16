using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class calculateVelocity : MonoBehaviour
{
    [SerializeField] float WaiTime;
    private Vector3 previous;
    public Vector3 speed;
    void Start()
    {
        previous = transform.position;
    }
    void Update()
    {
        speed = (transform.position - previous)/Time.deltaTime;
        previous = transform.position;
    }

}
