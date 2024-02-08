// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// PJ 13/09/2017

Shader "Hidden/VisSim/myInpainter"
{
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	CGINCLUDE

	#include "UnityCG.cginc"

	uniform sampler2D _MainTex;
	uniform float4 _MainTex_TexelSize;
	half4 _MainTex_ST;
	sampler2D _OffsetTextureX;
	sampler2D _OffsetTextureY;
	sampler2D _BlurTexture;
	half _MouseX, _MouseY;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	v2f vert(appdata_img v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = half4(v.texcoord.xy, 1, 1);
		return o;
	}
	
	float4 frag_inpaint(v2f i) : SV_Target
	{
		//float2 degcoords = i.uv; // !!!not mouse contingent

		// make texture coordinate mouse-contingent
		// correct mouse y-coordinates if on a platform where '0' is the top (e.g., Direct3D-like)
		# if UNITY_UV_STARTS_AT_TOP
			_MouseY = 1.0 - _MouseY;
		# endif
		float2 mouseOffset = (half2(0.5, 0.5) - half2(_MouseX, _MouseY)); // map 0-1 (min/max of screen) to -.5 to +.5
		float2 degcoords = i.uv - mouseOffset;
		degcoords = degcoords*.5 + .25; // map 0-1 to 0.25-0.75 (i.e., since deg tex is twice as big) [this has to be a hack! must be a better way of doing this...)

										// is this necessary???
		//degcoords = UnityStereoScreenSpaceUVAdjust(degcoords, _Overlay_ST);

		float4 offsetX = tex2D(_OffsetTextureX, degcoords);
		float4 offsetY = tex2D(_OffsetTextureY, degcoords);

		float w = (offsetX.x + offsetX.y + offsetX.z) / 3.0; // would be better to also use the alpha channel too, and get another 8-bits of precision...
		float xOffset = (2 * w) - 1; // rescale(0 to 1) -> (-1 to 1)
		xOffset = -xOffset;

		w = (offsetY.x + offsetY.y + offsetY.z) / 3.0;
		float yOffset = (2 * w) - 1; // rescale(0 to 1) -> (-1 to 1)
		yOffset = yOffset;

		// HACKS - suggestive of some kind of rounding error somewhere in the code?
		if (abs(xOffset) < 0.01) {
			xOffset = 0;
		}
		if (abs(yOffset) < 0.01) {
			yOffset = 0;
		}

		// offset
		i.uv.x += xOffset;
		i.uv.y += yOffset;

		// return
		return tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv, _MainTex_ST));
	}
		
	float4 frag_blurH(v2f i) : SV_Target
	{
		// make texture coordinate mouse-contingent
		float2 mouseOffset = (half2(0.5, 0.5) - half2(_MouseX, _MouseY)); // map 0-1 (min/max of screen) to -.5 to +.5
		float2 degcoords = i.uv - mouseOffset;
		degcoords = degcoords*.5 + .25; // map 0-1 to 0.25-0.75 (i.e., since deg tex is twice as big) [this has to be a hack! must be a better way of doing this...)

		// is this necessary???
		//degcoords = UnityStereoScreenSpaceUVAdjust(degcoords, _Overlay_ST);

		float4 warpEdges = tex2D(_BlurTexture, degcoords);

		float w = (warpEdges.x + warpEdges.y + warpEdges.z) / 3.0; // would be better to also use the alpha channel too, and get another 8-bits of precision...

		if (w > 0.5) {
			// !!! FIX ME!!! RANDOM UNITS!!!
			half4 color = 0;
			half4 tap = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv + float2(-3*0.005, 0), _MainTex_ST));
			color += tap * half4(0.0044, 0.0044, 0.0044, 1);
			tap = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv + float2(-2 * 0.005, 0), _MainTex_ST));
			color += tap * half4(0.0540, 0.0540, 0.0540, 1);
			tap = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv + float2(-1 * 0.005, 0), _MainTex_ST));
			color += tap * half4(0.2420, 0.2420, 0.2420, 1);
			tap = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv + float2(0, 0), _MainTex_ST));
			color += tap * half4(0.3989, 0.3989, 0.3989, 1);
			tap = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv + float2(1 * 0.005, 0), _MainTex_ST));
			color += tap * half4(0.2420, 0.2420, 0.2420, 1);
			tap = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv + float2(2 * 0.005, 0), _MainTex_ST));
			color += tap * half4(0.0540, 0.0540, 0.0540, 1);
			tap = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv + float2(3 * 0.005, 0), _MainTex_ST));
			color += tap * half4(0.0044, 0.0044, 0.0044, 1);

			return color;
		}
		else {
			return tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv, _MainTex_ST));
		}
	}

		float4 frag_blurV(v2f i) : SV_Target
	{
		// make texture coordinate mouse-contingent
		float2 mouseOffset = (half2(0.5, 0.5) - half2(_MouseX, _MouseY)); // map 0-1 (min/max of screen) to -.5 to +.5
		float2 degcoords = i.uv - mouseOffset;
		degcoords = degcoords*.5 + .25; // map 0-1 to 0.25-0.75 (i.e., since deg tex is twice as big) [this has to be a hack! must be a better way of doing this...)

		// is this necessary???
		//degcoords = UnityStereoScreenSpaceUVAdjust(degcoords, _Overlay_ST);

		float4 warpEdges = tex2D(_BlurTexture, degcoords);

		float w = (warpEdges.x + warpEdges.y + warpEdges.z) / 3.0; // would be better to also use the alpha channel too, and get another 8-bits of precision...

		if (w > 0.5) {
			// !!! FIX ME!!! RANDOM UNITS!!!
			half4 color = 0;
			half4 tap = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv + float2(0, -0.015), _MainTex_ST));
			color += tap * half4(0.0044, 0.0044, 0.0044, 1);
			tap = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv + float2(0, -0.010), _MainTex_ST));
			color += tap * half4(0.0540, 0.0540, 0.0540, 1);
			tap = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv + float2(0, -0.005), _MainTex_ST));
			color += tap * half4(0.2420, 0.2420, 0.2420, 1);
			tap = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv, _MainTex_ST));
			color += tap * half4(0.3989, 0.3989, 0.3989, 1);
			tap = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv + float2(0, 0.005), _MainTex_ST));
			color += tap * half4(0.2420, 0.2420, 0.2420, 1);
			tap = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv + float2(0, 0.010), _MainTex_ST));
			color += tap * half4(0.0540, 0.0540, 0.0540, 1);
			tap = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv + float2(0, 0.015), _MainTex_ST));
			color += tap * half4(0.0044, 0.0044, 0.0044, 1);

			return color;
		}
		else {
			return tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv, _MainTex_ST));
		}
	}
	
	ENDCG


	SubShader {

		ZTest Always Cull Off ZWrite Off

		// 0
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag_inpaint
			ENDCG
		}
		
		// 1
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag_blurH
			ENDCG
		}

		// 2
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag_blurV
			ENDCG
		}
		
	}

	Fallback off
}