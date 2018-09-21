Shader "Custom/PeekQuad" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_StencilMask("Stencil Mask", Range(0, 255)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry"}
		LOD 200
		ZTest Less
		//ColorMask 0
		Stencil
		{
			Ref [_StencilMask]
			Comp Never
			Fail Replace
		}
		Pass{}
	}
   
}