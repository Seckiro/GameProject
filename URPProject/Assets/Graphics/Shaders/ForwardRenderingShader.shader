Shader "Unlit/ForwardRenderingShader"
{
	Properties 
	{
		_Diffuse ("Diffuse", Color) = (1, 1, 1, 1)
		_Specular ("Specular", Color) = (1, 1, 1, 1)
		_Gloss ("Gloss", float) = 20
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		
		Pass 
		{
			Name "ForwardRenderingShaderBase"

			Tags { "LightMode"="UniversalForward" }
			
			HLSLPROGRAM

			#pragma multi_compile_fwdbase	
			
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"

			CBUFFER_START(UnityPerMaterial)
				float _Gloss;
				float4 _Diffuse;
				float4 _Specular;
			CBUFFER_END

			struct a2v 
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};
			
			struct v2f 
			{
				float4 pos : SV_POSITION;
				float3 worldNormal : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
			};
			
			v2f vert(a2v v) 
			{
				v2f o;
				o.pos = TransformObjectToHClip(v.vertex.xyz);
				
				o.worldNormal = TransformObjectToWorldNormal(v.normal);
				
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				
				return o;
			}
			
			float4 frag(v2f i) : SV_Target 
			{
				float3 worldNormal = normalize(i.worldNormal);
				float3 worldLightDir = normalize(_MainLightPosition.xyz);
				
				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				
				float3 diffuse = _MainLightColor.rgb * _Diffuse.rgb * max(0, dot(worldNormal, worldLightDir));

				float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
				float3 halfDir = normalize(worldLightDir + viewDir);
				float3 specular = _MainLightColor.rgb * _Specular.rgb * pow(max(0, dot(worldNormal, halfDir)), _Gloss);

				float atten = 1.0;
				
				return float4(ambient + (diffuse + specular) * atten, 1.0);
			}
			
			ENDHLSL
		}
		
		Pass 
		{
			Name "ForwardRenderingShaderAdd"
			
			Tags { "LightMode"="UniversalForward" }
			
			Blend One One
			
			HLSLPROGRAM
			
			#pragma multi_compile_fwdadd
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			
			CBUFFER_START(UnityPerMaterial)
				float _Gloss;
				float4 _Diffuse;
				float4 _Specular;
			CBUFFER_END
			
			struct a2v 
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};
			
			struct v2f 
			{
				float4 pos : SV_POSITION;
				float3 worldNormal : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
			};
			
			v2f vert(a2v v) 
			{
				v2f o;

				o.pos = TransformObjectToHClip(v.vertex.xyz);
				
				o.worldNormal = TransformObjectToWorldNormal(v.normal);
				
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				
				return o;
			}
			
			float4 frag(v2f i) : SV_Target 
			{
				float3 worldNormal = normalize(i.worldNormal);
				#ifdef USING_DIRECTIONAL_LIGHT
					float3 worldLightDir = normalize(_MainLightPosition.xyz);
				#else
					float3 worldLightDir = normalize(_MainLightPosition.xyz - i.worldPos.xyz);
				#endif
				
				float3 diffuse = _MainLightColor.rgb * _Diffuse.rgb * max(0, dot(worldNormal, worldLightDir));
				
				float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
				float3 halfDir = normalize(worldLightDir + viewDir);
				float3 specular = _MainLightColor.rgb * _Specular.rgb * pow(max(0, dot(worldNormal, halfDir)), _Gloss);
				
				#ifdef USING_DIRECTIONAL_LIGHT
					float atten = 1.0;
				#else
					/*#if defined (POINT)
					float3 lightCoord = mul(unity_WorldToLight, float4(i.worldPos, 1)).xyz;
					float atten = tex2D(_LightTexture0, dot(lightCoord, lightCoord).rr).UNITY_ATTEN_CHANNEL;
				#elif defined (SPOT)
					float4 lightCoord = mul(unity_WorldToLight, float4(i.worldPos, 1));
					float atten = (lightCoord.z > 0) * tex2D(_LightTexture0, lightCoord.xy / lightCoord.w + 0.5).w * tex2D(_LightTextureB0, dot(lightCoord, lightCoord).rr).UNITY_ATTEN_CHANNEL;
				#else*/
					float atten = 1.0;
					//#endif
				#endif

				return float4((diffuse + specular) * atten, 1.0);
			}
			ENDHLSL   
		}
	}
	Fallback "Diffuse" 
}
