// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "MADFINGER/Environment/Dual diffuse with lightmap" 
{
    Properties
    {
        _MainTex0 ("Base layer0 (RGB)", 2D) = "white" {}
        _MainTex1 ("Base layer1 (RGB)", 2D) = "white" {}
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
            float4 _MainTex0_ST;
            float4 _MainTex1_ST;
            sampler2D _MainTex0;
            sampler2D _MainTex1;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float4 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
            };
            struct v2f
            {
                float4 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float4 color : COLOR;
                float4 vertex : POSITION;
            };
            v2f vert(appdata_t v)
            {
                v2f o;
                  float4 tmpvar_1;
                  tmpvar_1.xy = ((v.texcoord0.xy * _MainTex0_ST.xy) + _MainTex0_ST.zw);
                  tmpvar_1.zw = ((v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
                  o.vertex = (UnityObjectToClipPos(v.vertex));
                  o.texcoord0 = tmpvar_1;
                  o.texcoord1 = ((v.texcoord0.xy * _MainTex1_ST.xy) + _MainTex1_ST.zw);
                  o.color = v.color;
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                  float4 res_1;
                  float4 tmpvar_2;
                  tmpvar_2 = lerp (tex2D (_MainTex0, i.texcoord0.xy), tex2D (_MainTex1, i.texcoord1), i.color.wwww);
                  res_1.w = tmpvar_2.w;
                  res_1.xyz = (tmpvar_2.xyz * (2.0 * UNITY_SAMPLE_TEX2D (unity_Lightmap, i.texcoord0.zw).xyz));
                  return res_1;
            }
            ENDCG
        }
    }
    
}