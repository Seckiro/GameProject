Shader "Unlit/TextureShader"
{
    Properties
    {
        _MainTex ("MainTexture", 2D) = "white" {}
        _Color("Color",Color)=(1,1,1,1)
        _Specular("Specular",Color)=(1,1,1,1)
        _Gloss("Gloss",Range(8.0,256))=20
    }
    SubShader
    {


        Pass
        {
            Tags 
            { 
                "LightMode"="UniversalForward"
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Lighting.cginc"

            float4 _Color;

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Specular;
            float _Gloss; 


            struct a2v
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal:TEXCOORD0;
                float3 worldPos:TEXCOORD1;
                float2 texcoord : TEXCOORD2;
            };

            
            v2f vert (a2v v)
            {
                v2f o;

                o.pos=UnityObjectToClipPos(v.vertex);

                o.worldNormal=UnityObjectToWorldNormal(v.normal);

                o.worldPos=mul(unity_ObjectToWorld,v.vertex).xyz;

                o.texcoord=v.texcoord.xy*_MainTex_ST.xy+_MainTex_ST.zw;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 worldNormal=normalize(i.worldNormal);
                
                fixed3 worldLightDir=normalize(UnityWorldSpaceLightDir(i.worldPos));
                
                fixed3 albedo=tex2D(_MainTex,i.texcoord).rgb*_Color.rgb;
                
                fixed3 anbient=UNITY_LIGHTMODEL_AMBIENT.xyz*albedo;

                fixed3 diffuse=_LightColor0.rgb*albedo*max(0,dot(worldNormal,worldLightDir));

                fixed3 viewDir=normalize(UnityWorldSpaceViewDir(i.worldPos));

                fixed3 halfDir=normalize(worldLightDir+viewDir);

                fixed3 specular=_LightColor0.rgb * _Specular.rgb * pow(max(0,dot(worldNormal,halfDir)),_Gloss);
                
                return fixed4 (anbient + diffuse + specular,1.0);
            }
            ENDCG
        }
    }
    Fallback "Specular"
}