Shader "AR/ShadowReceiver-Multiply-Texture"
{
	Properties
	{
		_Color("Shadow Color", Color) = (0,0,0,1)
		_MainTex("Main Texture", 2D) = "white"
		_CullMode("Cull Mode (Off=0, Front=1, Back=2)", Float) = 2.0
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry"}
		LOD 100

		Cull[_CullMode]
		Blend Zero SrcColor


		Pass
		{
			Tags {"LightMode" = "UniversalForward"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos: SV_POSITION;
				float2 uv : TEXCOORD0;
				SHADOW_COORDS(1)
			};

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				TRANSFER_SHADOW(o)
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float shadow = SHADOW_ATTENUATION(i);
				float4 col = tex2D(_MainTex, i.uv);
				float4 shadowColor = lerp(_Color, fixed4(1, 1, 1, 1),  shadow);
				return col * shadowColor;
			}
			ENDCG
		}
	}
	Fallback "VertexLit"
}
