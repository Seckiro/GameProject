Shader "Unlit/FragmentNormalTexture"
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
            "LightMode"="ForwardBase"
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
                float3 lightDir:TEXCOORD1;
                float3 viewDir : TEXCOORD2;
            };

            

            v2f vert (a2v v)
            {
                v2f o=(v2f)0;

                o.pos = UnityObjectToClipPos(v.vertex);

                o.uv.xy = v.texcoord * _MainTex_ST.xy + _MainTex_ST.zw;

                o.uv.zw = v.texcoord * _BumpMap_ST.xy + _BumpMap_ST.zw;

                float3 binormal = cross(normalize(v.normal),normalize(v.tangent.xyz)) * v.tangent.w;

                float3x3 rotation = float3x3(v.tangent.xyz,binormal,v.normal);
                //转换光线方向从ObjSpeace到切线坐标
                o.lightDir = mul(rotation,ObjSpaceLightDir(v.vertex)).xyz;

                o.viewDir = mul(rotation,ObjSpaceViewDir(v.vertex)).xyz;

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float3 tangentLightDir = normalize(i.lightDir);

                float3 tangentViewDir = normalize(i.viewDir);

                float4 packedNormal = tex2D(_BumpMap,i.uv.zw);
                
                //解压缩法线向量 Unpack Normal
                float3 tangentNormal = UnpackNormal(packedNormal);

                tangentNormal.xy *= _BumpScale;

                tangentNormal.z = sqrt(1.0 - saturate(dot(tangentNormal.xy,tangentNormal.xy)));

                float3 albedo = tex2D(_MainTex,i.uv).rgb * _Color.rgb;

                float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

                float3 diffuse = _LightColor0.rgb * albedo.rgb * max(0,dot(tangentNormal,tangentLightDir));

                float3 halfDir = normalize(tangentLightDir + tangentViewDir);

                float3 specular = _LightColor0.rgb * _Specular.rgb * pow(max(0,dot(tangentNormal,halfDir)),_Gloss);

                return float4 (ambient+ diffuse + specular,1.0f);
            }
            ENDCG
        }
    }
    Fallback "Specular"
}
