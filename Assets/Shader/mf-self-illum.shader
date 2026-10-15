// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'

Shader "MADFINGER/Self-Illumin/Diffuse" 
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _ColorMult ("Color mult", Float) = 1
        _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
        _EmissionLM ("Emission (Lightmapper)", Float) = 0
    }
    SubShader
    {
        LOD 100
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Tags { "RenderType"="Opaque" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float4 _Color;
            float _ColorMult;
            sampler2D _MainTex;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
            };
            struct v2f
            {
                float2 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 vertex : POSITION;
            };
            v2f vert(appdata_t v)
            {
                v2f o;
                  float4 tmpvar_1;
                  float4 tmpvar_2;
                  tmpvar_2 = (_Color * _ColorMult);
                  tmpvar_1 = tmpvar_2;
                  o.vertex = (UnityObjectToClipPos(v.vertex));
                  o.texcoord0 = v.texcoord0.xy;
                  o.texcoord1 = tmpvar_1;
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                  float4 tmpvar_1;
                  tmpvar_1 = (tex2D (_MainTex, i.texcoord0) * i.texcoord1);
                  return tmpvar_1;
            }
            ENDCG
        }
    }
    
}