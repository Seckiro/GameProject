Shader "Debug/VertexShader"
{
    Properties
    {
        [MainTexture]_MainTex("MainTexture", 2D) = "white" {}
        _Color("Color",Color) = (1,1,1,1)
        [Toggle]_EnableNormal("Enable Normal",Int) = 0
        [Toggle]_EnableTangent("Enable Tangent",Int) = 0
        [Toggle]_EnableVertex("Enable Vertex",Int) = 0
        [Toggle]_EnableHClipVertex("Enable HClipVertex",Int) = 0
        [Toggle]_EnableWorldVertex("Enable WorldVertex",Int) = 0
        [Toggle]_EnableUV("Enable UV",Int) = 0
        [Toggle]_EnableTexcoord1("Enable Texcoord1",Int) = 0
    }
    SubShader
    {
        Pass
        {
            HLSLPROGRAM

            #pragma target 2.0

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                int _EnableNormal;
                int _EnableTangent;
                int _EnableVertex;
                int _EnableHClipVertex;
                int _EnableWorldVertex;
                int _EnableUV;
                int _EnableTexcoord1;
                float4 _Color;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct a2v
            {
                float4 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
            };

            struct v2f
            {
                float4 color : Color;
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD1;
            };

            v2f vert (a2v input)
            {
                v2f output = (v2f) 0;

                output.color = float4(1,1,1,1);

                output.vertex = TransformObjectToHClip(input.vertex.xyz);

                output.worldPos.xyz = TransformObjectToWorld(input.vertex.xyz);

                if(_EnableNormal > 0)
                output.color = input.normal * 0.5 + float4(0.5,0.5,0.5,0.5);
                
                if(_EnableTangent > 0)
                output.color = input.tangent * 0.5 + float4(0.5,0.5,0.5,0.5);
                
                if(_EnableVertex > 0)
                output.color = input.vertex *  0.5 + float4(0.5,0.5,0.5,0.5);
                
                if(_EnableHClipVertex > 0)
                output.color = output.vertex *  0.5 + float4(0.5,0.5,0.5,0.5);

                if(_EnableWorldVertex > 0)
                output.color = output.worldPos  * 0.5 + float4(0.5,0.5,0.5,0.5);

                if(_EnableUV > 0)
                output.color = input.uv * 0.5 + float4(0.5,0.5,0.5,0.5);

                if(_EnableTexcoord1 > 0)
                output.color = input.texcoord1 * 0.5 + float4(0.5,0.5,0.5,0.5);

                return output;
            }

            float4 frag (v2f input) : SV_Target
            {
                return input.color * _Color;
            }
            ENDHLSL
        }
    }
}
