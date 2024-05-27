Shader "Debug/GeometryPoint"
{
    Properties
    {
        [HDR]_PointColor ("Point Color",Color)=(1,0,0,1)    
        _PointOffset("Point Offset",Float) =1
    }
    SubShader
    {
        Pass
        {
            NAME "Point Pass"

            Tags { "RenderType"="Opaque" }

            CGPROGRAM

            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            float4 _PointColor;
            float _PointOffset;

            struct a2v
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2g 
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };

            struct g2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2g  vert (a2v v)
            {
                v2g o = (v2g)0;
                o.pos =v.vertex;
                o.normal = v.normal;
                o.uv = v.texcoord;
                
                return o;
            }

            [maxvertexcount(3)]
            void geom (triangle v2g input[3], inout PointStream<g2f> g)
            {
                g2f o = (g2f)0;

                for (int i = 0; i<3; i++)
                {
                    o.pos = input[i].pos+float4(input[i].normal,0)*_PointOffset;
                    o.pos = UnityObjectToClipPos(o.pos);
                    o.uv = input[i].uv;
                    g.Append(o);
                }
            }

            half4 frag (g2f i) : SV_Target
            {
                return _PointColor;
            }
            ENDCG
        }
    }
}
