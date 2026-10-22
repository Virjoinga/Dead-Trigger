Shader "MADFINGER/Environment/Bumped cubemap specular + Lightprobe FPV"
{
    Properties
    {
        _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
        _NormalsTex ("Normalmap", 2D) = "bump" {}
        _SpecCubeTex ("SpecCube", CUBE) = "black" {}
        _SpecularStrength ("Specular strength", Float) = 1
        _SHLightingScale ("LightProbe influence scale", Float) = 1
        _FPVDepthScale ("FPV depth range", Range(0.001, 1)) = 0.02
    }

    SubShader
    {
        LOD 100
        Tags { "LightMode"="ForwardBase" "Queue"="Overlay" "RenderType"="Opaque" }

        Pass
        {
            ZTest LEqual
            ZWrite On
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _SHLightingScale;
            float4 _ProjParams;
            float _FPVDepthScale;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord0 : TEXCOORD0;
                float3 color : COLOR0;
            };

            v2f vert(appdata_t v)
            {
                v2f o;

                // Retain the custom FPV projection when supplied by the game.
                float4 projectionParams = float4(1.0, 1.0, 1.0, 0.0);
                if (dot(abs(_ProjParams), float4(1.0, 1.0, 1.0, 1.0)) > 0.0001)
                    projectionParams = _ProjParams;

                float4x4 projectionMatrix = UNITY_MATRIX_P;
                projectionMatrix[0][0] *= projectionParams.x;
                projectionMatrix[1][1] *= projectionParams.y;

                float3 viewPosition = mul(UNITY_MATRIX_MV, v.vertex).xyz;
                float4 clipPosition = mul(projectionMatrix, float4(viewPosition, 1.0));
                clipPosition.z = clipPosition.z * projectionParams.z + clipPosition.w * projectionParams.w;

                // Render FPV geometry in front of the level, keeping its own depth test.
                clipPosition.z = lerp(UNITY_NEAR_CLIP_VALUE * clipPosition.w, clipPosition.z, _FPVDepthScale);
                o.vertex = clipPosition;

                // Match the linear SH evaluation used by the original GLES shader.
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float4 normal = float4(worldNormal, 1.0);
                float3 lighting = SHEvalLinearL0L1(normal) + SHEvalLinearL2(normal);
                o.color = lighting * _SHLightingScale;

                o.texcoord0 = v.texcoord0;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 baseColor = tex2D(_MainTex, i.texcoord0);
                return float4(baseColor.rgb * i.color, baseColor.a);
            }
            ENDCG
        }
    }
}
