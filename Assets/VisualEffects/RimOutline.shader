//------------------------------------------------------------------------------ -
//MRTK - Quest
//https ://github.com/provencher/MRTK-Quest
//------------------------------------------------------------------------------ -
//
//MIT License
//
//Copyright(c) 2020 Eric Provencher
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files(the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions :
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
//------------------------------------------------------------------------------ -

Shader "Mixed Reality Toolkit/RimOutline"
{
    Properties{
        _Color("Color", Color) = (1,1,1,1)

        _RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0)
        _PressRimColor("Press Rim Color", Color) = (1,1,1,1)

        _RimPower("Rim Power", Range(0.5,8.0)) = 6.6   
        _PressRimPower("Press Rim Power", Range(0.5,8.0)) = 5.0

        _PressIntensity("Press Intensity", Range(0, 1.0)) = 1.0

        _GrabbableObjectPos("GrabbableObjectPos", Vector) = (0.0, 0.0, 0.0, 0.0)
        _HighlightHand("HighlightHand", float) = 0.0
    }

    SubShader
    {

        Tags{ "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : POSITION;
                float3 worldPos : COLOR;
                float3 normal : NORMAL;
                float3 viewDir: TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };


            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v); //Insert
                UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0)).xyz;
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = UnityWorldSpaceViewDir(mul(UNITY_MATRIX_M, v.vertex));

                return o;
            }

            uniform float4 _Color;
            uniform float4 _PressRimColor;
            uniform float4 _RimColor;

            uniform float _RimPower;
            uniform float _PressRimPower;

            uniform float _PressIntensity;

            uniform float4 _GrabbableObjectPos;
            uniform float _HighlightHand;

            fixed4 frag(v2f i) : SV_Target
            {

                half rim = 1.0 - abs(dot(i.viewDir, i.normal));
                _PressIntensity = 0.0;
                float rimPower = pow(rim, lerp(_RimPower, _PressRimPower, _PressIntensity));
                float3 rimColor = lerp(_RimColor.rgb, _PressRimColor.rgb, _PressIntensity);
                float3 emission = rimColor * (rimPower);

                float3 color = _Color.rgb;
                float distanceToObject = pow(1.02 - distance(_GrabbableObjectPos.xyz, i.worldPos), 30.0);
                if (distanceToObject > 0.0f)
                {
                    distanceToObject += 0.001f;
                }
                if (_HighlightHand == 0.0f)
                {
                    distanceToObject = 0;
                }
                float3 directionToObject = float3(distanceToObject, distanceToObject, 0);// distanceToObject, distanceToObject);
                
                float4 output = fixed4(color + emission + directionToObject, 1);
                return output;
            }
        ENDCG
        }
    }
}