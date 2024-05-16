using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDetection : PostEffectsBase
{
    [Range(0.0f, 1.0f)]
    public float edgesOnly = 1.0f;

    public Color edgesColor = Color.black;

    public Color backGroundColor = Color.white;

    public float sampleDistance = 1.0f;

    public float sensitivityDepth = 1.0f;

    public float sensitivityNormal = 1.0f;

    [ImageEffectOpaque]
    public override void MaterialSetProperties()
    {
        Material.SetFloat("_EdgesOnly", edgesOnly);
        Material.SetColor("_EdgesColor", edgesColor);
        Material.SetColor("_BackGroundColor", backGroundColor);
        Material.SetFloat("_SampleDistance", sampleDistance);
        Material.SetVector("_Sensitivity", new Vector4(sensitivityNormal, sensitivityDepth, 0, 0));
    }

}
