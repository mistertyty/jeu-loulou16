using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Shoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePosition;
    public Transform fireRotation;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(bulletPrefab, firePosition.position, fireRotation.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(Vector3.forward * 1000);
        }
    }
}
