// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/Diffuse_Vertex"
{
    Properties
    {
        _Diffuse("Diffuse",Color)=(1,1,1,1)
    }
    SubShader
    {
        //Tags { "LightMode" = "FrowardBase" }

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
                float3 color : COLOR; 
                float4 vertex : SV_POSITION;
            };

            v2f vert (a2f i)
            {
                v2f o ;

                o.vertex = UnityObjectToClipPos(i.vertex);

                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

                fixed3 worldNormal = normalize(mul((float3x3) unity_WorldToObject,i.normal));

                fixed3 worldLight = normalize(_WorldSpaceLightPos0.xyz);
                
                fixed diffuse = _LightColor0.rgb * _Diffuse.rgb  * saturate(dot(worldNormal,worldLight));

                o.color = ambient + diffuse;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(i.color,1.0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
