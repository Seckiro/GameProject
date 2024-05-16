Shader "Unlit/SceneViewDisplayShader"
{
    Properties
    {
        _transParentRang ("TransParentRang", Range(0,1)) = 1
    }

    SubShader
    {
        Tags 
        {
            "RenderType"="Transparent" 
            "RenderPipeline"="UniversalPipeline"
        }

        LOD 100
        Cull Off
        ZTest Always
        ZWrite On
        Blend SrcColor OneMinusSrcAlpha

        Pass
        {

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            float _transParentRang;

            Texture2D _SceneViewRenderTexture;

            SamplerState sampler_SceneViewRenderTexture;

            float4 _SceneViewRenderTexture_ST;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex =mul(UNITY_MATRIX_MVP, v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _SceneViewRenderTexture);

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 color = _SceneViewRenderTexture.Sample(sampler_SceneViewRenderTexture, i.uv);

                return float4(color.rgb,_transParentRang);
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
