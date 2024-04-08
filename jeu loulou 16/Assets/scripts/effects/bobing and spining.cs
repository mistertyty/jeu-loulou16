using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class bobingandspining : MonoBehaviour
{
    private float startingBob;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float bobSpeed;
    [SerializeField] private float bobHeight;
    // Start is called before the first frame update
    void Start()
    {
        startingBob = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, startingBob + math.cos(Time.time * bobSpeed) * bobHeight, transform.position.z);
    }
}
