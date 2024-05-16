using UnityEngine;

public class CSMainParticle : MonoBehaviour
{
    public struct ParticleBufferData
    {
        public Vector3 pos;//�ȼ���float3
        public Color color;//�ȼ���float4
    }

    public int _particleCount = 20;
    public int _particleNum = 1000;
    public MeshTopology meshTopology = MeshTopology.Points;
    public ComputeShader particleComputeShader;

    private int _kernelIndex = -1;
    private string _kernelName = "CSMainParticle";
    private string _shaderName = "Unlit/CSMainParticle";
    private string _particleBufferName = "_ParticleBuffer";

    private Material _material;

    //ComputeBuffer�е�stride��С�����RWStructuredBuffer��ÿ��Ԫ�صĴ�Сһ�¡�
    private ComputeBuffer _particleBuffer;

    public void Start()
    {
        InitUpdateParticle();
    }

    private void InitUpdateParticle()
    {
        _kernelIndex = particleComputeShader.FindKernel(_kernelName);
        _material = new Material(Shader.Find(_shaderName));
        _particleBuffer = new ComputeBuffer(_particleCount * _particleNum, 28);
        _particleBuffer.SetData(new ParticleBufferData[_particleCount * _particleNum]);
    }

    private void Update()
    {
        particleComputeShader.SetBuffer(_kernelIndex, _particleBufferName, _particleBuffer);
        particleComputeShader.SetFloat("_Time", Time.time);
        particleComputeShader.SetVector("_Pos", this.transform.position);
        particleComputeShader.Dispatch(_kernelIndex, _particleCount, 1, 1);
        _material.SetBuffer(_particleBufferName, _particleBuffer);
    }

    void OnRenderObject()
    {
        _material.SetPass(0);
        Graphics.DrawProceduralNow(meshTopology, _particleCount * _particleNum);
    }

    void OnDestroy()
    {
        _particleBuffer.Release();
        _particleBuffer = null;
    }
}
