using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SceneViewDisplayFeature : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        private int _sceneViewRTID = Shader.PropertyToID("_SceneViewRenderTexture");
        private int _cameraOpaqueRTID = Shader.PropertyToID("_CameraOpaqueTexture");

        private string _cmdBufferName = "SceneViewDisplayCmdBuffer";

        //private string _sceneViewGraphName = "SceneViewDisplayGraph";
        //private string _sceneViewShaderName = "SceneViewDisplayShader";

        private Material _sceneViewDisplayBlitMaterial = new Material(Shader.Find("Unlit/SceneViewDisplayBlit"));

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor descriptor = new RenderTextureDescriptor(2560, 1440, RenderTextureFormat.Default, 0);

            cmd.GetTemporaryRT(_sceneViewRTID, descriptor);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(_cmdBufferName);

            cmd.Blit(_cameraOpaqueRTID, _sceneViewRTID, _sceneViewDisplayBlitMaterial);

            context.ExecuteCommandBuffer(cmd);

            cmd.Clear();

            cmd.Release();
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_sceneViewRTID);
        }
    }

    private CustomRenderPass _scriptablePass;

    public override void Create()
    {
        _scriptablePass = new CustomRenderPass();

        _scriptablePass.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_scriptablePass);
    }
}


