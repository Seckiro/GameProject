Shader "Unlit/TransparentShadowsShader"
{
    Properties 
    {
        [MainTex]_MainTex ("Main Tex", 2D) = "white" {}
        [MainColor]_Color ("Main Color", Color) = (1, 1, 1, 1)
        _BaseMap ("Base Tex", 2D) = "white" {}
        _BlendProportion("Blend Proportion", Range(-10, 10)) = 1
        
        [space]
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpScale("Bump Scale",  Range(-5,5)) = 1.0
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
            NAME "UniversalForwardBase"

            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha

            Cull off
            //ColorMask g 2
            //Conservative True //打开看三角网格
            //Offset 1, 1
            //ZTest Always
            //ZClip False
            ZWrite On

            HLSLPROGRAM

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
            
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_fwdbase	

            CBUFFER_START(UnityPerMaterial)
                float _Gloss;
                float _Alpha;
                float _Cutoff;
                float _BumpScale;
                float _BlendProportion;
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
                float4 _BaseMap_ST;
                float4 _BumpMap_ST;
                float4 _Dissolve_ST;
                
                float4 _ReflectCubemap_ST;
                float4 _ReflratCubemap_ST;

                sampler2D _MainTex;
                sampler2D _BaseMap;
                sampler2D _BumpMap;
                sampler2D _Dissolve;

                samplerCUBE _ReflectCubemap;
                samplerCUBE _RefractCubemap;
            CBUFFER_END

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
                v2f o = (v2f)0;

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
                fixed4 albedo = tex2D(_MainTex, i.uv.xy);
                fixed4 baseTex = tex2D(_BaseMap, i.uv.xy);

                albedo = lerp (albedo, baseTex, _BlendProportion) * _Color;

                clip(albedo.a - _Cutoff);

                fixed4 dissolve = tex2D(_Dissolve, i.effectUV.xy);
                
                clip(dissolve.r - _DissolveAmount);

                float3 worldPos = float3(i.TtoW0.w, i.TtoW1.w, i.TtoW2.w);
                fixed3 worldNormal = normalize(float3(i.TtoW0.z, i.TtoW1.z, i.TtoW2.z));

                fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
                fixed3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));

                fixed3 worldReflect = reflect(-viewDir, worldNormal);
                fixed3 worldRefract = refract(-viewDir, worldNormal,_RefractRatio);

                fixed3 bump = UnpackNormal(tex2D(_BumpMap, i.uv.zw));
                bump.xy = bump.xy * _BumpScale;
                bump.z = sqrt(1.0 - saturate(dot(bump.xy,bump.xy)));
                bump = normalize(half3(dot(i.TtoW0.xyz, bump), dot(i.TtoW1.xyz, bump), dot(i.TtoW2.xyz, bump)));

                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
                
                fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(bump, lightDir));
                
                fixed3 halfDir = normalize(lightDir + viewDir);
                
                fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(max(0, dot(bump, halfDir)), _Gloss);
                
                fixed3 reflection = texCUBE(_ReflectCubemap,worldReflect).rgb * _ReflectColor.rgb;
                fixed3 refraction = texCUBE(_RefractCubemap,worldRefract).rgb * _RefractColor.rgb;

                fixed fresnel = _FresnelScale + (1 - _FresnelScale) * pow(1 - dot(viewDir, worldNormal),5);

                fixed range = 1 - smoothstep(0.0,_DissolveWidth, dissolve.r - _DissolveAmount);

                fixed3 dissolveColor= lerp(_DissolveFristColor,_DissolveSecondColor,range);

                fixed3  finalColor = lerp(diffuse, reflection, _ReflectAmount);

                UNITY_LIGHT_ATTENUATION(atten, i, worldPos);

                dissolveColor = pow(dissolveColor,_DissolveColorPow);
                
                finalColor = ambient + specular + lerp(finalColor, refraction, saturate(fresnel) + _RefractAmount) * atten; 

                finalColor = lerp(finalColor,dissolveColor, range * step(0.0001,_DissolveAmount)) ; 

                return fixed4(finalColor , _Alpha );
            }
            ENDHLSL
        }

        Pass 
        { 
            NAME "ShadowCaster"

            Tags { "LightMode"="ShadowCaster" }

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

            #pragma multi_compile_shadowcaster

            

            CBUFFER_START(UnityPerMaterial)
                float4 _TriangleColor;
                float _TriangleOffset;

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

            sampler2D _MainTex;
            sampler2D _Dissolve;

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

            float4 GetShadowPositionHClips(a2v v)
            {
                float3 positionWS = TransformObjectToWorld(v.vertex.xyz);
                float3 normalWS = TransformObjectToWorldNormal(v.normal);
                // 获取阴影专用裁剪空间下的坐标
                float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));
                
                // 判断是否是在DirectX平台翻转过坐标
                #if UNITY_REVERSED_Z
                    positionCS.z = min(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
                #else
                    positionCS.z = max(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
                #endif
                
                return positionCS;
            }

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

                    float3 positionWS = TransformObjectToWorld(input[i].pos.xyz);
                    float3 normalWS = TransformObjectToWorldNormal(input[i].normal);

                    o.pos = input[i].pos + float4(input[i].normal,0) * _TriangleOffset;

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
                float4 albedo = tex2D(_MainTex, i.uv.xy) * _Color;

                clip(albedo.a - _Cutoff);

                float4 dissolve = tex2D(_Dissolve, i.uv.xy);
                
                clip(dissolve.r - _DissolveAmount);

                return 0;
            }


            ENDHLSL
        }
    } 
    
    FallBack "Transparent/Cutout/VertexLit"
}
