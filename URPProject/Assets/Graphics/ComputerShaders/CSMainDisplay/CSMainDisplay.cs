using UnityEngine;

public class CSMainDisplay : MonoBehaviour
{
    public int depth = 16;
    public Vector2Int textureSize = new Vector2Int(256, 256);
    public ComputeShader displayComputeShader;

    private int _kernelIndex = -1;
    private string _kernelName = "CSMainDisplay";
    private string _shaderName = "Unlit/CSMainDisplay";

    void Start()
    {
        InitCSMainDisplay();
    }

    private void InitCSMainDisplay()
    {
        _kernelIndex = displayComputeShader.FindKernel(_kernelName);

        RenderTexture renderTexture = new RenderTexture(textureSize.x, textureSize.y, depth);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        Material material = new Material(Shader.Find(_shaderName));
        material.mainTexture = renderTexture;

        displayComputeShader.SetTexture(_kernelIndex, "_RenderTexture", renderTexture);
        displayComputeShader.Dispatch(_kernelIndex, textureSize.x / 8, textureSize.x / 8, 1);

        this.GetComponent<Renderer>().sharedMaterial = material;
    }

}
