Shader "Unlit/GeometryTest"
{
    Properties
    {
        [HDR]_TriangleColor ("TriangleColor",Color)=(0,0,1,1)
        [HDR]_LineColor ("LineColor",Color)=(0,1,0,1)
        [HDR]_PointColor ("PointColor",Color)=(1,0,0,1)    
        _Offset("ScaleTriangle",Float) =1
        _Offset1("ScaleLine",Float) =1
        _Offset2("ScalePoint",Float) =1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        /* Pass
        {
            //输出三角形的pass
            NAME "TRIANGLE PASS"
            CGPROGRAM
            #pragma vertex vert
            //几何着色器声明
            #pragma geometry geom
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            float4 _TriangleColor;
            float _Offset;

            struct a2v
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float3 normal : NORMAL;
            };
            //几何着色器需要传递的数据
            struct v2g 
            {
                float4 pos : POSITION;
                float2 uvG : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };
            //再从几何着色器变化过的数据传到片元着色器的结构      所以可以不用v2f 不过一个名字 想怎么取随便 自己别忘了就行
            struct g2f
            {
                float2 uv : TEXCOORD0;
                float4 posf : SV_POSITION;
            };

            //取代原本v2f 将顶点结构传入到结合着色器需要的结构中
            v2g  vert (a2v v)
            {
                v2g o = (v2g)0;
                o.pos =v.vertex;
                o.normal = v.normal;
                o.uvG = v.texcoord;
                
                return o;
            }

            [maxvertexcount(3)]
            void geom (triangle v2g input[3], inout TriangleStream<g2f> g)
            {
                g2f o = (g2f)0;
                for (int i = 0; i<3; i++)
                {
                    o.posf = input[i].pos+float4(input[i].normal,0)*_Offset;
                    o.uv = input[i].uvG;
                    o.posf = UnityObjectToClipPos(o.posf);
                    g.Append(o);
                }
            }

            half4 frag (g2f i) : SV_Target
            {
                return _TriangleColor;
            }
            ENDCG
        }
        
        Pass
        {
            NAME "LINE PASS"
            CGPROGRAM
            #pragma vertex vert
            //几何着色器声明
            #pragma geometry geom
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            float4 _LineColor;
            float _Offset1;

            struct a2v
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2g 
            {
                float4 pos : POSITION;
                float2 uvG : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };

            struct g2f
            {
                float2 uv : TEXCOORD0;
                float4 posf : SV_POSITION;
            };

            v2g  vert (a2v v)
            {
                v2g o = (v2g)0;
                o.pos = v.vertex;
                o.normal = v.normal;
                o.uvG = v.texcoord;
                
                return o;
            }

            [maxvertexcount(3)]
            void geom (triangle v2g input[3], inout LineStream<g2f> g)
            {
                for (int i = 0; i<3; i++)
                {
                    g2f o = (g2f)0;
                    o.posf = input[i].pos + float4(input[i].normal,0)*_Offset1;
                    o.posf = UnityObjectToClipPos(o.posf);
                    o.uv = input[i].uvG;
                    g.Append(o);
                }
                
            }

            half4 frag (g2f i) : SV_Target
            {
                return _LineColor;
            }
            ENDCG
        }
        */
        Pass
        {
            //输出三角形的pass
            NAME "Point PASS"
            CGPROGRAM
            #pragma vertex vert
            //几何着色器声明
            #pragma geometry geom
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            float4 _PointColor;
            float _Offset2;

            struct a2v
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2g 
            {
                float4 pos : POSITION;
                float2 uvG : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };

            struct g2f
            {
                float2 uv : TEXCOORD0;
                float4 posf : SV_POSITION;
            };

            v2g  vert (a2v v)
            {
                v2g o = (v2g)0;
                o.pos =v.vertex;
                o.normal = v.normal;
                o.uvG = v.texcoord;
                
                return o;
            }

            [maxvertexcount(3)]
            void geom (triangle v2g input[3], inout PointStream<g2f> g)
            {
                g2f o = (g2f)0;

                for (int i = 0; i<3; i++)
                {
                    o.posf = input[i].pos+float4(input[i].normal,0)*_Offset2;
                    o.posf = UnityObjectToClipPos(o.posf);
                    o.uv = input[i].uvG;
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
