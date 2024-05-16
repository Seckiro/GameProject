Shader "Unlit/Specular_Fragment"
{
    Properties
    {
        _Diffuse("Diffuse",Color)=(1,1,1,1)
        _Specular("Specular",Color)=(1,1,1,1)
        _Gloss("Gloss",Range(8.0,256))=20
    }
    SubShader
    {
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
                float3 worldNormal : TEXCOORD0; 
                float4 vertex : SV_POSITION;
            };

            v2f vert (a2v i)
            {
                v2f o = (v2f) 0;

                o.vertex = UnityObjectToClipPos(i.vertex);

                o.worldNormal = mul((float3x3) unity_WorldToObject,i.normal);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

                fixed3 worldNormal = normalize(i.worldNormal);

                fixed3 worldLight = normalize(_WorldSpaceLightPos0.xyz);
                
                fixed diffuse = _LightColor0.rgb * _Diffuse.rgb  * saturate(dot(worldNormal,worldLight));

                fixed3 reflectDir = normalize(reflect(-worldLight,worldNormal));

                fixed3 view = normalize(_WorldSpaceCameraPos.xyz-mul(unity_ObjectToWorld,i.vertex).xyz);

                fixed3 specular = _LightColor0.rgb * _Specular.rgb  * pow(saturate(dot(reflectDir,view)),_Gloss);

                fixed3 color = ambient + diffuse + specular;
                
                return fixed4(color.xyz,1) ;
            }
            ENDCG
        }
    }
}
