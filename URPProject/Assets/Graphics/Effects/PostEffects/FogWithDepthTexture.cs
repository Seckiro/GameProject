using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogWithDepthTexture : PostEffectsBase
{
    public Color fogColor = Color.white;
    public float fogDensity = 1.0f;
    public float fogStart = 0.0f;
    public float fogEnd = 2.0f;
    public float fogScale = 1.0f;
    public float fogAlpha = 1.0f;
    public float fogAlphaScale = 1.0f;

    public override void MaterialSetProperties()
    {
        Matrix4x4 frustumCorners = Matrix4x4.identity;
        float fov = Camera.fieldOfView;
        float near = Camera.nearClipPlane;
        float far = Camera.farClipPlane;
        float aspect = Camera.aspect;

        float halfHeight = near * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
        Vector3 toRight = CameraTrans.right * halfHeight * aspect;
        Vector3 toTop = CameraTrans.up * halfHeight;

        Vector3 topLeft = CameraTrans.forward * near + toTop - toRight;
        float scale = topLeft.magnitude / near;

        topLeft.Normalize();
        topLeft *= scale;

        Vector3 topRight = CameraTrans.forward * near + toRight + toTop;
        topRight.Normalize();
        topRight *= scale;

        Vector3 bottomRight = CameraTrans.forward * near + toRight - toTop;
        bottomRight.Normalize();
        bottomRight *= scale;

        Vector3 bottomLeft = CameraTrans.forward * near - toTop - toRight;
        bottomLeft.Normalize();
        bottomLeft *= scale;

        frustumCorners.SetRow(0, bottomLeft);
        frustumCorners.SetRow(1, bottomRight);
        frustumCorners.SetRow(2, topRight);
        frustumCorners.SetRow(3, topLeft);

        Material.SetMatrix("_FrustumCornersWS", frustumCorners);
        Material.SetMatrix("_ViewProjectionInverseMatrix", (Camera.projectionMatrix * Camera.worldToCameraMatrix).inverse);

        Material.SetColor("_FogColor", fogColor);

        Material.SetFloat("_FogDensity", fogDensity);
        Material.SetFloat("_FogStart", fogStart);
        Material.SetFloat("_FogEnd", fogEnd);
    }
}
