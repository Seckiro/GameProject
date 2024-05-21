Shader "Unlit/GeometryTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2g
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            struct g2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2g vert (appdata v)
            {
                v2g o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            /*
            [maxvertexcount(1)]
            void geom(point v2g input[1], inout PointStream<g2f> outStream)
            {
                g2f o = (g2f)0;
                o.vertex = input[0].vertex;
                o.uv=input[0].uv;
                outStream.Append(o);
            }
            */
            /*
            [maxvertexcount(2)]
            void geom(line v2g input[2], inout LineStream<g2f> outStream)
            {
                g2f o = (g2f)0;
                for(int i = 0;i < 2 ; i++)
                {
                    o.vertex = input[i].vertex;
                    o.uv=input[i].uv;
                    outStream.Append(o);
                }
            }
            */
            
            [maxvertexcount(3)]
            void geom(triangle v2g input[3], inout TriangleStream<g2f> outStream)
            {
                g2f o = (g2f)0;
                for(int i = 0;i < 3 ; i++)
                {
                    o.vertex =UnityObjectToClipPos(input[i].vertex);
                    o.uv = -input[i].uv;
                    outStream.Append(o);
                }
            }
            
            fixed4 frag (g2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
