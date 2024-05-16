Shader "Unlit/TransParentBlendShader"
{
     Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color" ,Color )=(1,1,1,1)
        _AlphaRange("AlphaRange", float) = 1
    }
    SubShader
    {
        Tags {"Queue"="Transparent""IgnoreProjector"="Ture" "RenderType"="Transparent"}

        Pass
        {
            Name "TransParentBlendShaderFront"

            Tags {"LightMode"="ForwardBase"}
         
            ZWrite On
            Blend  SrcAlpha OneMinusSrcAlpha
            Cull Front
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Lighting.cginc"

            struct a2v
            {
                float3 normal : NORMAL;
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color;
            fixed _AlphaRange;

            v2f vert (a2v v)
            {
                v2f o;

                o.pos = UnityObjectToClipPos(v.vertex);

                o.worldNormal = UnityObjectToWorldNormal(v.normal);

                o.worldPos = mul(unity_ObjectToWorld,v.vertex).xyz;

                o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 worldNormal = normalize(i.worldNormal);

                fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
                
                fixed4 texColor = tex2D(_MainTex,i.uv);   

                fixed3 albedo = texColor.rgb + _Color.rgb;

                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

                fixed diffuse = _LightColor0.rgb * albedo * max(0 , dot(worldNormal,worldLightDir));

                return fixed4 ( ambient + diffuse, texColor.a * _AlphaRange);
            }
            ENDCG
        }

        Pass
        {
            Name "TransParentBlendShaderBack"

            Tags {"LightMode"="ForwardBase"}
         
            ZWrite On
            Blend  SrcAlpha OneMinusSrcAlpha
            Cull Back
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Lighting.cginc"

            struct a2v
            {
                float3 normal : NORMAL;
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color;
            fixed _AlphaRange;

            v2f vert (a2v v)
            {
                v2f o;

                o.pos = UnityObjectToClipPos(v.vertex);

                o.worldNormal = UnityObjectToWorldNormal(v.normal);

                o.worldPos = mul(unity_ObjectToWorld,v.vertex).xyz;

                o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 worldNormal = normalize(i.worldNormal);

                fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
                
                fixed4 texColor = tex2D(_MainTex,i.uv);   

                fixed3 albedo = texColor.rgb - _Color.rgb;

                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

                fixed diffuse = _LightColor0.rgb * albedo * max(0 , dot(worldNormal,worldLightDir));

                return fixed4 ( ambient + diffuse, texColor.a * _AlphaRange);
            }
            ENDCG
        }
    }
}
