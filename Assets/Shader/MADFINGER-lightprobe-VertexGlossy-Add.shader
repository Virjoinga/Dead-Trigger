// Upgrade NOTE: commented out 'float4x4 _Object2World', a built-in variable
// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'

Shader "MADFINGER/Environment/Lightprobes with Gloss Per-Vertex Additive" 
{
    Properties
    {
        _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
        _SpecDir ("Specular Direction", Vector) = (1,1,0,0)
        _SpecColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
        _Shininess ("Shininess", Range(0.01,1)) = 0.078125
        _SHLightingScale ("LightProbe influence scale", Float) = 1
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
            #include "UnityCG.cginc"
            // float4x4 _Object2World;
            float3 _SpecDir;
            float3 _SpecColor;
            float _Shininess;
            float _SHLightingScale;
            sampler2D _MainTex;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord0 : TEXCOORD0;
            };
            struct v2f
            {
                float2 texcoord0 : TEXCOORD0;
                float3 texcoord1 : TEXCOORD1;
                float3 texcoord3 : TEXCOORD3;
                float3 texcoord4 : TEXCOORD4;
                float4 vertex : POSITION;
            };
            v2f vert(appdata_t v)
            {
                v2f o;
                  float3 tmpvar_1;
                  float3 tmpvar_2;
                  float3x3 tmpvar_3;
                  tmpvar_3[0] = unity_ObjectToWorld[0].xyz;
                  tmpvar_3[1] = unity_ObjectToWorld[1].xyz;
                  tmpvar_3[2] = unity_ObjectToWorld[2].xyz;
                  float3 tmpvar_4;
                  tmpvar_4 = mul(tmpvar_3, normalize(v.normal));
                  float3 tmpvar_5;
                  tmpvar_5 = normalize((mul(unity_ObjectToWorld, v.vertex).xyz - _WorldSpaceCameraPos));
                  float3 tmpvar_6;
                  tmpvar_6 = (tmpvar_5 - (2.0 * (dot (tmpvar_4, tmpvar_5) * tmpvar_4)));
                  float3 tmpvar_7;
                  tmpvar_7 = ((_SpecColor * pow (clamp (dot (normalize(_SpecDir), tmpvar_6), 0.0, 1.0), (_Shininess * 128.0))) * 2.0);
                  tmpvar_1 = tmpvar_7;
                  float4 tmpvar_8;
                  tmpvar_8.w = 1.0;
                  tmpvar_8.xyz = tmpvar_4;
                  float3 tmpvar_9;
                  float4 normal_10;
                  normal_10 = tmpvar_8;
                  float vC_11;
                  float3 x3_12;
                  float3 x2_13;
                  float3 x1_14;
                  float tmpvar_15;
                  tmpvar_15 = dot (unity_SHAr, normal_10);
                  x1_14.x = tmpvar_15;
                  float tmpvar_16;
                  tmpvar_16 = dot (unity_SHAg, normal_10);
                  x1_14.y = tmpvar_16;
                  float tmpvar_17;
                  tmpvar_17 = dot (unity_SHAb, normal_10);
                  x1_14.z = tmpvar_17;
                  float4 tmpvar_18;
                  tmpvar_18 = (normal_10.xyzz * normal_10.yzzx);
                  float tmpvar_19;
                  tmpvar_19 = dot (unity_SHBr, tmpvar_18);
                  x2_13.x = tmpvar_19;
                  float tmpvar_20;
                  tmpvar_20 = dot (unity_SHBg, tmpvar_18);
                  x2_13.y = tmpvar_20;
                  float tmpvar_21;
                  tmpvar_21 = dot (unity_SHBb, tmpvar_18);
                  x2_13.z = tmpvar_21;
                  float tmpvar_22;
                  tmpvar_22 = ((normal_10.x * normal_10.x) - (normal_10.y * normal_10.y));
                  vC_11 = tmpvar_22;
                  float3 tmpvar_23;
                  tmpvar_23 = (unity_SHC.xyz * vC_11);
                  x3_12 = tmpvar_23;
                  tmpvar_9 = ((x1_14 + x2_13) + x3_12);
                  float3 tmpvar_24;
                  tmpvar_24 = (tmpvar_9 * _SHLightingScale);
                  tmpvar_2 = tmpvar_24;
                  o.vertex = (UnityObjectToClipPos(v.vertex));
                  o.texcoord0 = v.texcoord0.xy;
                  o.texcoord1 = tmpvar_6;
                  o.texcoord3 = tmpvar_1;
                  o.texcoord4 = tmpvar_2;
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                  float4 c_1;
                  float4 tmpvar_2;
                  tmpvar_2 = tex2D (_MainTex, i.texcoord0);
                  c_1.w = tmpvar_2.w;
                  c_1.xyz = (tmpvar_2.xyz * i.texcoord4);
                  c_1.xyz = (c_1.xyz + (i.texcoord3 * tmpvar_2.w));
                  return c_1;
            }
            ENDCG
        }
    }
    
}