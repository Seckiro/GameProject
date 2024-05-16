Shader "Unlit/MotionBlurWithDepthTexture"
{
    Properties
    {
        _MainTex ("Main Tex", 2D) = "white" {}
        _BlurSize("Blur Size", Float) = 1
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
            sampler2D _CameraDepthTexture;

            float4 _MainTex_ST;
            uniform half4 _MainTex_TexelSize;

            float _BlurSize;

            float4x4  _PreviousViewProjectionMatrix;
            float4x4  _CurrentViewProjectionInverseMatrix;

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv_depth : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_img v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.uv_depth = v.texcoord;

                #if UNITY_UV_STARTS_AT_TOP
                    if(_MainTex_TexelSize.y<0)
                    {
                        o.uv_depth.y = 1 - o.uv_depth.y;    
                    }
                #endif

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float d = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,i.uv_depth);
                float4 H = float4(i.uv.x * 2 - 1,i.uv.y * 2 - 1,d * 2 - 1, 1);
                float4 D = mul(_CurrentViewProjectionInverseMatrix,H);
                float4 worldPos = D / D.w;
                float4 currentPos = H;
                float4 previousPos = mul(_PreviousViewProjectionMatrix,worldPos);
                previousPos /= previousPos.w;

                float2 velocity = (currentPos.xy - previousPos.xy)/2.0f;

                float2 uv = i.uv;
                float4 col = tex2D(_MainTex, uv);
                uv += velocity * _BlurSize;
                for(int index = 1;index < 3; index ++)
                {
                    float4 currentColor = tex2D(_MainTex, uv);
                    col += currentColor;
                    uv += velocity * _BlurSize;
                }
                col /= 3;

                return fixed4(col.rgb,1.0);
            }
            ENDCG
        }
    }
}
