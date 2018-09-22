Shader "Hidden/PeekImageEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_StencilMask("Stencil Mask", Range(0,255)) = 1
		_Intensity ("Intensity", Range(0,1)) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _PeekMask;
			float _PeekIntensity;
			
			fixed4 posterize(fixed4 col, int lvl)
			{
				col.rgb = col.rgb * lvl;
				col.rgb = floor(col.rgb) / lvl;
				return col;
			}
			fixed4 grayScale(fixed4 col)
			{
				col.rgb = dot(col.rgb, float3(0.3, 0.59, 0.11));
				return col;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 baseCol = tex2D(_MainTex, i.uv);
				float col = tex2D(_PeekMask, i.uv).a;
				baseCol -= (0.0025 * (1 - col)) * _PeekIntensity;
				// also lerp the color into posterized based on col
				baseCol = lerp(baseCol, grayScale(baseCol), (1-col) * _PeekIntensity);
				baseCol = lerp(baseCol, baseCol * 6, col * _PeekIntensity);
				//baseCol = lerp(baseCol, posterize(baseCol, 128), ((1-col) * _PeekIntensity));
				return baseCol;
			}
			ENDCG
		}
	}
}
