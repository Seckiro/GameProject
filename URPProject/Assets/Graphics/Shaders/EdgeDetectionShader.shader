Shader "Unlit/EdgeDetectionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EdgesOnly("Edges Only", float) = 1
        _EdgesColor("Edges Color", Color) = (0, 0, 0, 1)
        _BackGroundColor("BackGround Color", Color) = (1, 1, 1, 1)
        _SampleDistance("Sample Distance", float) = 1
        _Sensitivity("Sensitivity", Vector) = (0, 0, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Cull Off
            ZWrite Off
            ZTest Always

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;  
            sampler2D _CameraDepthNormalsTexture;  

            uniform half4 _MainTex_TexelSize;

            fixed _EdgesOnly;
            fixed4 _EdgesColor;
            fixed4 _BackGroundColor;
            fixed  _SampleDistance;
            fixed4  _Sensitivity;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv[5] : TEXCOORD0;
            };

            fixed luminance(fixed4 color)
            {
                return 0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b;
            }

            fixed sobel(v2f i)
            {
                const half GX[9] ={-1,-2,-1,0,0,0,1,2,1};
                const half GY[9] ={-1,0,1,-2,0,2,-1,0,1};

                half texColor;

                half edgeX=0;
                half edgeY=0;

                for(int index=0;index<9;index++)
                {
                    texColor = luminance(tex2D(_MainTex,i.uv[index]));
                    edgeX += texColor*GX[index];
                    edgeY += texColor*GY[index];
                } 

                half edge = 1- abs(edgeX) - abs(edgeY);
                return edge;
            }

            fixed CheckSame(half4 center, half4 sample)
            {
                half2 centerNormal = center.xy;
                float centerDepth = DecodeFloatRG(center.zw);

                half2 sampleNormal = sample.xy;
                float sampleDepth = DecodeFloatRG(sample.zw); 

                half2 diffNormal = abs(centerNormal - sampleNormal) * _Sensitivity.x;
                int isSameNormal = (diffNormal.x + diffNormal.y)<0.1;

                float diffDepth = abs (centerDepth - sampleDepth) * _Sensitivity.y;
                int isSampleDepth = diffDepth < 0.1 *centerDepth;

                return isSameNormal * isSampleDepth ? 1.0 : 0.0;
            }

            v2f vert (appdata_img v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float2 uv = v.texcoord;

                o.uv[0] = uv ;

                #if UNITY_UV_STARTS_AT_TOP
                    if  (_MainTex_TexelSize.y<0)
                    {
                        uv.y= 1 - uv.y;
                    }
                #endif

                o.uv[1] = uv + _MainTex_TexelSize.xy * half2(1, 1) * _SampleDistance;
                o.uv[2] = uv + _MainTex_TexelSize.xy * half2(-1, -1) * _SampleDistance;
                o.uv[3] = uv + _MainTex_TexelSize.xy * half2(-1, 1) * _SampleDistance;
                o.uv[4] = uv + _MainTex_TexelSize.xy * half2(1, -1) * _SampleDistance;             

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half4 sample1 = tex2D(_CameraDepthNormalsTexture,i.uv[1]); 
                half4 sample2 = tex2D(_CameraDepthNormalsTexture,i.uv[2]); 
                half4 sample3 = tex2D(_CameraDepthNormalsTexture,i.uv[3]); 
                half4 sample4 = tex2D(_CameraDepthNormalsTexture,i.uv[4]); 

                half edge = 1.0;

                edge *= CheckSame(sample1,sample2);
                edge *= CheckSame(sample3,sample4);

                fixed4 withEdgeColor = lerp(_EdgesColor,tex2D(_MainTex,i.uv[0]),edge);
                fixed4 onlyEdgeColor = lerp(_EdgesColor,_BackGroundColor,edge);

                return lerp(withEdgeColor,onlyEdgeColor,_EdgesOnly);
            }

            ENDCG
        }
    }
}
