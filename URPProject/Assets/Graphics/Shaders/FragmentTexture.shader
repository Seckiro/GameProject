Shader "Unlit/FragmentTexture"
{
    Properties
    {
        [MainTexture]_MainTex ("MainTexture", 2D) = "white" {}
        [MainColor] _Color("Color",Color)=(1,1,1,1)
        _Specular("Specular",Color)=(1,1,1,1)
        _Gloss("Gloss",Range(8.0,256))=20
    }
    SubShader
    {
        Tags 
        { 
            "RenderQueue"="Geometry"
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
                float4 _MainTex_ST;
                float _Gloss; 
                float4 _Color;
                float4 _Specular;

                struct a2v
                {
                    float4 vertex : POSITION;
                    float4 normal : NORMAL;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float3 worldNormal:TEXCOORD0;
                    float3 worldPos:TEXCOORD1;
                    float2 texcoord : TEXCOORD2;
                };

            CBUFFER_END

            v2f vert (a2v v)
            {
                v2f o=(v2f)0;

                o.pos=mul(UNITY_MATRIX_MVP,v.vertex);

                o.worldNormal=TransformObjectToWorldNormal(v.normal);

                o.worldPos=TransformObjectToWorld(v.vertex).xyz;

                o.texcoord=v.texcoord.xy*_MainTex_ST.xy+_MainTex_ST.zw;

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float3 worldNormal=normalize(i.worldNormal);

                float3 worldLightDir=normalize(GetMainLight().direction);

                float3 albedo= _MainTex.Sample(sampler_MainTex,i.texcoord).rgb * _Color.rgb;
                
                float3 anbient=UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

                float3 diffuse=GetMainLight().color.rgb*albedo * max(0,dot(worldNormal,worldLightDir));

                float3 viewDir=normalize(GetCameraPositionWS().xyz -i.worldPos);

                float3 halfDir=normalize(worldLightDir + viewDir);

                float3 specular=GetMainLight().color.rgb * _Specular.rgb * pow(max(0,dot(worldNormal,halfDir)),_Gloss);

                return float4 (anbient + diffuse + specular,1.0f);
            }
            ENDHLSL
        }
    }
    Fallback "Specular"
}
