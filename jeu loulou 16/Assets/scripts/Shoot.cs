using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Shoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePosition;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(bulletPrefab, firePosition.position, quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
        }
    }
}
