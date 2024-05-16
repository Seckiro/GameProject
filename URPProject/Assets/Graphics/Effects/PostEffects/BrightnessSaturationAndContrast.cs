using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightnessSaturationAndContrast : PostEffectsBase
{
    public float _RollSpeed = 0.5f;

    [Range(0.0f, 3.0f)]
    public float brightness = 1.0f;

    [Range(0.0f, 3.0f)]
    public float saturation = 1.0f;

    [Range(0.0f, 3.0f)]
    public float contrast = 1.0f;

    public override void MaterialSetProperties()
    {
        Material.SetFloat("_Brightness", brightness);
        Material.SetFloat("_Saturation", saturation);
        Material.SetFloat("_Contrast", contrast);
    }
}
