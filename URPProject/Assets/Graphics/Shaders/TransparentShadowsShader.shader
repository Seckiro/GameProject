Shader "Unlit/TransparentShadowsShader"
{
    Properties 
    {
        _MainTex ("Main Tex", 2D) = "white" {}
        _Color ("Tex Color", Color) = (1, 1, 1, 1)
        [space]
        _BumpMap ("Normal Map", 2D) = "bump" {}
        [Space]
        _Specular ("Specular Color", Color) = (1, 1, 1, 1)
        _Diffuse ("Diffuse Color", Color) = (1, 1, 1, 1)
        [Space]
        _Gloss ("Gloss", Range(8.0, 256)) = 20
        [Space]
        _Cutoff ("Cutoff", Range(0, 1)) = 1
        _Alpha ("Alpha", Range(0, 1)) = 1
        [Space]
        _Dissolve ("Dissolve", 2D) = "white" {}
        _DissolveFristColor ("Dissolve Frist Color",  Color) = (1, 1, 1, 1)
        _DissolveSecondColor ("Dissolve Second Color",  Color) = (1, 1, 1, 1)
        _DissolveColorPow ("Dissolve Color Pow",  Range(1, 5)) = 1
        _DissolveWidth("Dissolve Width", Range(0, 1)) = 1
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 1
        [Space]
        _ReflectColor("Reflect Color",Color) = (1, 1, 1, 1)
        _ReflectAmount("Reflect Amount",Range(0, 1)) = 1
        _ReflectCubemap("Reflect Cubemap",Cube)="_Skybox"{} 
        [Space]
        _RefractColor("Refract Color",Color) = (1, 1, 1, 1)
        _RefractAmount("Refract Amount",Range(0, 1)) = 1
        _RefractRatio("Refract Ratio",Range(0.0, 0.5)) = 0.5
        _RefractCubemap("Refract Cubemap",Cube)="_Skybox"{}
        [Space]
        _FresnelScale("Fresnel Scale",Range(0, 1)) = 1
        [Space]
        _Frequency("Frequency", Range(0, 100)) = 1
        _Magnitude("Magnitude", Range(0, 100)) = 1
        _InvWaveLengh("InvWave Lengh", Range(0, 100)) = 1
        _WaveSpeed("Wave Speed", Range(0, 100)) = 1
    }
    SubShader 
    {
        Tags {"Queue"="Transparent""IgnoreProjector"="Ture" "RenderType"="TransparentCutout" "DisAbleBatching"="True"}

        Pass 
        { 
            Tags { "LightMode"="ForwardBase" }

            Cull Off
            ZWrite On
            Blend  SrcAlpha OneMinusSrcAlpha

            CGPROGRAM

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase	
            
            float _Gloss;
            float _Alpha;
            float _Cutoff;

            float _DissolveColorPow;
            float _DissolveAmount;
            float _DissolveWidth;
            fixed4 _DissolveFristColor;
            fixed4 _DissolveSecondColor;

            float _ReflectAmount;
            fixed4 _ReflectColor;

            float _RefractAmount;
            fixed4 _RefractColor;
            fixed4 _RefractRatio;

            float _FresnelScale;

            float _Frequency;
            float _Magnitude;
            float _InvWaveLengh;
            float _WaveSpeed;

            fixed4 _Color;
            fixed4 _Specular;

            float4 _MainTex_ST;
            float4 _BumpMap_ST;
            float4 _Dissolve_ST;
            
            float4 _ReflectCubemap_ST;
            float4 _ReflratCubemap_ST;
            
            sampler2D _MainTex;
            sampler2D _BumpMap;
            sampler2D _Dissolve;

            samplerCUBE _ReflectCubemap;
            samplerCUBE _RefractCubemap;
            
            struct a2v 
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 texcoord : TEXCOORD0;
            };
            
            struct v2f 
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                float4 TtoW0 : TEXCOORD1;  
                float4 TtoW1 : TEXCOORD2;  
                float4 TtoW2 : TEXCOORD3; 
                SHADOW_COORDS(4)
                float4 effectUV : TEXCOORD5; 
            };
            
            v2f vert(a2v v) 
            {
                v2f o;

                float4 offset = float4(0.0,0.0,0.0,0.0);
                
                offset.x = sin(_Frequency * _Time.y + v.vertex.x * _InvWaveLengh + v.vertex.y * _InvWaveLengh + v.vertex.z * _InvWaveLengh) * _Magnitude;
                offset.y = sin(_Frequency * _Time.y + v.vertex.x * _InvWaveLengh + v.vertex.y * _InvWaveLengh + v.vertex.z * _InvWaveLengh) * _Magnitude;
                offset.z = sin(_Frequency * _Time.y + v.vertex.x * _InvWaveLengh + v.vertex.y * _InvWaveLengh + v.vertex.z * _InvWaveLengh) * _Magnitude;
                
                o.pos = UnityObjectToClipPos(v.vertex + offset);

                o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw + float2 (0.0,_Time.y * _WaveSpeed);
                o.uv.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw + float2 (0.0,_Time.y * _WaveSpeed);

                o.effectUV.xy = v.texcoord.xy * _Dissolve_ST.xy + _Dissolve_ST.zw + float2 (0.0,_Time.y * _WaveSpeed);

                TANGENT_SPACE_ROTATION;
                
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;  
                fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
                fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
                fixed3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w; 

                o.TtoW0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);  
                o.TtoW1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);  
                o.TtoW2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);  

                TRANSFER_SHADOW(o);
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target 
            {
                float3 worldPos = float3(i.TtoW0.w, i.TtoW1.w, i.TtoW2.w);
                fixed3 worldNormal = normalize(float3(i.TtoW0.z, i.TtoW1.z, i.TtoW2.z));

                fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
                fixed3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));

                fixed3 worldReflect = reflect(-viewDir, worldNormal);
                fixed3 worldRefract = refract(-viewDir, worldNormal,_RefractRatio);

                fixed3 bump = UnpackNormal(tex2D(_BumpMap, i.uv.zw));
                bump = normalize(half3(dot(i.TtoW0.xyz, bump), dot(i.TtoW1.xyz, bump), dot(i.TtoW2.xyz, bump)));

                fixed4 albedo = tex2D(_MainTex, i.uv.xy) * _Color;

                clip(albedo.a - _Cutoff);

                fixed4 dissolve = tex2D(_Dissolve, i.effectUV.xy);
                
                clip(dissolve.r - _DissolveAmount);

                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
                
                fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(bump, lightDir));
                
                fixed3 halfDir = normalize(lightDir + viewDir);
                fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(max(0, dot(bump, halfDir)), _Gloss);
                
                fixed3 reflection = texCUBE(_ReflectCubemap,worldReflect).rgb * _ReflectColor.rgb;
                fixed3 refraction = texCUBE(_RefractCubemap,worldRefract).rgb * _RefractColor.rgb;

                fixed fresnel = _FresnelScale + (1 - _FresnelScale) * pow(1 - dot(viewDir, worldNormal),5);

                fixed t = 1 - smoothstep(0.0,_DissolveWidth, dissolve.r - _DissolveAmount);

                fixed3 dissolveColor= lerp(_DissolveFristColor,_DissolveSecondColor,t);

                dissolveColor = pow(dissolveColor,_DissolveColorPow);

                UNITY_LIGHT_ATTENUATION(atten, i, worldPos);

                fixed3  finalColor = lerp(diffuse, reflection, _ReflectAmount);

                finalColor = ambient + specular + lerp(finalColor, refraction, saturate(fresnel) + _RefractAmount) * atten; 

                finalColor = lerp(finalColor,dissolveColor, t * step(0.0001,_DissolveAmount)) ; 

                return fixed4(finalColor , _Alpha );
            }
            ENDCG
        }
        
        Pass 
        { 
            Tags { "LightMode"="ForwardAdd" }
            
            Blend  One One
            
            CGPROGRAM
            
            #pragma multi_compile_fwdadd

            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
            
            float _Gloss;
            float _Alpha;
            float _Cutoff;

            fixed4 _Color;
            fixed4 _Specular;

            float4 _MainTex_ST;
            float4 _BumpMap_ST;

            sampler2D _MainTex;
            sampler2D _BumpMap;
            
            struct a2v 
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 texcoord : TEXCOORD0;
            };
            
            struct v2f 
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                float4 TtoW0 : TEXCOORD1;  
                float4 TtoW1 : TEXCOORD2;  
                float4 TtoW2 : TEXCOORD3; 
                SHADOW_COORDS(4)
            };
            
            v2f vert(a2v v)
            {
                v2f o;
                
                o.pos = UnityObjectToClipPos(v.vertex);
                
                o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                o.uv.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;  
                fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);  
                fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);  
                fixed3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w; 
                
                o.TtoW0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
                o.TtoW1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
                o.TtoW2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);  
                
                TRANSFER_SHADOW(o);
                
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target 
            {
                float3 worldPos = float3(i.TtoW0.w, i.TtoW1.w, i.TtoW2.w);

                fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
                fixed3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                
                fixed3 bump = UnpackNormal(tex2D(_BumpMap, i.uv.zw));
                bump = normalize(half3(dot(i.TtoW0.xyz, bump), dot(i.TtoW1.xyz, bump), dot(i.TtoW2.xyz, bump)));
                
                fixed4 albedo = tex2D(_MainTex, i.uv.xy) * _Color;
                
                clip(albedo.a - _Cutoff);

                fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(bump, lightDir));
                
                fixed3 halfDir = normalize(lightDir + viewDir);
                fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(max(0, dot(bump, halfDir)), _Gloss);
                
                UNITY_LIGHT_ATTENUATION(atten, i, worldPos);

                return fixed4((diffuse + specular) * atten, _Alpha);
            }
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            fixed4 _Color;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            void frag(v2f i, out fixed4 color : COLOR)
            {
                color = _Color;
            }

            [maxvertexcount(3)]
            void geom(triangle v2f input[3], inout TriangleStream<v2f> triStream)
            {
                v2f o;
                for (int i = 0; i < 3; i++)
                {
                    o = input[i];
                    o.pos = UnityObjectToClipPos(o.pos);
                    triStream.Append(o);
                }
            }
            ENDCG
        }
        Pass 
        { 
            Tags { "LightMode"="ShadowCaster" }

            CGPROGRAM
            
            #pragma multi_compile_shadowcaster

            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            
            float _Frequency;
            float _Magnitude;
            float _InvWaveLengh;
            float _WaveSpeed;

            fixed4 _Color;
            float _Cutoff;
            float _DissolveAmount;

            float4 _MainTex_ST;
            float4 _Dissolve_ST;
            
            sampler2D _MainTex;
            sampler2D _Dissolve;

            struct a2v 
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float4 texcoord : TEXCOORD0; 
            };
            
            struct v2f 
            {
                float4 effectUV : TEXCOORD0; 
                V2F_SHADOW_CASTER;
            };
            
            v2f vert(a2v v)
            {
                v2f o;

                float4 offset =float4(0,0,0,0);

                offset.x = sin(_Frequency * _Time.y + v.vertex.x * _InvWaveLengh + v.vertex.y * _InvWaveLengh + v.vertex.z * _InvWaveLengh) * _Magnitude;
                offset.z = sin(_Frequency * _Time.y + v.vertex.x * _InvWaveLengh + v.vertex.y * _InvWaveLengh + v.vertex.z * _InvWaveLengh) * _Magnitude;

                v.vertex += offset;
                
                o.effectUV.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw + float2 (0.0,_Time.y * _WaveSpeed);
                o.effectUV.xy = v.texcoord.xy * _Dissolve_ST.xy + _Dissolve_ST.zw + float2 (0.0,_Time.y * _WaveSpeed);

                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o);
                
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target 
            {
                fixed4 albedo = tex2D(_MainTex, i.effectUV.xy) * _Color;

                clip(albedo.a - _Cutoff);

                fixed4 dissolve = tex2D(_Dissolve, i.effectUV.xy);
                
                clip(dissolve.r - _DissolveAmount);

                SHADOW_CASTER_FRAGMENT(i);
            }
            ENDCG
        }


    } 
    FallBack "Transparent/Cutout/VertexLit"
}
