// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "MADFINGER/Environment/Unlit (Supports Lightmap) Transparent" 
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("HACK: temporary to fix lightmap bouncing light (will be fixed in RC1)", Color) = (1,1,1,1)
    }
    SubShader
    {
        LOD 100
        Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
        Pass
        {
            Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex;
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
                  float4 tmpvar_1;
                  float4 c_2;
                  float4 tmpvar_3;
                  tmpvar_3 = tex2D (_MainTex, i.texcoord0);
                  c_2 = tmpvar_3;
                  float3 tmpvar_4;
                  tmpvar_4 = (2.0 * UNITY_SAMPLE_TEX2D (unity_Lightmap, i.texcoord1).xyz);
                  c_2.xyz = (c_2.xyz * tmpvar_4);
                  tmpvar_1 = c_2;
                  return tmpvar_1;
            }
            ENDCG
        }
    }
    
}