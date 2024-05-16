using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenOutLinesFeature : ScriptableRendererFeature
{
    class ViewSpaceNormalsTexturePass : ScriptableRenderPass
    {
        private int _msaaSamples;
        private LayerMask _layerMask;
        private FilteringSettings _filteringSettings;

        private readonly Material _normalMaterial;
        private readonly List<ShaderTagId> _listShaderTagID = new List<ShaderTagId>();
        private readonly RenderTargetHandle _renderTargetHandle;
        public RenderTargetIdentifier TargetIdentifier => _renderTargetHandle.Identifier();

        public ViewSpaceNormalsTexturePass(LayerMask layerMask, int msaaSamples)
        {
            this._layerMask = layerMask;
            this._msaaSamples = msaaSamples;

            renderPassEvent = RenderPassEvent.AfterRenderingSkybox;

            _normalMaterial = new Material(Shader.Find("Hidden/ViewSpaceNormals"));
            _listShaderTagID = new List<ShaderTagId>()
            {
                new ShaderTagId("UniversalForward"),
                new ShaderTagId("UniversalForwardOnly"),
                new ShaderTagId("LightweightForward"),
                new ShaderTagId("SRPDefaultUnlit"),
            };

            _renderTargetHandle.Init("_SceneViewSpaceNormals");
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            RenderTextureDescriptor renderTextureDescriptor = cameraTextureDescriptor;
            renderTextureDescriptor.msaaSamples = _msaaSamples;
            //renderTextureDescriptor.colorFormat = RenderTextureFormat.ARGB32;
            //renderTextureDescriptor.depthBufferBits = 0;
            //renderTextureDescriptor.width = cameraTextureDescriptor.width;
            //renderTextureDescriptor.height = cameraTextureDescriptor.height;

            cmd.GetTemporaryRT(_renderTargetHandle.id, renderTextureDescriptor, FilterMode.Point);
            ConfigureTarget(TargetIdentifier);
            ConfigureClear(ClearFlag.All, Color.clear);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_normalMaterial)
            {
                DrawingSettings drawingSettings = CreateDrawingSettings(_listShaderTagID, ref renderingData, SortingCriteria.CommonOpaque);
                drawingSettings.overrideMaterial = _normalMaterial;
                _filteringSettings = new FilteringSettings(RenderQueueRange.opaque, _layerMask);
                context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref _filteringSettings);
            }
        }
    }

    class ScreenSpaceOutlinePass : ScriptableRenderPass
    {
        private Material _outlineMaterial;
        private RenderTargetIdentifier cameraColorTarget;

        private readonly RenderTargetIdentifier _renderTargetIdentifier;

        private Material Creatmaterial()
        {
            return _outlineMaterial = new Material(Shader.Find("Hidden/ViewSpaceOutline"));
        }

        public void SetColor(Color color)
        {
            if (_outlineMaterial == null)
            {
                _outlineMaterial = Creatmaterial();
            }
            _outlineMaterial.SetColor("_Color", color);
            _outlineMaterial.SetFloat("_Intensity", color.a);
        }

        public ScreenSpaceOutlinePass(RenderTargetIdentifier normalsIdentifier, Color color)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingSkybox;
            this._renderTargetIdentifier = normalsIdentifier;
            _outlineMaterial = Creatmaterial();
            SetColor(color);
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            cameraColorTarget = renderingData.cameraData.renderer.cameraColorTarget;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_outlineMaterial)
            {
                CommandBuffer cmd = CommandBufferPool.Get();
                Blit(cmd, _renderTargetIdentifier, cameraColorTarget, _outlineMaterial, 0);
                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }
        }
    }

    [SerializeField]
    [Range(1, 16)]
    int msaaSamples = 2;

    [SerializeField]
    LayerMask outlineLayerMask;

    [SerializeField]
    Color color = Color.yellow;

    ViewSpaceNormalsTexturePass _viewSpaceNormalsTexturePass;

    ScreenSpaceOutlinePass _screenSpaceOutlinePass;

    public override void Create()
    {
        _viewSpaceNormalsTexturePass = new ViewSpaceNormalsTexturePass(outlineLayerMask, msaaSamples);
        _screenSpaceOutlinePass = new ScreenSpaceOutlinePass(_viewSpaceNormalsTexturePass.TargetIdentifier, color);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_viewSpaceNormalsTexturePass);
        renderer.EnqueuePass(_screenSpaceOutlinePass);
    }

    public void SetColor(Color color)
    {
        if (_screenSpaceOutlinePass != null) _screenSpaceOutlinePass.SetColor(color);
    }

    public void SetMsaaSamples(int msaaSamples)
    {
        this.msaaSamples = Mathf.Clamp(msaaSamples, 1, 16);
    }

    public void SetLayerMask(LayerMask outlineLayerMask)
    {
        this.outlineLayerMask = outlineLayerMask;
    }
}


