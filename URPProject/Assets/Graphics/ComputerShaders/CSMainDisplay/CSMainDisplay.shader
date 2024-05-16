Shader "Unlit/CSMainDisplay"
{
    Properties
    {
        [MainTex] 
        _MainTex ("CSMainDisplayTexture", 2D) =""{}
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

            Texture2D _MainTex;

            SamplerState sampler_MainTex;

            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex =mul(UNITY_MATRIX_MVP, v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }



            float4 frag (v2f i) : SV_Target
            {
                float4 color = _MainTex.Sample(sampler_MainTex, i.uv);

                return float4(color.rgb,_transParentRang);
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
