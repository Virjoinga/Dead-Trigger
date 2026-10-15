// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "MADFINGER/Environment/Lightmap + emissivity" 
{
    Properties
    {
        _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
        _Emissivity ("Emissivity scaler", Range(0,4)) = 1
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
            sampler2D _MainTex;
            float _Emissivity;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
            };
            struct v2f
            {
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float4 vertex : POSITION;
            };
            v2f vert(appdata_t v)
            {
                v2f o;
                  o.vertex = (UnityObjectToClipPos(v.vertex));
                  o.texcoord0 = v.texcoord0.xy;
                  o.texcoord1 = ((v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                  float4 c_1;
                  float4 tmpvar_2;
                  tmpvar_2 = tex2D (_MainTex, i.texcoord0);
                  c_1.w = tmpvar_2.w;
                  float3 tmpvar_3;
                  tmpvar_3 = (2.0 * UNITY_SAMPLE_TEX2D (unity_Lightmap, i.texcoord1).xyz);
                  float3 tmpvar_4;
                  tmpvar_4 = (tmpvar_2.xyz * (tmpvar_3 + (tmpvar_2.www * _Emissivity)));
                  c_1.xyz = tmpvar_4;
                  return c_1;
            }
            ENDCG
        }
    }
    
}