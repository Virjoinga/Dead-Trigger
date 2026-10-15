Shader "MADFINGER/PostFX/ScreenSpaceLightFX" 
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
            float4 _GlowsIntensityMask;
            float4 _GlobalColor;
            float4x4 _UnprojectTM;
            float4x4 _Glow0Params;
            float4x4 _Glow1Params;
            float4x4 _Glow2Params;
            float4x4 _Glow3Params;
            sampler2D _MainTex;
            struct appdata_t
            {
                float4 vertex : POSITION;
            };
            struct v2f
            {
                float2 texcoord0 : TEXCOORD0;
                float4 color : COLOR;
                float4 vertex : POSITION;
            };
            v2f vert(appdata_t v)
            {
                v2f o;
                  float4 tmpvar_1;
                  float4 tmpvar_2;
                  tmpvar_2.zw = float2(0.0, 1.0);
                  tmpvar_2.xy = ((v.vertex.xy * 2.0) - 1.0);
                  float4 tmpvar_3;
                  tmpvar_3 = mul(_UnprojectTM, tmpvar_2);
                  float3 tmpvar_4;
                  tmpvar_4 = normalize(((tmpvar_3 * (1.0/(tmpvar_3.w))).xyz - _WorldSpaceCameraPos));
                  float4 tmpvar_5;
                  tmpvar_5.zw = float2(0.0, 1.0);
                  tmpvar_5.xy = ((v.vertex.xy * 2.0) - float2(1.0, 1.0));
                  float4 v_6;
                  v_6.x = _Glow0Params[0].x;
                  v_6.y = _Glow0Params[1].x;
                  v_6.z = _Glow0Params[2].x;
                  v_6.w = _Glow0Params[3].x;
                  float4 v_7;
                  v_7.x = _Glow1Params[0].x;
                  v_7.y = _Glow1Params[1].x;
                  v_7.z = _Glow1Params[2].x;
                  v_7.w = _Glow1Params[3].x;
                  float4 v_8;
                  v_8.x = _Glow2Params[0].x;
                  v_8.y = _Glow2Params[1].x;
                  v_8.z = _Glow2Params[2].x;
                  v_8.w = _Glow2Params[3].x;
                  float4 v_9;
                  v_9.x = _Glow3Params[0].x;
                  v_9.y = _Glow3Params[1].x;
                  v_9.z = _Glow3Params[2].x;
                  v_9.w = _Glow3Params[3].x;
                  float4 tmpvar_10;
                  tmpvar_10.x = dot (-(normalize((_WorldSpaceCameraPos - v_6.xyz))), tmpvar_4);
                  tmpvar_10.y = dot (-(normalize((_WorldSpaceCameraPos - v_7.xyz))), tmpvar_4);
                  tmpvar_10.z = dot (-(normalize((_WorldSpaceCameraPos - v_8.xyz))), tmpvar_4);
                  tmpvar_10.w = dot (-(normalize((_WorldSpaceCameraPos - v_9.xyz))), tmpvar_4);
                  float4 tmpvar_11;
                  tmpvar_11 = max (tmpvar_10, float4(0.0, 0.0, 0.0, 0.0));
                  float4 tmpvar_12;
                  tmpvar_12 = ((tmpvar_11 * tmpvar_11) * _GlowsIntensityMask);
                  float4 v_13;
                  v_13.x = _Glow0Params[0].y;
                  v_13.y = _Glow0Params[1].y;
                  v_13.z = _Glow0Params[2].y;
                  v_13.w = _Glow0Params[3].y;
                  float4 v_14;
                  v_14.x = _Glow1Params[0].y;
                  v_14.y = _Glow1Params[1].y;
                  v_14.z = _Glow1Params[2].y;
                  v_14.w = _Glow1Params[3].y;
                  float4 v_15;
                  v_15.x = _Glow2Params[0].y;
                  v_15.y = _Glow2Params[1].y;
                  v_15.z = _Glow2Params[2].y;
                  v_15.w = _Glow2Params[3].y;
                  float4 v_16;
                  v_16.x = _Glow3Params[0].y;
                  v_16.y = _Glow3Params[1].y;
                  v_16.z = _Glow3Params[2].y;
                  v_16.w = _Glow3Params[3].y;
                  float4 tmpvar_17;
                  tmpvar_17 = ((((tmpvar_12.x * v_13.xyzz) + (tmpvar_12.y * v_14.xyzz)) + (tmpvar_12.z * v_15.xyzz)) + (tmpvar_12.w * v_16.xyzz));
                  tmpvar_1 = tmpvar_17;
                  float4 tmpvar_18;
                  tmpvar_18 = (tmpvar_1 + _GlobalColor);
                  tmpvar_1 = tmpvar_18;
                  o.vertex = tmpvar_5;
                  o.texcoord0 = v.vertex.xy;
                  o.color = tmpvar_1;
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                  float4 tmpvar_1;
                  tmpvar_1 = (tex2D (_MainTex, i.texcoord0) + i.color);
                  return tmpvar_1;
            }
            ENDCG
        }
    }
    Fallback Off
}