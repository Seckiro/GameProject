Shader "Unlit/TestTextureBlend"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _BaseMap ("BaseMap", 2D) = "white" {}
        _MainColor ("MainColor", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
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

            sampler2D _BaseMap;
            sampler2D _MainTex;

            float4 _MainColor;

            float4 _BaseMap_ST;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 mainTex = tex2D(_MainTex, i.uv);
                fixed4 baseMap = tex2D(_BaseMap, i.uv);

                fixed4 fineColor = mainTex * baseMap * _MainColor;

                return fineColor;
            }
            
            ENDCG
        }
    }
}
