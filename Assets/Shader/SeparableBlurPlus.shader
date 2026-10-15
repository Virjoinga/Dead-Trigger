// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'

Shader "Hidden/SeparableBlurPlus" 
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
            float4 offsets;
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
                float4 texcoord2 : TEXCOORD2;
                float4 texcoord3 : TEXCOORD3;
                float4 texcoord4 : TEXCOORD4;
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
                  o.texcoord1 = (v.texcoord0.xyxy + (offsets.xyxy * float4(1.0, 1.0, -1.0, -1.0)));
                  o.texcoord2 = (v.texcoord0.xyxy + (float4(2.0, 2.0, -2.0, -2.0) * offsets.xyxy));
                  o.texcoord3 = (v.texcoord0.xyxy + (float4(3.0, 3.0, -3.0, -3.0) * offsets.xyxy));
                  o.texcoord4 = (v.texcoord0.xyxy + (float4(7.0, 7.0, -7.0, -7.0) * offsets.xyxy));
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                  float4 tmpvar_1;
                  tmpvar_1 = tex2D (_MainTex, i.texcoord0);
                  float4 tmpvar_2;
                  tmpvar_2 = tex2D (_MainTex, i.texcoord1.xy);
                  float4 tmpvar_3;
                  tmpvar_3 = tex2D (_MainTex, i.texcoord1.zw);
                  float4 tmpvar_4;
                  tmpvar_4 = tex2D (_MainTex, i.texcoord2.xy);
                  float4 tmpvar_5;
                  tmpvar_5 = tex2D (_MainTex, i.texcoord2.zw);
                  float4 tmpvar_6;
                  tmpvar_6 = tex2D (_MainTex, i.texcoord3.xy);
                  float4 tmpvar_7;
                  tmpvar_7 = tex2D (_MainTex, i.texcoord3.zw);
                  float4 tmpvar_8;
                  tmpvar_8 = tex2D (_MainTex, i.texcoord4.xy);
                  float4 tmpvar_9;
                  tmpvar_9 = tex2D (_MainTex, i.texcoord4.zw);
                  return (((((((((0.25 * tmpvar_1) + (0.15 * tmpvar_2)) + (0.15 * tmpvar_3)) + (0.11 * tmpvar_4)) + (0.11 * tmpvar_5)) + (0.075 * tmpvar_6)) + (0.075 * tmpvar_7)) + (0.04 * tmpvar_8)) + (0.04 * tmpvar_9));
            }
            ENDCG
        }
    }
    Fallback Off
}