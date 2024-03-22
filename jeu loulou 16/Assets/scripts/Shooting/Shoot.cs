using System.Collections;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePosition;
    public int force;
    public float shootInterval = 0.5f; // Adjust this to change the interval between shots
    private bool isShooting = false;
    private Vector3 bulletDirection;

    void Update()
    {
        if (!pauseTrigger.isPaused)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isShooting = true;
                StartCoroutine(ShootRoutine());
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isShooting = false;
                StopAllCoroutines();
            }
        }
    }

    IEnumerator ShootRoutine()
    {
        while (isShooting)
        {
            ShootBullet();
            yield return new WaitForSeconds(shootInterval);
        }
    }

    void ShootBullet()
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
