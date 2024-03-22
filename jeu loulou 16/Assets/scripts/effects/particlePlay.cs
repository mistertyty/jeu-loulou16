using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particlePlay : MonoBehaviour
{
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ps.Play();
        Destroy(this.gameObject, 3f);
    }
}
