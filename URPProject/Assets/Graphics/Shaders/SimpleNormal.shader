Shader "Unlit/NormalShader"
{
    Properties
    {
        _Color("Color",Color)=(1,1,1,1)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 _Color;

            struct a2v
            {
                float4 normal:NORMAL;
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 color:Color;
                float4 vertex : SV_POSITION;
            };

            v2f vert (a2v input)
            {
                v2f output = (v2f) 0;

                output.vertex = UnityObjectToClipPos(input.vertex);
                
                output.color = input.normal * 0.5 + fixed4(0.5,0.5,0.5,0.5);

                return output;
            }

            fixed4 frag (v2f input) : SV_Target
            {
                input.color = input.color * _Color;

                return input.color;
            }
            ENDCG
        }
    }
}
