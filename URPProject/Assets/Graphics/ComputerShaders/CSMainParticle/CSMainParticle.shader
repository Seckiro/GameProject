Shader "Unlit/CSMainParticle"
{
    SubShader
    {
        Tags { "RenderType"="Opaque"  "Queue"="Geometry"}
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct v2f
            {
                float4 col : COLOR0;
                float4 vertex : SV_POSITION;
            };
            
            struct particleData
            {
                float3 pos;
                float4 color;
            };

            StructuredBuffer<particleData> _ParticleBuffer;
            
            v2f vert (uint id : SV_VertexID)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(float4(_ParticleBuffer[id].pos, 0));
                o.col = _ParticleBuffer[id].color;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                return i.col;
            }
            ENDHLSL
        }
    }
}
