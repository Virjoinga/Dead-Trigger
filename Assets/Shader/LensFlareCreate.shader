// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'

Shader "Hidden/LensFlareCreate" 
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
            Blend One One
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float4 color0;
            float4 colorA;
            float4 colorB;
            float4 colorC;
            float4 colorD;
            sampler2D _MainTex;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
            };
            struct v2f
            {
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord0_1 : TEXCOORD0_1;
                float2 texcoord0_2 : TEXCOORD0_2;
                float2 texcoord0_3 : TEXCOORD0_3;
                float2 texcoord0_4 : TEXCOORD0_4;
                float4 vertex : POSITION;
            };
            v2f vert(appdata_t v)
            {
                v2f o;
                  float2 tmpvar_1;
                  float2 tmpvar_2;
                  float2 tmpvar_3;
                  float2 tmpvar_4;
                  float2 tmpvar_5;
                  float2 tmpvar_6;
                  tmpvar_6 = v.texcoord0.xy;
                  tmpvar_1 = tmpvar_6;
                  float2 tmpvar_7;
                  tmpvar_7 = (((v.texcoord0.xy - 0.5) * -0.85) + 0.5);
                  tmpvar_2 = tmpvar_7;
                  float2 tmpvar_8;
                  tmpvar_8 = (((v.texcoord0.xy - 0.5) * -1.45) + 0.5);
                  tmpvar_3 = tmpvar_8;
                  float2 tmpvar_9;
                  tmpvar_9 = (((v.texcoord0.xy - 0.5) * -2.55) + 0.5);
                  tmpvar_4 = tmpvar_9;
                  float2 tmpvar_10;
                  tmpvar_10 = (((v.texcoord0.xy - 0.5) * -4.15) + 0.5);
                  tmpvar_5 = tmpvar_10;
                  o.vertex = (UnityObjectToClipPos(v.vertex));
                  o.texcoord0 = tmpvar_1;
                  o.texcoord0_1 = tmpvar_2;
                  o.texcoord0_2 = tmpvar_3;
                  o.texcoord0_3 = tmpvar_4;
                  o.texcoord0_4 = tmpvar_5;
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                  float4 color_1;
                  float4 tmpvar_2;
                  tmpvar_2 = tex2D (_MainTex, i.texcoord0);
                  float4 tmpvar_3;
                  tmpvar_3 = (tmpvar_2 * color0);
                  color_1 = tmpvar_3;
                  float4 tmpvar_4;
                  tmpvar_4 = tex2D (_MainTex, i.texcoord0_1);
                  float4 tmpvar_5;
                  tmpvar_5 = (color_1 + (tmpvar_4 * colorA));
                  color_1 = tmpvar_5;
                  float4 tmpvar_6;
                  tmpvar_6 = tex2D (_MainTex, i.texcoord0_2);
                  float4 tmpvar_7;
                  tmpvar_7 = (color_1 + (tmpvar_6 * colorB));
                  color_1 = tmpvar_7;
                  float4 tmpvar_8;
                  tmpvar_8 = tex2D (_MainTex, i.texcoord0_3);
                  float4 tmpvar_9;
                  tmpvar_9 = (color_1 + (tmpvar_8 * colorC));
                  color_1 = tmpvar_9;
                  float4 tmpvar_10;
                  tmpvar_10 = tex2D (_MainTex, i.texcoord0_4);
                  float4 tmpvar_11;
                  tmpvar_11 = (color_1 + (tmpvar_10 * colorD));
                  color_1 = tmpvar_11;
                  return color_1;
            }
            ENDCG
        }
    }
    Fallback Off
}