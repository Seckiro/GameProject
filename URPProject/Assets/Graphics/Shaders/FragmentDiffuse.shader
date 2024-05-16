Shader "Unlit/Diffuse_Fragment"
{
    Properties
    {
        _Diffuse("Diffuse",Color)=(1,1,1,1)
    }
    SubShader
    {
        //Tags { "LightMode" = "FrowardBase" }

        LOD 100

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Lighting.cginc"

            fixed4 _Diffuse;

            struct a2f
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float3 worldNormal : TEXCOORD0; 
                float4 vertex : SV_POSITION;
            };

            v2f vert (a2f i)
            {
                v2f o ;

                o.vertex = UnityObjectToClipPos(i.vertex);

                o.worldNormal = mul((float3x3) unity_WorldToObject,i.normal);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

                fixed3 worldNormal = normalize(i.worldNormal);

                fixed3 worldLight = normalize(_WorldSpaceLightPos0.xyz);
                
                fixed diffuse = _LightColor0.rgb * _Diffuse.rgb * saturate(dot(worldNormal,worldLight));

                fixed3 color = ambient + diffuse;

                return fixed4(color,1.0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
