// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'

Shader "Hidden/VignetteShader" 
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "" {}
    }
    SubShader
    {
        Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off
            Fog { Mode Off }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex;
            float vignetteIntensity;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
            };
            struct v2f
            {
                float2 texcoord0 : TEXCOORD0;
                float4 vertex : POSITION;
            };
            v2f vert(appdata_t v)
            {
                v2f o;
                  float2 tmpvar_1;
                  float2 tmpvar_2;
                  tmpvar_2 = v.texcoord0.xy;
                  tmpvar_1 = tmpvar_2;
                  o.vertex = (UnityObjectToClipPos(v.vertex));
                  o.texcoord0 = tmpvar_1;
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                  float4 tmpvar_1;
                  float4 color_2;
                  float2 uv_3;
                  float2 coords_4;
                  coords_4 = i.texcoord0;
                  uv_3 = i.texcoord0;
                  float2 tmpvar_5;
                  tmpvar_5 = ((coords_4 - 0.5) * 2.0);
                  coords_4 = tmpvar_5;
                  float tmpvar_6;
                  tmpvar_6 = dot (tmpvar_5, tmpvar_5);
                  float4 tmpvar_7;
                  tmpvar_7 = tex2D (_MainTex, uv_3);
                  color_2 = tmpvar_7;
                  float tmpvar_8;
                  tmpvar_8 = (1.0 - (tmpvar_6 * vignetteIntensity));
                  tmpvar_1 = (color_2 * tmpvar_8);
                  return tmpvar_1;
            }
            ENDCG
        }
    }
    Fallback Off
}