Shader "Unlit/WorldNormalTexture"
{
    Properties
    {
        [MainTexture]_MainTex ("MainTexture", 2D) = "white" {}
        [MainColor] _Color("Color",Color)=(1,1,1,1)
        _BumpMap("Narmal", 2D) = "white" {}
        _BumpScale("Bump Scale", float) = 1.0
        _Specular("Specular",Color)=(1,1,1,1)
        _Gloss("Gloss",Range(8.0,256))=20
    }
    SubShader
    {
        Tags 
        { 
            "LightMode"="UniversalForward"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            sampler2D _MainTex;
            
            sampler2D _BumpMap;

            float _Gloss; 
            float _BumpScale;
            float4 _Color;
            float4 _Specular;

            float4 _MainTex_ST;
            float4 _BumpMap_ST;

            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                float4 Ttow0 : TEXCOORD1;
                float4 Ttow1 : TEXCOORD2;
                float4 Ttow2 : TEXCOORD3;
            };

            v2f vert (a2v v)
            {
                v2f o=(v2f)0;

                o.pos = UnityObjectToClipPos(v.vertex);

                o.uv.xy = v.texcoord * _MainTex_ST.xy + _MainTex_ST.zw;

                o.uv.zw = v.texcoord * _BumpMap_ST.xy + _BumpMap_ST.zw;

                float3 worldPos = mul(unity_ObjectToWorld,v.vertex).xyz;

                float3 worldNormal = UnityObjectToWorldNormal(v.normal);

                float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);

                float3 worldBinormal = cross(normalize(v.normal),normalize(v.tangent.xyz)) * v.tangent.w;

                o.Ttow0 = float4(worldTangent.x,worldBinormal.x,worldNormal.x,worldPos.x);

                o.Ttow1 = float4(worldTangent.y,worldBinormal.y,worldNormal.y,worldPos.y);

                o.Ttow2 = float4(worldTangent.z,worldBinormal.z,worldNormal.z,worldPos.z);

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {

                float3 worldPos= float3(i.Ttow0.w,i.Ttow1.w,i.Ttow2.w);

                float3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));

                float3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                
                float3 bump = UnpackNormal(tex2D(_BumpMap,i.uv.zw));

                bump.xy *= _BumpScale;

                bump.z = sqrt(1.0 - saturate(dot(bump.xy,bump.xy)));

                bump = normalize(float3(dot(i.Ttow0.xyz,bump),dot(i.Ttow1.xyz,bump),dot(i.Ttow2.xyz,bump)));

                float3 albedo = tex2D(_MainTex,i.uv).rgb * _Color.rgb;

                float3 anbient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

                float3 diffuse = _LightColor0.rgb * albedo * max(0,dot(bump,lightDir));

                float3 halfDir = normalize(lightDir + viewDir);

                float3 specular = _LightColor0.rgb * _Specular.rgb * pow(max(0,dot(bump,halfDir)),_Gloss);

                return float4 (anbient+ diffuse + specular,1.0f);
            }
            ENDCG
        }
    }
    Fallback "Specular"
}
