Shader "Debug/GeometryNormal"
{
    Properties
    {
        [HDR]_NormalColorOffset("Normal Color Offset",COLOR) = (1,1,1,1)
        _NormalLength("Normal Length",float) = 1
        _NormalColorRectify("Normal Color Rectify",float)=(0,0,0)
        _NormalColorIntensity("Normal Color Intensity",Range(0,5)) = 0.5
    }
    SubShader
    {
        Pass
        {
            NAME "Normal Pass"

            Tags { "RenderType"="Opaque" }

            CGPROGRAM

            #pragma target 5.0

            #pragma vertex vert
            #pragma geometry geo
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            float _NormalLength;
            float _NormalColorIntensity;
            float3 _NormalColorRectify;
            fixed4 _NormalColorOffset;

            struct v2g
            {
                float4 pos       : POSITION;
                float3 normal    : NORMAL;
                float2 tex0      : TEXCOORD0;
                fixed3 color     : COLOR;
            };

            struct g2f
            {
                float4 pos       : POSITION;
                float2 tex0      : TEXCOORD0;
                fixed3 color     : COLOR;
            };
            
            v2g vert(appdata_base v)
            {
                v2g output = (v2g)0;

                output.pos = mul(unity_ObjectToWorld, v.vertex);

                float3 worldNormal = UnityObjectToWorldNormal(v.normal);

                output.normal = normalize(worldNormal);

                output.tex0 = v.texcoord;

                output.color = v.normal * _NormalColorIntensity + _NormalColorRectify.rgb;

                return output;
            }

            [maxvertexcount(3)]
            void geo(point v2g p[1], inout LineStream<g2f> triStream)
            {
                g2f pIn_1;

                pIn_1.pos = mul(UNITY_MATRIX_VP, p[0].pos);

                pIn_1.tex0 =  p[0].tex0;
                
                pIn_1.color =  p[0].color;

                triStream.Append(pIn_1);

                g2f pIn_2;
                
                float4 pos = p[0].pos + float4(p[0].normal,0) *_NormalLength;
                
                pIn_2.pos = mul(UNITY_MATRIX_VP, pos);
                
                pIn_2.tex0 = p[0].tex0;
                
                pIn_2.color =  p[0].color;

                triStream.Append(pIn_2);
            }

            fixed4 frag(g2f input) : COLOR
            {
                return fixed4(input.color*_NormalColorOffset.rgb,1);
            }

            ENDCG
        }
    }
}
