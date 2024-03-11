using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particlePlay : MonoBehaviour
{
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ps.Play();
        Debug.Log(gameObject);
        Destroy(this.gameObject, 3f);
    }
}
