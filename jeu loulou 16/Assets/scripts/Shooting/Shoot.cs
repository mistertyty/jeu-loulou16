using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Shoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePosition;
    public int force;
    private Vector3 bulletDirection;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            GameObject bullet = Instantiate(bulletPrefab, firePosition.position, Quaternion.identity); // Assuming bullet prefab already has correct rotation
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
                bulletDirection = hit.point - firePosition.position;

            else
                bulletDirection = (Camera.main.transform.position + Camera.main.transform.forward * 1000) - firePosition.position;

            bullet.GetComponent<Rigidbody>().AddForce(bulletDirection.normalized * force, ForceMode.Impulse);
        }
    }
}