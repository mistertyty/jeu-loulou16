using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class discoBaby: MonoBehaviour
{
    private ColorAdjustments colorAdjustments;
    public VolumeProfile profile;
    public float colorSpeed;
    private bool yes;

    // Update is called once per frame
    void Update()
    {
        if (profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            yes = true;
        }

        if (yes)
        {
            colorAdjustments.hueShift.value += Time.deltaTime * colorSpeed;
            if (colorAdjustments.hueShift.value >= 180f)
            {
                colorAdjustments.hueShift.value = -180f;
            }
        }
    }
}
