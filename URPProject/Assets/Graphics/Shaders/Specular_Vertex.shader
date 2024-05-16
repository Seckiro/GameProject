// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/Specular_Vertex"
{
    Properties
    {
        _Diffuse("Diffuse",Color)=(1,1,1,1)
        _Specular("Specular",Color)=(1,1,1,1)
        _Gloss("Gloss",Range(8.0,256))=20
    }
    SubShader
    {
        //Tags { "LightMode" = "FrowardBase" }
        
        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            float4  _Diffuse;
            float4  _Specular;
            float   _Gloss;

            struct a2v
            {
                float3 normal : NORMAL;
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float3 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            v2f vert (a2v i)
            {
                v2f o = (v2f) 0;

                o.vertex = UnityObjectToClipPos(i.vertex);
                
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

                fixed3 worldNormal = normalize(mul((float3x3) unity_WorldToObject,i.normal));

                fixed3 worldLight = normalize(_WorldSpaceLightPos0.xyz);
                
                fixed diffuse = _LightColor0.rgb * _Diffuse.rgb  * saturate(dot(worldNormal,worldLight));

                fixed3 reflectDir = normalize(reflect(-worldLight,worldNormal));

                fixed3 view = normalize(_WorldSpaceCameraPos.xyz-mul(unity_ObjectToWorld,i.vertex).xyz);

                fixed3 specular = _LightColor0.rgb * _Specular.rgb  * pow(saturate(dot(reflectDir,view)),_Gloss);

                o.color = ambient + diffuse + specular;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(i.color,0.5) ;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
