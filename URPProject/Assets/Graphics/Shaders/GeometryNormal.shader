Shader "Unlit/GeometryNormal"
{
    Properties
    {
        _LineLength("LineLength",float) = 1.
        _LineColorOffset("ColorOffset",COLOR) = (1,1,1,1)
        _LineColorRectify("ColorRectify",float)=(0,0,0)
        _LineColorIntensity("ColorIntensity",Range(0,5)) = 0.5
    }

    SubShader
    {
        Pass
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200
            
            CGPROGRAM
            #pragma target 5.0
            #pragma vertex vert
            #pragma geometry geo
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            float _LineLength;
            float _LineColorIntensity;
            float3 _LineColorRectify;
            fixed4 _LineColorOffset;

            struct v2g
            {
                float4 pos       : POSITION;
                
                float3 normal    : NORMAL;
                
                float2 tex0        : TEXCOORD0;

                fixed3 color        :COLOR;
            };

            struct g2f
            {
                float4 pos       : POSITION;
                
                float2 tex0        : TEXCOORD0;

                fixed3 color        :COLOR;
            };
            
            v2g vert(appdata_base v)
            {
                v2g output = (v2g)0;

                output.pos = mul(unity_ObjectToWorld, v.vertex);

                float3 worldNormal = UnityObjectToWorldNormal(v.normal);

                output.normal = normalize(worldNormal);

                output.tex0 = v.texcoord;

                output.color = v.normal * _LineColorIntensity + _LineColorRectify.rgb;

                return output;
            }

            //几何着色器，放在顶点和片元之间。
            //maxvertexcount设置从顶点着色器到几何着色器每次输出最大顶点数量
            //输入修饰有point、line、triangle、lineadj、triangleadj分别对应点、线、三角形、有临接的线段、有邻接的三角形
            //inout输出修饰类型有PointStream、LineStream、TriangleStream，分别显示顶点、线段、三角面
            //输入顶点的信息，inout这里是将一个数据传入函数，在函数中的修改会返回到函数外的数据
            //一次接受point 的1个顶点数据，输出为LineStream的4条线段
            [maxvertexcount(4)]
            void geo(point v2g p[1], inout LineStream<g2f> triStream)
            {
                g2f pIn_1;

                pIn_1.pos = mul(UNITY_MATRIX_VP, p[0].pos);

                pIn_1.tex0 =  p[0].tex0;
                
                pIn_1.color =  p[0].color;

                triStream.Append(pIn_1);

                g2f pIn_2;
                
                float4 pos = p[0].pos + float4(p[0].normal,0) *_LineLength;
                
                pIn_2.pos = mul(UNITY_MATRIX_VP, pos);
                
                pIn_2.tex0 = p[0].tex0;
                
                pIn_2.color =  p[0].color;

                triStream.Append(pIn_2);
            }

            fixed4 frag(g2f input) : COLOR
            {
                return fixed4(input.color*_LineColorOffset.rgb,1);
            }

            ENDCG
        }
    }
}
