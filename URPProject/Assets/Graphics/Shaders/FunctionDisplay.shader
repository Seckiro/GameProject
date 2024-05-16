Shader "Unlit/FunctionDisplay"
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
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normal;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                col *= i.normal * 0.5 + fixed4(0.5,0.5,0.5,0);
                //float noiseCol = noise(i.uv * 10.0);
                //fixed4 noiseColor =fixed4(noiseCol,noiseCol,noiseCol,1);
                fixed4 stepCol = fixed4(step(i.normal.y*0.5,0.1),step(i.normal.y*0.5,0.2),step(i.normal.y*0.5,0.3),1);
                //return lerp(stepCol, col, i.normal.y*0.5+0.5);
                return stepCol;
            }
            ENDCG
        }
    }
}
