Shader "Unlit/BrightnessSaturationAndContrast"
{
    Properties
    {
        _MainTex ("Main Tex", 2D) = "white" {}
        _Brightness("Brightness", float) = 1
        _Saturation("Saturation", float) = 1
        _Contrast("Contrast", float) = 1
    }
    SubShader
    {
        Pass
        {
            Cull Off
            ZTest Always
            ZWrite Off

            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _Brightness;
            float _Saturation;
            float _Contrast;

            float4 _MainTex_ST;

            sampler2D _MainTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 renderTex = tex2D(_MainTex, i.uv);

                fixed3 finalColor = renderTex.rgb * _Brightness;

                fixed luminance = 0.2125 * renderTex.r + 0.7154 * renderTex.g + 0.0721 * renderTex.b;
                
                fixed3 luminanceColor = fixed3(luminance,luminance,luminance);
                
                finalColor =lerp(luminanceColor,finalColor,_Saturation); 
                finalColor =lerp(fixed3(0.5,0.5,0.5),finalColor,_Contrast); 

                return fixed4(finalColor,renderTex.a);
            }
            ENDCG
        }
    }
}
