Shader "Unlit/FragmentNormalTexture"
{
    Properties
    {
        [MainTexture]_MainTex ("MainTexture", 2D) = "white" {}
        [MainColor] _Color("Color",Color)=(1,1,1,1)
        _BumpMap("Narmal", 2D) = "white" {}
        _BumpScale("Bump Scale", float) = 1.0
        _Specular("Specular",Color)=(1,1,1,1)
        _Gloss("Gloss",Range(8.0,256))=20
    }
    SubShader
    {
        Tags 
        { 
            "LightMode"="UniversalForward"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            CBUFFER_START(UnityPerMaterial)

                Texture2D _MainTex;
                SamplerState sampler_MainTex;
                
                Texture2D _BumpMap;
                SamplerState sampler_BumpMap;

                float _Gloss; 
                float _BumpScale;
                float4 _Color;
                float4 _Specular;

                float4 _MainTex_ST;
                float4 _BumpMap_ST;

                struct a2v
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float4 uv : TEXCOORD0;
                    float3 lightDir:TEXCOORD1;
                    float3 viewDir : TEXCOORD2;
                };

            CBUFFER_END

            v2f vert (a2v v)
            {
                v2f o=(v2f)0;

                o.pos = mul(UNITY_MATRIX_MVP,v.vertex);

                o.uv.xy = v.texcoord * _MainTex_ST.xy + _MainTex_ST.zw;

                o.uv.zw = v.texcoord * _BumpMap_ST.xy + _BumpMap_ST.zw;

                float3 binormal = cross(normalize(v.normal),normalize(v.tangent.xyz)) * v.tangent.w;

                float3x3 rotation = float3x3(v.tangent.xyz,binormal,v.normal);
                
                //转换光线方向从ObjSpeace到切线坐标
                o.lightDir = mul(rotation,_MainLightPosition.xyz + v.vertex.xyz).xyz;

                o.viewDir = mul(rotation,TransformWorldToViewDir(v.vertex)).xyz;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float3 tangentLightDir = normalize(i.lightDir);

                float3 tangentViewDir = normalize(i.viewDir);

                float4 packedNormal = _BumpMap.Sample(sampler_BumpMap,i.uv.zw);
                
                //解压缩法线向量 Unpack Normal
                float3 tangentNormal = UnpackNormal(packedNormal);

                tangentNormal.xy *= _BumpScale;

                tangentNormal.z = sqrt(1.0 - saturate(dot(tangentNormal.xy,tangentNormal.xy)));

                float3 albedo = _MainTex.Sample(sampler_MainTex,i.uv).rgb * _Color.rgb;

                float3 anbient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

                float3 diffuse = GetMainLight().color.rgb * albedo * max(0,dot(tangentNormal,tangentLightDir));

                float3 halfDir = normalize(tangentLightDir + tangentViewDir);

                float3 specular = GetMainLight().color.rgb * _Specular.rgb * pow(max(0,dot(tangentNormal,halfDir)),_Gloss);

                return float4 (anbient+ diffuse + specular,1.0f);
            }
            ENDHLSL
        }
    }
    Fallback "Specular"
}
