using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class canShoot : MonoBehaviour
{
    public float maxRange = 50f; // Maximum range of the aim
    public float coneAngle; // Angle of the cone in degrees
    public Camera mainCamera;
    public LayerMask enemyLayer; // Layer mask to filter enemies

    void Start()
    {
        coneAngle = mainCamera.fieldOfView;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the center of the screen
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            RaycastHit hit;

            // Perform a raycast to find the aim direction
            if (Physics.Raycast(ray, out hit, maxRange))
            {
                // Aim direction is the direction towards the hit point
                Vector3 aimDirection = (hit.point - transform.position).normalized;

                // Iterate over all potential enemies
                foreach (Collider enemyCollider in Physics.OverlapSphere(transform.position, maxRange, enemyLayer))
                {
                    // Calculate direction to the enemy
                    Vector3 directionToEnemy = (enemyCollider.transform.position - transform.position).normalized;

                    // Calculate angle between aim direction and direction to enemy
                    float angleToEnemy = Vector3.Angle(aimDirection, directionToEnemy);

                    // Check if the enemy is within the cone angle
                    if (angleToEnemy <= coneAngle / 2f)
                    {
                        // Aim at the enemy
                        AimAtEnemy(enemyCollider.transform);
                        return; // Aim at the closest enemy
                    }
                }
            }
        }
    }

    void AimAtEnemy(Transform enemyTransform)
    {
        // Your aiming logic here, e.g., rotate towards the enemy
        transform.LookAt(enemyTransform);
    }
}