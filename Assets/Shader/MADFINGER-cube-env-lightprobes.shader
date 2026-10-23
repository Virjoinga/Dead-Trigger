// Upgrade NOTE: commented out 'float4x4 _Object2World', a built-in variable
// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'

Shader "MADFINGER/Environment/Cube env map (Supports LightProbes)"
{
    Properties
    {
        _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
        _EnvTex ("Cube env tex", CUBE) = "black" {}
        _SHLightingScale ("LightProbe influence scale", Float) = 1
        _EnvStrength ("Env strength weights", Vector) = (0,0,0,2)
        _UVScrollSpeed ("UV scroll speed XY", Vector) = (0,0,0,0)
    }
    SubShader
    {
        LOD 100
        Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
        Pass
        {
            Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // Preserve the original detail paths; no keyword defaults to full detail.
            #pragma multi_compile __ UNITY_SHADER_DETAIL_LOW UNITY_SHADER_DETAIL_MEDIUM UNITY_SHADER_DETAIL_HIGH UNITY_SHADER_DETAIL_VERY_HIGH
            #include "UnityCG.cginc"
            // float4x4 _Object2World;
            float4 _MainTex_ST;
            float _SHLightingScale;
            float4 _UVScrollSpeed;
            sampler2D _MainTex;
            samplerCUBE _EnvTex;
            float4 _EnvStrength;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float3 normal : NORMAL;
                float4 texcoord0 : TEXCOORD0;
            };
            struct v2f
            {
                float2 texcoord0 : TEXCOORD0;
                float3 texcoord1 : TEXCOORD1;
                float3 color : COLOR;
                float4 vertex : POSITION;
            };
            v2f vert(appdata_t v)
            {
                v2f o;
                  float3 tmpvar_1;
                  float3x3 tmpvar_2;
                  tmpvar_2[0] = unity_ObjectToWorld[0].xyz;
                  tmpvar_2[1] = unity_ObjectToWorld[1].xyz;
                  tmpvar_2[2] = unity_ObjectToWorld[2].xyz;
                  float3 tmpvar_3;
                  tmpvar_3 = mul(tmpvar_2, normalize(v.normal));
                  float3 tmpvar_4;
                  float3 i_5;
                  i_5 = (mul(unity_ObjectToWorld, v.vertex).xyz - _WorldSpaceCameraPos);
                  tmpvar_4 = (i_5 - (2.0 * (dot (tmpvar_3, i_5) * tmpvar_3)));
                  tmpvar_1.yz = tmpvar_4.yz;
                  tmpvar_1.x = -(tmpvar_4.x);
                  float4 tmpvar_6;
                  tmpvar_6.w = 1.0;
                  tmpvar_6.xyz = tmpvar_3;
                  float3 tmpvar_7;
                  float4 normal_8;
                  normal_8 = tmpvar_6;
                  float vC_9;
                  float3 x3_10;
                  float3 x2_11;
                  float3 x1_12;
                  float tmpvar_13;
                  tmpvar_13 = dot (unity_SHAr, normal_8);
                  x1_12.x = tmpvar_13;
                  float tmpvar_14;
                  tmpvar_14 = dot (unity_SHAg, normal_8);
                  x1_12.y = tmpvar_14;
                  float tmpvar_15;
                  tmpvar_15 = dot (unity_SHAb, normal_8);
                  x1_12.z = tmpvar_15;
                  float4 tmpvar_16;
                  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
                  float tmpvar_17;
                  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
                  x2_11.x = tmpvar_17;
                  float tmpvar_18;
                  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
                  x2_11.y = tmpvar_18;
                  float tmpvar_19;
                  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
                  x2_11.z = tmpvar_19;
                  float tmpvar_20;
                  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
                  vC_9 = tmpvar_20;
                  float3 tmpvar_21;
                  tmpvar_21 = (unity_SHC.xyz * vC_9);
                  x3_10 = tmpvar_21;
                  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
                  // Unity 2017 supplies linear SH lighting even in Gamma projects.
                  // Match ShadeSH9's output space before the original material/AO tint.
                  #if defined(UNITY_COLORSPACE_GAMMA)
                  tmpvar_7 = LinearToGammaSpace(tmpvar_7);
                  #endif
                  o.vertex = (UnityObjectToClipPos(v.vertex));
                  o.texcoord0 = (((v.texcoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + frac((_UVScrollSpeed.xy * _Time.xy)));
                  o.texcoord1 = tmpvar_1;
                  o.color = ((tmpvar_7 * _SHLightingScale) * v.color.xyz);
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                  float4 c_1;
                  float4 tmpvar_2;
                  tmpvar_2 = tex2D (_MainTex, i.texcoord0);
                  c_1.w = tmpvar_2.w;
                  float3 tmpvar_3;
                  tmpvar_3 = (tmpvar_2.xyz * i.color);
                  c_1.xyz = tmpvar_3;
                  #if !defined(UNITY_SHADER_DETAIL_LOW)
                  c_1.xyz = (tmpvar_2.xyz + texCUBE(_EnvTex, i.texcoord1).rgb * dot(tmpvar_2, _EnvStrength)) * i.color;
                  #endif
                  return c_1;
            }
            ENDCG
        }
    }

}
