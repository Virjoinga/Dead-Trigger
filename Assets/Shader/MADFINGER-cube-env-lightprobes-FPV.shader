Shader "MADFINGER/Environment/Cube env map (Supports LightProbes) FPV"
{
    Properties
    {
        _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
        _EnvTex ("Cube env tex", CUBE) = "black" {}
        _SHLightingScale ("LightProbe influence scale", Float) = 1
        _EnvStrength ("Env strength weights", Vector) = (0,0,0,2)
        _Params ("x - FPV proj", Vector) = (1,0,0,0)
        _UVScrollSpeed ("UV scroll speed XY", Vector) = (0,0,0,0)
        _FPVDepthScale ("FPV depth range", Range(0.001,1)) = 0.02
    }

    SubShader
    {
        LOD 100
        Tags { "LightMode"="ForwardBase" "Queue"="Overlay" "RenderType"="Opaque" }

        Pass
        {
            // Render after the level, but preserve proper depth testing within the gun.
            ZTest LEqual
            ZWrite On
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile UNITY_SHADER_DETAIL_LOW UNITY_SHADER_DETAIL_MEDIUM UNITY_SHADER_DETAIL_HIGH UNITY_SHADER_DETAIL_VERY_HIGH
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            samplerCUBE _EnvTex;
            float4 _MainTex_ST;
            float _SHLightingScale;
            float4 _EnvStrength;
            float4 _Params;
            float4 _ProjParams;
            float4 _UVScrollSpeed;
            float _FPVDepthScale;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord0 : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord0 : TEXCOORD0;
                float3 texcoord1 : TEXCOORD1;
                float3 color : COLOR0;
            };

            v2f vert(appdata_t v)
            {
                v2f o;

                // Reproduce the original first-person projection when _ProjParams
                // is supplied by game code; otherwise use an ordinary projection.
                float4 projectionParams = float4(1.0, 1.0, 1.0, 0.0);
                if (_Params.x > 0.0 && dot(abs(_ProjParams), float4(1.0, 1.0, 1.0, 1.0)) > 0.0001)
                    projectionParams = _ProjParams;

                float4x4 projectionMatrix = UNITY_MATRIX_P;
                projectionMatrix[0][0] *= projectionParams.x;
                projectionMatrix[1][1] *= projectionParams.y;

                float3 viewPosition = mul(UNITY_MATRIX_MV, v.vertex).xyz;
                float4 clipPosition = mul(projectionMatrix, float4(viewPosition, 1.0));
                clipPosition.z = clipPosition.z * projectionParams.z + clipPosition.w * projectionParams.w;

                // Map the gun's depth into a thin region near the camera.
                // UNITY_NEAR_CLIP_VALUE handles OpenGL and Direct3D clip conventions.
                // Unlike ZTest Always, this keeps front/rear surfaces properly occluded.
                clipPosition.z = lerp(UNITY_NEAR_CLIP_VALUE * clipPosition.w, clipPosition.z, _FPVDepthScale);
                o.vertex = clipPosition;

                // Preserve the original GLES normal and reflection math.
                float3 worldNormal = mul((float3x3)unity_ObjectToWorld, normalize(v.normal));
                float3 incident = mul(unity_ObjectToWorld, v.vertex).xyz - _WorldSpaceCameraPos;
                float3 reflected = incident - 2.0 * dot(worldNormal, incident) * worldNormal;
                reflected.x = -reflected.x;
                o.texcoord1 = reflected;

                // GLES originally evaluates the SH polynomials in linear space.
                // Avoid ShadeSH9(), which additionally gamma-converts in Gamma projects.
                float4 normal = float4(worldNormal, 1.0);
                float3 lighting = SHEvalLinearL0L1(normal) + SHEvalLinearL2(normal);
                o.color = lighting * _SHLightingScale;

                o.texcoord0 = v.texcoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw + frac(_UVScrollSpeed.xy * _Time.xy);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 baseColor = tex2D(_MainTex, i.texcoord0);

                #if defined(UNITY_SHADER_DETAIL_LOW)
                    return float4(baseColor.rgb * i.color, baseColor.a);
                #else
                    float4 envColor = texCUBE(_EnvTex, i.texcoord1);
                    float3 litColor = (baseColor.rgb + envColor.rgb * dot(baseColor, _EnvStrength)) * i.color;
                    return float4(litColor, baseColor.a);
                #endif
            }
            ENDCG
        }
    }
}
