Shader "Unlit/FogWithDepthTexture"
{
    Properties
    {
        _MainTex ("Main Tex", 2D) = "white" {}
        _FogColor("Fog Color", Color) = (1,1,1,1)
        _FogDensity("Fog Density", Float) =1
        _FogStart("Fog Start", Float) = 1
        _FogEnd("Fog End", Float) = 1
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

            uniform half4 _MainTex_TexelSize;

            float _FogDensity;
            float _FogStart;
            float _FogEnd;
            float _FogScale;
            float _FogAlpha;
            float _FogAlphaScale;
            
            float4 _FogColor;

            float4x4 _FrustumCornersRay;

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;

            float4 _MainTex_ST;

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv_depth : TEXCOORD1;
                float4 interpolatedRay : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_img v)
            {
                v2f o;
                o.uv = v.texcoord;
                o.uv_depth = v.texcoord;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                #if UNITY_UV_STARTS_AT_TOP
                    if(_MainTex_TexelSize.y<0)
                    {
                        o.uv_depth.y = 1 - o.uv_depth.y;    
                    }
                #endif

                int index = 0;
                if(v.texcoord.x < 0.5 && v.texcoord.y < 0.5)
                {
                    index = 0;
                    
                }
                else if(v.texcoord.x > 0.5 && v.texcoord.y < 0.5)
                {
                    index = 1;
                }
                else if(v.texcoord.x > 0.5 && v.texcoord.y > 0.5)
                {
                    index = 2;
                }
                else
                {
                    index = 3;
                }
                #if UNITY_UV_STARTS_AT_TOP
                    if(_MainTex_TexelSize.y<0)
                    {
                        index = 3 - index;  
                    }
                #endif
                o.interpolatedRay = _FrustumCornersRay[index];
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float linearDepth=LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,i.uv_depth));

                float3 worldPos = _WorldSpaceCameraPos + linearDepth * i.interpolatedRay.xyz;

                float fogDensity= saturate((_FogEnd - worldPos.y) / (_FogEnd - _FogStart) * _FogDensity );
                
                fixed4 finalColor = tex2D(_MainTex,i.uv);

                return fixed4(lerp(finalColor.rgb, _FogColor, fogDensity), finalColor.a);
            }
            ENDCG
        }
    }
}
