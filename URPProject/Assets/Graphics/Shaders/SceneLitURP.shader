Shader "Unlit/SceneLitURP"
{
    Properties
    {
        [MainTex]_MainTex ("Main Tex", 2D) = "white" {}
        [MainColor]_Color ("Main Color", Color) = (1, 1, 1, 1)
        _BaseMap ("Base Tex", 2D) = "white" {}
        _BlendProportion("Blend Proportion", Range(-10, 10)) = 1
        
        _RampTex("Ramp Tex", 2D) = "white"{}

        _SpecularMask("SpecularMask", 2D) = "white"{}
        _SpecularScale("SpecularScale", Range(-2, 2)) = 1.0

        [space]
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpScale("Bump Scale",  Range(-5,5)) = 1.0
        [Space]
        _Specular ("Specular Color", Color) = (1, 1, 1, 1)
        _Diffuse ("Diffuse Color", Color) = (1, 1, 1, 1)
        [Space]
        _Gloss ("Gloss", Range(8.0, 256)) = 20
        [Space]
        _Ambient ("Ambient", Range(0.0, 1)) = 1
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

        [HDR]_TriangleColor ("Triangle Color",Color)=(1,0,0,1)    
        _TriangleOffset("Triangle Offset",Float) =1
        _Frequency("Frequency", Range(0, 100)) = 1
        _Magnitude("Magnitude", Range(0, 100)) = 1
        _InvWaveLengh("InvWave Lengh", Range(0, 100)) = 1
        _WaveSpeed("Wave Speed", Range(0, 100)) = 1

        [Toggle(_AdditionalLights)] _AddLights("AddLights", Float) = 1
    }
    SubShader
    {
        Tags {"RenderPipeline"="UniversalPipeline" "Queue" = "Transparent" "IgnoreProjector" = "Ture" "RenderType" = "TransparentCutout" "DisAbleBatching" = "True"}
        
        Pass
        {
            NAME "UniversalForward"

            Tags {"LightMode"="UniversalForward"}
            
            Cull Front
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM

            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            #pragma shader_feature _AdditionalLights

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            CBUFFER_START(UnityPerMaterial)
                float _Gloss;
                float _Ambient;
                float _SpecularScale;
                float _Alpha;
                float _Cutoff;
                float _BumpScale;
                float _BlendProportion;
                float _DissolveColorPow;
                float _DissolveAmount;
                float _DissolveWidth;
                float4 _DissolveFristColor;
                float4 _DissolveSecondColor;

                float _ReflectAmount;
                float4 _ReflectColor;

                float _RefractAmount;
                float4 _RefractColor;
                float _RefractRatio;

                float _FresnelScale;

                float _Frequency;
                float _Magnitude;
                float _InvWaveLengh;
                float _WaveSpeed;

                float4 _Color;
                float4 _Specular;

                float4 _MainTex_ST;
                float4 _BaseMap_ST;
                float4 _BumpMap_ST;
                float4 _Dissolve_ST;
                
                float4 _ReflectCubemap_ST;
                float4 _ReflratCubemap_ST;

                float4 _LightDirection;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            TEXTURE2D(_SpecularMask);
            SAMPLER(sampler_SpecularMask);

            TEXTURE2D(_RampTex);
            SAMPLER(sampler_RampTex);

            TEXTURE2D(_BumpMap);
            SAMPLER(sampler_BumpMap);

            TEXTURE2D(_Dissolve);
            SAMPLER(sampler_Dissolve);

            TEXTURECUBE(_ReflectCubemap);
            SAMPLER(sampler_ReflectCubemap);

            TEXTURECUBE(_RefractCubemap);
            SAMPLER(sampler_RefractCubemap);
            
            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 texcoord : TEXCOORD0;
            };

            struct v2g 
            {
                float4 pos : POSITION;
                float4 uv : TEXCOORD0;
                float4 TtoW0 : TEXCOORD1;  
                float4 TtoW1 : TEXCOORD2;  
                float4 TtoW2 : TEXCOORD3; 
                float4 effectUV : TEXCOORD4; 
            };

            struct g2f 
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                float4 TtoW0 : TEXCOORD1;  
                float4 TtoW1 : TEXCOORD2;  
                float4 TtoW2 : TEXCOORD3; 
                float4 effectUV : TEXCOORD4; 
            };

            v2g vert (a2v v)
            {
                v2g o = (v2g)0;

                float3 worldPos =  mul(UNITY_MATRIX_M, v.vertex).xyz;  
                float3 worldNormal = TransformObjectToWorldNormal(v.normal);
                float3 worldTangent = TransformObjectToWorldDir(v.tangent.xyz);
                float3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w; 

                o.pos = v.vertex;

                o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw + float2 (0.0,_Time.y * _WaveSpeed);
                o.uv.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw + float2 (0.0,_Time.y * _WaveSpeed);

                o.TtoW0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);  
                o.TtoW1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);  
                o.TtoW2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);  

                return o;
            }

            [maxvertexcount(3)]
            void geom (triangle v2g input[3], inout TriangleStream<g2f> g)
            {
                g2f o = (g2f)0;
                for (int i = 0; i<3; i++)
                {
                    o.uv = input[i].uv;
                    o.pos = input[i].pos;

                    float posValue = sin( _Frequency * _Time.y + o.pos.x * _InvWaveLengh +o.pos.y * _InvWaveLengh + o.pos.z * _InvWaveLengh) * _Magnitude;
                    float4 offset = float4(posValue,posValue,posValue,posValue);

                    o.effectUV.xy = input[i].uv.xy * _Dissolve_ST.xy + _Dissolve_ST.zw + float2 (0.0 ,_Time.y * _WaveSpeed);

                    o.pos = TransformObjectToHClip(o.pos + offset);

                    o.TtoW0 = input[i].TtoW0;  
                    o.TtoW1 = input[i].TtoW1;  
                    o.TtoW2 = input[i].TtoW2; 

                    g.Append(o);
                }

            }

            float4 frag (g2f i) : SV_Target
            {         
                float4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                float4 baseTex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);
                
                albedo = lerp (albedo, baseTex, _BlendProportion) * _Color;

                clip(albedo.a - _Cutoff);

                float4 dissolve = SAMPLE_TEXTURE2D(_Dissolve, sampler_Dissolve, i.effectUV.xy);
                
                clip(dissolve.r - _DissolveAmount);

                float dissolveColorBlend = 1 - smoothstep(0.0,_DissolveWidth, dissolve.r - _DissolveAmount);
                float3 dissolveColor= lerp(_DissolveFristColor,_DissolveSecondColor,dissolveColorBlend);

                dissolveColor = pow(dissolveColor,_DissolveColorPow);

                half3 worldPos = float3(i.TtoW0.w,i.TtoW1.w,i.TtoW2.w);
                half3 worldNormal = normalize(float3(i.TtoW0.z,i.TtoW1.z,i.TtoW2.z));

                half3 viewDir = normalize(_WorldSpaceCameraPos.xyz - worldPos);

                //half3 ambient = half3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w);
                half3 ambient = SampleSH(worldNormal) * _Ambient;

                float4 shadowCoord = TransformWorldToShadowCoord(worldNormal);
                Light mainLight = GetMainLight(shadowCoord);

                half halfLambert = saturate(dot(worldNormal, normalize(mainLight.direction)) * 0.5 + 0.5);
                half3 diffuseColor = SAMPLE_TEXTURE2D(_RampTex, sampler_RampTex, half2(halfLambert, halfLambert)).rgb;
                
                //half3 diffuse = mainLight.color * albedo * diffuseColor *  mainLight.shadowAttenuation * mainLight.distanceAttenuation;
                half3 diffuse = mainLight.color * albedo * halfLambert *  mainLight.shadowAttenuation * mainLight.distanceAttenuation;
                
                #if _AdditionalLights
                    uint addLightCount = GetAdditionalLightsCount();
                    for(uint iu = 0; iu < addLightCount; iu++) 
                    {
                        Light addLight = GetAdditionalLight(iu, worldPos);
                        half3 addLightDiffuse = addLight.color * albedo * saturate(dot(worldNormal, normalize(addLight.direction)) * 0.5 + 0.5);
                        diffuse += addLightDiffuse * addLight.shadowAttenuation * addLight.distanceAttenuation;
                    }
                #endif

                float3 bump = UnpackNormal(SAMPLE_TEXTURECUBE(_BumpMap,sampler_BumpMap, i.uv.zw));
                bump.xy = bump.xy * _BumpScale;
                bump.z = sqrt(1.0 - saturate(dot(bump.xy,bump.xy)));
                bump = normalize(half3(dot(i.TtoW0.xyz, bump), dot(i.TtoW1.xyz, bump), dot(i.TtoW2.xyz, bump)));


                half specularMask = SAMPLE_TEXTURE2D(_SpecularMask, sampler_SpecularMask, i.uv).r * _SpecularScale;
                half3 specular = mainLight.color.rgb * _Specular.rgb * pow(max(0, dot(bump, normalize(mainLight.direction + viewDir))), _Gloss) * specularMask;

                float fresnel = _FresnelScale + (1 - _FresnelScale) * pow(1 - saturate(dot(viewDir, bump)),4); 

                float3 worldReflect = reflect(-viewDir, worldNormal);
                float3 reflection = SAMPLE_TEXTURECUBE(_ReflectCubemap, sampler_ReflectCubemap, worldReflect).rgb * _ReflectColor.rgb;
                float3 reflectionColor = lerp(diffuse, reflection, fresnel) * _ReflectAmount;

                float3 worldRefract = refract(-viewDir, worldNormal, _RefractRatio);
                float3 refraction = SAMPLE_TEXTURECUBE(_RefractCubemap, sampler_RefractCubemap, worldRefract).rgb * _RefractColor.rgb;
                float3 refractionColor = lerp(diffuse, refraction, 1 - fresnel) * _RefractAmount;

                float3 lightFinalColor = ambient + specular + (refractionColor * reflectionColor);

                float3 finalColor = lerp(lightFinalColor, dissolveColor, dissolveColorBlend * step(0.0001,_DissolveAmount)) ; 

                return float4(finalColor , _Alpha);
            }
            ENDHLSL
        }
        
        Pass
        {
            NAME "SRPDefaultUnlit"

            Tags {"LightMode"="SRPDefaultUnlit"}
            
            Cull Back
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM

            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            #pragma shader_feature _AdditionalLights

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            CBUFFER_START(UnityPerMaterial)
                float _Gloss;
                float _Ambient;
                float _SpecularScale;
                float _Alpha;
                float _Cutoff;
                float _BumpScale;
                float _BlendProportion;
                float _DissolveColorPow;
                float _DissolveAmount;
                float _DissolveWidth;
                float4 _DissolveFristColor;
                float4 _DissolveSecondColor;

                float _ReflectAmount;
                float4 _ReflectColor;

                float _RefractAmount;
                float4 _RefractColor;
                float _RefractRatio;

                float _FresnelScale;

                float _Frequency;
                float _Magnitude;
                float _InvWaveLengh;
                float _WaveSpeed;

                float4 _Color;
                float4 _Specular;

                float4 _MainTex_ST;
                float4 _BaseMap_ST;
                float4 _BumpMap_ST;
                float4 _Dissolve_ST;
                
                float4 _ReflectCubemap_ST;
                float4 _ReflratCubemap_ST;

                float4 _LightDirection;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            TEXTURE2D(_SpecularMask);
            SAMPLER(sampler_SpecularMask);

            TEXTURE2D(_RampTex);
            SAMPLER(sampler_RampTex);

            TEXTURE2D(_BumpMap);
            SAMPLER(sampler_BumpMap);

            TEXTURE2D(_Dissolve);
            SAMPLER(sampler_Dissolve);

            TEXTURECUBE(_ReflectCubemap);
            SAMPLER(sampler_ReflectCubemap);

            TEXTURECUBE(_RefractCubemap);
            SAMPLER(sampler_RefractCubemap);
            
            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 texcoord : TEXCOORD0;
            };

            struct v2g 
            {
                float4 pos : POSITION;
                float4 uv : TEXCOORD0;
                float4 TtoW0 : TEXCOORD1;  
                float4 TtoW1 : TEXCOORD2;  
                float4 TtoW2 : TEXCOORD3; 
                float4 effectUV : TEXCOORD4; 
            };

            struct g2f 
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                float4 TtoW0 : TEXCOORD1;  
                float4 TtoW1 : TEXCOORD2;  
                float4 TtoW2 : TEXCOORD3; 
                float4 effectUV : TEXCOORD4; 
            };

            v2g vert (a2v v)
            {
                v2g o = (v2g)0;

                float3 worldPos =  mul(UNITY_MATRIX_M, v.vertex).xyz;  
                float3 worldNormal = TransformObjectToWorldNormal(v.normal);
                float3 worldTangent = TransformObjectToWorldDir(v.tangent.xyz);
                float3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w; 

                o.pos = v.vertex;

                o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw + float2 (0.0,_Time.y * _WaveSpeed);
                o.uv.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw + float2 (0.0,_Time.y * _WaveSpeed);

                o.TtoW0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);  
                o.TtoW1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);  
                o.TtoW2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);  

                return o;
            }

            [maxvertexcount(3)]
            void geom (triangle v2g input[3], inout TriangleStream<g2f> g)
            {
                g2f o = (g2f)0;
                for (int i = 0; i<3; i++)
                {
                    o.uv = input[i].uv;
                    o.pos = input[i].pos;

                    float posValue = sin( _Frequency * _Time.y + o.pos.x * _InvWaveLengh +o.pos.y * _InvWaveLengh + o.pos.z * _InvWaveLengh) * _Magnitude;
                    float4 offset = float4(posValue,posValue,posValue,posValue);

                    o.effectUV.xy = input[i].uv.xy * _Dissolve_ST.xy + _Dissolve_ST.zw + float2 (0.0 ,_Time.y * _WaveSpeed);

                    o.pos = TransformObjectToHClip(o.pos + offset);

                    o.TtoW0 = input[i].TtoW0;  
                    o.TtoW1 = input[i].TtoW1;  
                    o.TtoW2 = input[i].TtoW2; 

                    g.Append(o);
                }

            }

            float4 frag (g2f i) : SV_Target
            {         
                float4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                float4 baseTex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);
                
                albedo = lerp (albedo, baseTex, _BlendProportion) * _Color;

                clip(albedo.a - _Cutoff);

                float4 dissolve = SAMPLE_TEXTURE2D(_Dissolve, sampler_Dissolve, i.effectUV.xy);
                
                clip(dissolve.r - _DissolveAmount);

                float dissolveColorBlend = 1 - smoothstep(0.0,_DissolveWidth, dissolve.r - _DissolveAmount);
                float3 dissolveColor= lerp(_DissolveFristColor,_DissolveSecondColor,dissolveColorBlend);

                dissolveColor = pow(dissolveColor,_DissolveColorPow);

                half3 worldPos = float3(i.TtoW0.w,i.TtoW1.w,i.TtoW2.w);
                half3 worldNormal = normalize(float3(i.TtoW0.z,i.TtoW1.z,i.TtoW2.z));

                half3 viewDir = normalize(_WorldSpaceCameraPos.xyz - worldPos);

                //half3 ambient = half3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w);
                half3 ambient = SampleSH(worldNormal) * _Ambient;

                float4 shadowCoord = TransformWorldToShadowCoord(worldNormal);
                Light mainLight = GetMainLight(shadowCoord);

                half halfLambert = saturate(dot(worldNormal, normalize(mainLight.direction)) * 0.5 + 0.5);
                half3 diffuseColor = SAMPLE_TEXTURE2D(_RampTex, sampler_RampTex, half2(halfLambert, halfLambert)).rgb;
                
                //half3 diffuse = mainLight.color * albedo * diffuseColor *  mainLight.shadowAttenuation * mainLight.distanceAttenuation;
                half3 diffuse = mainLight.color * albedo * halfLambert *  mainLight.shadowAttenuation * mainLight.distanceAttenuation;
                
                #if _AdditionalLights
                    uint addLightCount = GetAdditionalLightsCount();
                    for(uint iu = 0; iu < addLightCount; iu++) 
                    {
                        Light addLight = GetAdditionalLight(iu, worldPos);
                        half3 addLightDiffuse = addLight.color * albedo * saturate(dot(worldNormal, normalize(addLight.direction)) * 0.5 + 0.5);
                        diffuse += addLightDiffuse * addLight.shadowAttenuation * addLight.distanceAttenuation;
                    }
                #endif

                float3 bump = UnpackNormal(SAMPLE_TEXTURECUBE(_BumpMap,sampler_BumpMap, i.uv.zw));
                bump.xy = bump.xy * _BumpScale;
                bump.z = sqrt(1.0 - saturate(dot(bump.xy,bump.xy)));
                bump = normalize(half3(dot(i.TtoW0.xyz, bump), dot(i.TtoW1.xyz, bump), dot(i.TtoW2.xyz, bump)));


                half specularMask = SAMPLE_TEXTURE2D(_SpecularMask, sampler_SpecularMask, i.uv).r * _SpecularScale;
                half3 specular = mainLight.color.rgb * _Specular.rgb * pow(max(0, dot(bump, normalize(mainLight.direction + viewDir))), _Gloss) * specularMask;

                float fresnel = _FresnelScale + (1 - _FresnelScale) * pow(1 - saturate(dot(viewDir, bump)),4); 

                float3 worldReflect = reflect(-viewDir, worldNormal);
                float3 reflection = SAMPLE_TEXTURECUBE(_ReflectCubemap, sampler_ReflectCubemap, worldReflect).rgb * _ReflectColor.rgb;
                float3 reflectionColor = lerp(diffuse, reflection, fresnel) * _ReflectAmount;

                float3 worldRefract = refract(-viewDir, worldNormal, _RefractRatio);
                float3 refraction = SAMPLE_TEXTURECUBE(_RefractCubemap, sampler_RefractCubemap, worldRefract).rgb * _RefractColor.rgb;
                float3 refractionColor = lerp(diffuse, refraction, 1 - fresnel) * _RefractAmount;

                float3 lightFinalColor = ambient + specular + (refractionColor * reflectionColor);

                float3 finalColor = lerp(lightFinalColor, dissolveColor, dissolveColorBlend * step(0.0001,_DissolveAmount)) ; 

                return float4(finalColor , _Alpha);
            }
            ENDHLSL
        }
        
        Pass 
        { 
            NAME "ShadowCaster"

            Tags { "LightMode" = "ShadowCaster" }

            Cull Off
            ZWrite On
            ZTest LEqual
            ColorMask 0

            HLSLPROGRAM

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            #pragma shader_feature _ALPHATEST_ON

            CBUFFER_START(UnityPerMaterial)
                float _Frequency;
                float _Magnitude;
                float _InvWaveLengh;
                float _WaveSpeed;

                float4 _Color;
                float _Cutoff;
                float _DissolveAmount;
                
                float3 _LightDirection;

                float4 _MainTex_ST;
                float4 _Dissolve_ST;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_Dissolve);
            SAMPLER(sampler_Dissolve);

            struct a2v
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2g 
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };

            struct g2f
            {
                float2 uv : TEXCOORD0;
                float4 pos: SV_POSITION;
            };

            v2g vert(a2v v)
            {
                v2g o = (v2g)0;

                float4 offset =float4(0,0,0,0);

                offset.x = sin(_Frequency * _Time.y + v.vertex.x * _InvWaveLengh + v.vertex.y * _InvWaveLengh + v.vertex.z * _InvWaveLengh) * _Magnitude;
                offset.z = sin(_Frequency * _Time.y + v.vertex.x * _InvWaveLengh + v.vertex.y * _InvWaveLengh + v.vertex.z * _InvWaveLengh) * _Magnitude;

                v.vertex += offset;

                o.pos = v.vertex;

                o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw + float2 (0.0,_Time.y * _WaveSpeed);
                o.uv.xy = v.texcoord.xy * _Dissolve_ST.xy + _Dissolve_ST.zw + float2 (0.0,_Time.y * _WaveSpeed);

                return o;
            }

            [maxvertexcount(3)]
            void geom (triangle v2g input[3], inout TriangleStream<g2f> g)
            {
                g2f o = (g2f)0;
                for (int i = 0; i<3; i++)
                {
                    o.uv = input[i].uv;
                    o.pos = input[i].pos; //+ float4(input[i].normal,0) * _TriangleOffset;

                    float3 positionWS = TransformObjectToWorld(input[i].pos.xyz);
                    float3 normalWS = TransformObjectToWorldNormal(input[i].normal);
                    float posValue = sin( _Frequency * _Time.y + o.pos.x * _InvWaveLengh +o.pos.y * _InvWaveLengh + o.pos.z * _InvWaveLengh) * _Magnitude;
                    float4 offset = float4(posValue,posValue,posValue,posValue);

                    // 获取阴影专用裁剪空间下的坐标
                    o.pos = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection) + offset);
                    
                    // 判断是否是在DirectX平台翻转过坐标
                    #if UNITY_REVERSED_Z
                        o.pos.z = min(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
                    #else
                        o.pos.z = max(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
                    #endif
                    
                    g.Append(o);
                }
            }

            float4 frag(g2f i) : SV_Target 
            {
                float4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv.xy) * _Color;

                clip(albedo.a - _Cutoff);

                float4 dissolve = SAMPLE_TEXTURE2D(_Dissolve, sampler_Dissolve, i.uv.xy);
                
                clip(dissolve.r - _DissolveAmount);

                return 0;
            }  
            ENDHLSL
        }
    }
    FallBack "Transparent/Cutout/VertexLit"
} 