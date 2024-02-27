Shader "Custom/HandHighlightForObjects"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _GrabbableObjectPos("GrabbableObjectPos", Vector) = (0.0, 0.0, 0.0, 0.0)
        _HighlightHand("HighlightHand", float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;



        uniform float4 _GrabbableObjectPos;
        uniform float _HighlightHand;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

            float distanceToObject = pow(1.02 - distance(_GrabbableObjectPos.xyz, IN.worldPos), 30.0);
            if (distanceToObject > 0.0f)
            {
                distanceToObject += 0.001f;
            }
            if (_HighlightHand != 0.0f)
            {
                float3 directionToObject = float3(distanceToObject, distanceToObject, 0);// distanceToObject, distanceToObject);
                //c.rgb += 0.5f;
                o.Albedo.rgb += directionToObject;
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}