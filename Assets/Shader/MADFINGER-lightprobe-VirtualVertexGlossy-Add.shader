Shader "MADFINGER/Environment/Lightprobes with VirtualGloss Per-Vertex Additive" {
	Properties {
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_SpecOffset ("Specular Offset from Camera", Vector) = (1,10,2,0)
		_SpecRange ("Specular Range", Float) = 20
		_SpecColor ("Specular Color", Color) = (1,1,1,1)
		_Shininess ("Shininess", Range(0.01,1)) = 0.078125
		_SHLightingScale ("LightProbe influence scale", Float) = 1
	}
	SubShader { 
		LOD 100
		Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
		Pass {
			Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }

			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float3 _SpecOffset;
            float _SpecRange;
            float3 _SpecColor;
            float _Shininess;
            float _SHLightingScale;

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
                float3 uv3 : TEXCOORD3;
                float3 uv4 : TEXCOORD4;
            };

            v2f vert(appdata_t v)
            {
                v2f o;

                float3 tmpvar_1;
                tmpvar_1 = normalize(v.normal);
                float3 tmpvar_2;
                float3 tmpvar_3;
                float3 tmpvar_4;
                float3x3 tmpvar_5;
                tmpvar_5[0] = unity_ObjectToWorld[0].xyz;
                tmpvar_5[1] = unity_ObjectToWorld[1].xyz;
                tmpvar_5[2] = unity_ObjectToWorld[2].xyz;
                float3x3 tmpvar_6;
                tmpvar_6[0] = UNITY_MATRIX_MV[0].xyz;
                tmpvar_6[1] = UNITY_MATRIX_MV[1].xyz;
                tmpvar_6[2] = UNITY_MATRIX_MV[2].xyz;
                float3 tmpvar_7;
                tmpvar_7 = (mul(UNITY_MATRIX_MV, v.vertex).xyz - (_SpecOffset * float3(1.0, 1.0, -1.0)));
                float3 tmpvar_8;
                tmpvar_8 = (((_SpecColor * pow (clamp (dot (mul(tmpvar_6, tmpvar_1), normalize(((float3(0.0, 0.0, 1.0) + normalize(-(tmpvar_7))) * 0.5))), 0.0, 1.0), (_Shininess * 128.0))) * 2.0) * (1.0 - clamp ((sqrt(dot (tmpvar_7, tmpvar_7)) / _SpecRange), 0.0, 1.0)));
                tmpvar_3 = tmpvar_8;
                float4 tmpvar_9;
                tmpvar_9.w = 1.0;
                tmpvar_9.xyz = mul(tmpvar_5, tmpvar_1);
                float3 tmpvar_10;
                float4 normal_11;
                normal_11 = tmpvar_9;
                float vC_12;
                float3 x3_13;
                float3 x2_14;
                float3 x1_15;
                float tmpvar_16;
                tmpvar_16 = dot (unity_SHAr, normal_11);
                x1_15.x = tmpvar_16;
                float tmpvar_17;
                tmpvar_17 = dot (unity_SHAg, normal_11);
                x1_15.y = tmpvar_17;
                float tmpvar_18;
                tmpvar_18 = dot (unity_SHAb, normal_11);
                x1_15.z = tmpvar_18;
                float4 tmpvar_19;
                tmpvar_19 = (normal_11.xyzz * normal_11.yzzx);
                float tmpvar_20;
                tmpvar_20 = dot (unity_SHBr, tmpvar_19);
                x2_14.x = tmpvar_20;
                float tmpvar_21;
                tmpvar_21 = dot (unity_SHBg, tmpvar_19);
                x2_14.y = tmpvar_21;
                float tmpvar_22;
                tmpvar_22 = dot (unity_SHBb, tmpvar_19);
                x2_14.z = tmpvar_22;
                float tmpvar_23;
                tmpvar_23 = ((normal_11.x * normal_11.x) - (normal_11.y * normal_11.y));
                vC_12 = tmpvar_23;
                float3 tmpvar_24;
                tmpvar_24 = (unity_SHC.xyz * vC_12);
                x3_13 = tmpvar_24;
                tmpvar_10 = ((x1_15 + x2_14) + x3_13);
                float3 tmpvar_25;
                tmpvar_25 = (tmpvar_10 * _SHLightingScale);
                tmpvar_4 = tmpvar_25;
                o.pos = (UnityObjectToClipPos(v.vertex));
                o.uv = v.uv.xy;
                o.uv1 = tmpvar_2;
                o.uv3 = tmpvar_3;
                o.uv4 = tmpvar_4;

                return o;
            }
            half4 frag(v2f i) : SV_TARGET
            {
                float4 c_1;
                float4 tmpvar_2;
                tmpvar_2 = tex2D (_MainTex, i.uv);
                c_1.w = tmpvar_2.w;
                c_1.xyz = (tmpvar_2.xyz * i.uv4);
                c_1.xyz = (c_1.xyz + (i.uv3 * tmpvar_2.w));
                return c_1;
            }
            ENDCG
        }
    }
}