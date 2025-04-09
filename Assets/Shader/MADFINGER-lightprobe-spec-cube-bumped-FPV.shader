Shader "MADFINGER/Environment/Bumped cubemap specular + Lightprobe FPV" {
	Properties {
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_NormalsTex ("Normalmap", 2D) = "bump" {}
		_SpecCubeTex ("SpecCube", CUBE) = "black" {}
		_SpecularStrength ("Specular strength", Float) = 1
		_SHLightingScale ("LightProbe influence scale", Float) = 1
	}
	SubShader { 
		LOD 100
		Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry-5" "RenderType"="Opaque" }
		Pass {
			Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry-5" "RenderType"="Opaque" }

			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _SHLightingScale;
            float4 _ProjParams;

            sampler2D _MainTex;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 color : COLOR;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float2 uv3 : TEXCOORD3;
                float2 uv4 : TEXCOORD4;
                float3 color : COLOR;
            };

            v2f vert(appdata_t v)
            {
                v2f o;

                float4 tmpvar_1;
                tmpvar_1.xyz = normalize(v.tangent.xyz);
                tmpvar_1.w = v.tangent.w;
                float4x4 projTM_2;
                float4 tmpvar_3;
                float3 tmpvar_4;
                projTM_2 = UNITY_MATRIX_P;
                projTM_2[0] = UNITY_MATRIX_P[0]; projTM_2[0].x = (UNITY_MATRIX_P[0].x * _ProjParams.x);
                projTM_2[1] = projTM_2[1]; projTM_2[1].y = (projTM_2[1].y * _ProjParams.y);
                float4 tmpvar_5;
                tmpvar_5.w = 1.0;
                tmpvar_5.xyz = UnityObjectToViewPos(v.vertex).xyz;
                float4 tmpvar_6;
                tmpvar_6 = mul(projTM_2, tmpvar_5);
                tmpvar_3.xyw = tmpvar_6.xyw;
                tmpvar_3.z = (tmpvar_6.z * _ProjParams.z);
                tmpvar_3.z = (tmpvar_3.z + (_ProjParams.w * tmpvar_6.w));
                float3x3 tmpvar_7;
                tmpvar_7[0] = unity_ObjectToWorld[0].xyz;
                tmpvar_7[1] = unity_ObjectToWorld[1].xyz;
                tmpvar_7[2] = unity_ObjectToWorld[2].xyz;
                float3 tmpvar_8;
                tmpvar_8 = normalize(mul(tmpvar_7, tmpvar_1.xyz));
                float3x3 tmpvar_9;
                tmpvar_9[0] = unity_ObjectToWorld[0].xyz;
                tmpvar_9[1] = unity_ObjectToWorld[1].xyz;
                tmpvar_9[2] = unity_ObjectToWorld[2].xyz;
                float3 tmpvar_10;
                tmpvar_10 = normalize(mul(tmpvar_9, normalize(v.normal)));
                float3 tmpvar_11;
                tmpvar_11 = normalize((((tmpvar_10.yzx * tmpvar_8.zxy) - (tmpvar_10.zxy * tmpvar_8.yzx)) * v.tangent.w));
                float3 tmpvar_12;
                tmpvar_12.x = tmpvar_8.x;
                tmpvar_12.y = tmpvar_11.x;
                tmpvar_12.z = tmpvar_10.x;
                float3 tmpvar_13;
                tmpvar_13.x = tmpvar_8.y;
                tmpvar_13.y = tmpvar_11.y;
                tmpvar_13.z = tmpvar_10.y;
                float3 tmpvar_14;
                tmpvar_14.x = tmpvar_8.z;
                tmpvar_14.y = tmpvar_11.z;
                tmpvar_14.z = tmpvar_10.z;
                float4 tmpvar_15;
                tmpvar_15.w = 1.0;
                tmpvar_15.xyz = tmpvar_10;
                float3 tmpvar_16;
                float4 normal_17;
                normal_17 = tmpvar_15;
                float vC_18;
                float3 x3_19;
                float3 x2_20;
                float3 x1_21;
                float tmpvar_22;
                tmpvar_22 = dot (unity_SHAr, normal_17);
                x1_21.x = tmpvar_22;
                float tmpvar_23;
                tmpvar_23 = dot (unity_SHAg, normal_17);
                x1_21.y = tmpvar_23;
                float tmpvar_24;
                tmpvar_24 = dot (unity_SHAb, normal_17);
                x1_21.z = tmpvar_24;
                float4 tmpvar_25;
                tmpvar_25 = (normal_17.xyzz * normal_17.yzzx);
                float tmpvar_26;
                tmpvar_26 = dot (unity_SHBr, tmpvar_25);
                x2_20.x = tmpvar_26;
                float tmpvar_27;
                tmpvar_27 = dot (unity_SHBg, tmpvar_25);
                x2_20.y = tmpvar_27;
                float tmpvar_28;
                tmpvar_28 = dot (unity_SHBb, tmpvar_25);
                x2_20.z = tmpvar_28;
                float tmpvar_29;
                tmpvar_29 = ((normal_17.x * normal_17.x) - (normal_17.y * normal_17.y));
                vC_18 = tmpvar_29;
                float3 tmpvar_30;
                tmpvar_30 = (unity_SHC.xyz * vC_18);
                x3_19 = tmpvar_30;
                tmpvar_16 = ((x1_21 + x2_20) + x3_19);
                float3 tmpvar_31;
                tmpvar_31 = (tmpvar_16 * _SHLightingScale);
                tmpvar_4 = tmpvar_31;
                o.pos = tmpvar_3;
                o.uv = v.uv.xy;
                o.uv1 = (mul(unity_ObjectToWorld, v.vertex).xyz - _WorldSpaceCameraPos);
                o.uv2 = tmpvar_12;
                o.uv3 = tmpvar_13;
                o.uv4 = tmpvar_14;
                o.color = tmpvar_4;

                return o;
            }
            half4 frag(v2f i) : SV_TARGET
            {
                float4 c_1;
                float4 tmpvar_2;
                tmpvar_2 = tex2D (_MainTex, i.uv);
                c_1.w = tmpvar_2.w;
                c_1.xyz = (tmpvar_2.xyz * i.color);
                return c_1;
            }
            ENDCG
        }
    }
}