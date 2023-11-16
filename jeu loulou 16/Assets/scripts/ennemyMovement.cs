using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ennemymovement : MonoBehaviour
{
    public GameObject playerPos;
    public float speed;
    private Vector3 waypoint;

    // Update is called once per frame
    void Update()
    {
        waypoint = new Vector3(playerPos.transform.position.x, transform.position.y, playerPos.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, waypoint, speed * Time.deltaTime);
    }
}
