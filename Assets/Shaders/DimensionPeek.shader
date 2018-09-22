Shader "Hidden/DimensionPeek"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DimensionB ("Dimension B RenderTexture", 2D) = "clear" {}
		_DimensionCut ("Dimension CutFX RenderTexture", 2D) = "clear" {}
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
			sampler2D _DimensionB;
			sampler2D _DimensionCut;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 bCol = tex2D(_DimensionB, i.uv);
				fixed4 cutMask = tex2D(_DimensionCut, i.uv);
				
				// cutmask only determines what we draw out of bCol.
				bCol *= 1 - cutMask.a;
				
				if(bCol.a != 0)
					return bCol;
				
				return col;
			}
			ENDCG
		}
	}
}