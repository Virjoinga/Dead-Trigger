// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'glstate_matrix_modelview0' with 'UNITY_MATRIX_MV'
// Upgrade NOTE: replaced 'glstate_matrix_projection' with 'UNITY_MATRIX_P'

Shader "MADFINGER/Environment/Cube env map (Supports LightProbes) FPV" {
    Properties {
        _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
        _EnvTex ("Cube env tex", CUBE) = "black" {}
        _SHLightingScale ("LightProbe influence scale", Float) = 1
        _EnvStrength ("Env strength weights", Vector) = (0,0,0,2)
        _Params ("x - FPV proj", Vector) = (1,0,0,0)
        _UVScrollSpeed ("UV scroll speed XY", Vector) = (0,0,0,0)
    }
    SubShader { 
        LOD 100
        Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry-5" "RenderType"="Opaque" }
        Pass {
            Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry-5" "RenderType"="Opaque" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 _MainTex_ST;
            float _SHLightingScale;
            float4 _ProjParams;
            float4 _Params;
            float4 _UVScrollSpeed;

            sampler2D _MainTex;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float3 color : COLOR;
            };

            v2f vert(appdata_t v)
            {
                v2f o;

                float3 tmpvar_1;
                tmpvar_1 = normalize(v.normal);
                float4x4 projTM_2;
                float4 tmpvar_3;
                float3 tmpvar_4;
                float3 tmpvar_5;
                tmpvar_5 = mul(UNITY_MATRIX_MV, v.vertex).xyz;
                projTM_2 = UNITY_MATRIX_P;
                float4 tmpvar_6;
                if ((_Params.x > 0.0)) {
                    tmpvar_6 = _ProjParams;
                } else {
                    tmpvar_6 = float4(1.0, 1.0, 1.0, 0.0);
                };
                projTM_2[0] = UNITY_MATRIX_P[0]; projTM_2[0].x = (UNITY_MATRIX_P[0].x * tmpvar_6.x);
                projTM_2[1] = projTM_2[1]; projTM_2[1].y = (projTM_2[1].y * tmpvar_6.y);
                float4 tmpvar_7;
                tmpvar_7.w = 1.0;
                tmpvar_7.xyz = tmpvar_5;
                float4 tmpvar_8;
                tmpvar_8 = mul(projTM_2, tmpvar_7);
                tmpvar_3.xyw = tmpvar_8.xyw;
                tmpvar_3.z = (tmpvar_8.z * tmpvar_6.z);
                tmpvar_3.z = (tmpvar_3.z + (tmpvar_6.w * tmpvar_8.w));
                float3x3 tmpvar_9;
                tmpvar_9[0] = unity_ObjectToWorld[0].xyz;
                tmpvar_9[1] = unity_ObjectToWorld[1].xyz;
                tmpvar_9[2] = unity_ObjectToWorld[2].xyz;
                float3 tmpvar_10;
                tmpvar_10 = mul(tmpvar_9, tmpvar_1);
                float3 tmpvar_11;
                float3 i_12;
                i_12 = (mul(unity_ObjectToWorld, v.vertex).xyz - _WorldSpaceCameraPos);
                tmpvar_11 = (i_12 - (2.0 * (dot (tmpvar_10, i_12) * tmpvar_10)));
                tmpvar_4.yz = tmpvar_11.yz;
                tmpvar_4.x = -(tmpvar_11.x);
                float4 tmpvar_13;
                tmpvar_13.w = 1.0;
                tmpvar_13.xyz = tmpvar_10;
                float3 tmpvar_14;
                float4 normal_15;
                normal_15 = tmpvar_13;
                float vC_16;
                float3 x3_17;
                float3 x2_18;
                float3 x1_19;
                float tmpvar_20;
                tmpvar_20 = dot (unity_SHAr, normal_15);
                x1_19.x = tmpvar_20;
                float tmpvar_21;
                tmpvar_21 = dot (unity_SHAg, normal_15);
                x1_19.y = tmpvar_21;
                float tmpvar_22;
                tmpvar_22 = dot (unity_SHAb, normal_15);
                x1_19.z = tmpvar_22;
                float4 tmpvar_23;
                tmpvar_23 = (normal_15.xyzz * normal_15.yzzx);
                float tmpvar_24;
                tmpvar_24 = dot (unity_SHBr, tmpvar_23);
                x2_18.x = tmpvar_24;
                float tmpvar_25;
                tmpvar_25 = dot (unity_SHBg, tmpvar_23);
                x2_18.y = tmpvar_25;
                float tmpvar_26;
                tmpvar_26 = dot (unity_SHBb, tmpvar_23);
                x2_18.z = tmpvar_26;
                float tmpvar_27;
                tmpvar_27 = ((normal_15.x * normal_15.x) - (normal_15.y * normal_15.y));
                vC_16 = tmpvar_27;
                float3 tmpvar_28;
                tmpvar_28 = (unity_SHC.xyz * vC_16);
                x3_17 = tmpvar_28;
                tmpvar_14 = ((x1_19 + x2_18) + x3_17);
                o.pos = tmpvar_3;
                o.uv = (((v.uv.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + frac((_UVScrollSpeed.xy * _Time.xy)));
                o.uv1 = tmpvar_4;
                o.color = (tmpvar_14 * _SHLightingScale);

                return o;
            }
            half4 frag(v2f i) : SV_TARGET
            {
                float4 c_1;
                float4 tmpvar_2;
                tmpvar_2 = tex2D (_MainTex, i.uv);
                c_1.w = tmpvar_2.w;
                float3 tmpvar_3;
                tmpvar_3 = (tmpvar_2.xyz * i.color);
                c_1.xyz = tmpvar_3;
                return c_1;
            }
            ENDCG
        }
    }
}