Shader "Unlit/CropDissolve"
{
    Properties
    {
        _MainTexture ("Main Texture(RGB)", 2D) = "white" { }

        _DissolveTexture ("Dissolve Texture(R)", 2D) = "white" { }
        _DissolveAmount ("DissolveAmount", Range(0, 1)) = 0
        _DissolveEdge ("EdgeWidth", Range(0, 0.8)) = 0
        _DissolveEdgeColor ("DissolveEdgeColor", Color) = (1, 1, 1, 1)

        _CropAmount("CropAmount", float) = 0
        _CropEdge("CropEdge", Range(0,1)) = 0
        _CropEdgeColor("CropColor", Color) = (1,1, 1, 1)
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        Cull Back
        Blend One Zero
        ZTest LEqual
        ZWrite On
        AlphaToMask On

        Pass
        {


            HLSLPROGRAM

            #pragma target 4.5
            
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            Texture2D _MainTexture;
            SamplerState sampler_MainTexture;
            float4   _MainTexture_ST;

            Texture2D _DissolveTexture;
            SamplerState sampler_DissolveTexture;
            float4   _DissolveTexture_ST;
            
            float _DissolveAmount;
            float _DissolveEdge;
            float4 _DissolveEdgeColor;

            float _CropAmount;
            float _CropOffset;
            float _CropEdge;
            float4 _CropEdgeColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv: TEXCOORD; // 模型的纹理坐标
                float4 normal:NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv: TEXCOORD0; // 模型的纹理坐标
                float4 normal:TEXCOORD1;
                float3 positionWS:TEXCOORD2;
            };

            v2f vert (appdata v)
            {
                v2f o = (v2f)0 ;
                o.vertex = mul(UNITY_MATRIX_MVP,v.vertex);
                float3 pivot = float3(UNITY_MATRIX_M[0].w,UNITY_MATRIX_M[1].w,UNITY_MATRIX_M[2].w);//mul(UNITY_MATRIX_M,float4(0,0,0,1));
                o.normal = float4(pivot, 1);
                o.positionWS = mul(UNITY_MATRIX_M, v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {             
                float4 textureColor = _MainTexture.Sample(sampler_MainTexture,i.uv);
                float4 dissolveColor = _DissolveTexture.Sample(sampler_DissolveTexture,i.uv);

                float offsetValue = dissolveColor.r - _DissolveAmount;
                clip(offsetValue);
                
                offsetValue += (1 - sign(_DissolveAmount)) * _DissolveEdge;
                float edgeFactor = 1 - saturate(offsetValue / _DissolveEdge);

                float clipValue = - i.positionWS.y - _CropAmount + _CropOffset ;
                clip(clipValue);

                float4 dissolve = lerp(textureColor, _DissolveEdgeColor, edgeFactor);
                float4 Crop = lerp(_CropEdgeColor,textureColor , saturate (clipValue/_CropEdge));
                
                float4 color = dissolve + Crop;
                return color/2;
            }
            ENDHLSL
        }
    }
}
