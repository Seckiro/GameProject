using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionBlurWithDepthTexture : PostEffectsBase
{
    [Range(0.0f, 1.0f)]
    public float blurSize = 0.0f;

    private Matrix4x4 previousViewProjectionMatrix;

    public override void MaterialSetProperties()
    {
        Material.SetFloat("_BlurSize", blurSize);

        Material.SetMatrix("_PreviousViewProjectionMatrix", previousViewProjectionMatrix);

        Matrix4x4 currentViewProjectionMatrix = Camera.projectionMatrix * Camera.worldToCameraMatrix;
        Matrix4x4 currentViewProjectionInverseMatrix = currentViewProjectionMatrix.inverse;

        Material.SetMatrix("_CurrentViewProjectionInverseMatrix", currentViewProjectionInverseMatrix);
        previousViewProjectionMatrix = currentViewProjectionMatrix;
    }
}
