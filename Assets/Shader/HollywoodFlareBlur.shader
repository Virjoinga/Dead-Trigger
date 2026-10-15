// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'

Shader "Hidden/HollywoodFlareBlurShader" 
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "" {}
        _NonBlurredTex ("Base (RGB)", 2D) = "" {}
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
            float4 tintColor;
            sampler2D _MainTex;
            sampler2D _NonBlurredTex;
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
                  float4 colorNb_2;
                  float4 color_3;
                  float4 tmpvar_4;
                  tmpvar_4 = tex2D (_MainTex, i.texcoord0);
                  color_3 = tmpvar_4;
                  float4 tmpvar_5;
                  tmpvar_5 = tex2D (_NonBlurredTex, i.texcoord0);
                  colorNb_2 = tmpvar_5;
                  float4 tmpvar_6;
                  tmpvar_6 = normalize(tintColor);
                  tmpvar_1 = (((color_3 * tintColor) * 0.5) + ((colorNb_2 * tmpvar_6) * 0.5));
                  return tmpvar_1;
            }
            ENDCG
        }
    }
    Fallback Off
}