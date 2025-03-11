Ò1Shader "MADFINGER/PostFX/ScreenSpaceLightFX" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "" {}
}
SubShader { 
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _GlowsIntensityMask;
uniform highp vec4 _GlobalColor;
uniform highp mat4 _UnprojectTM;
uniform highp mat4 _Glow0Params;
uniform highp mat4 _Glow1Params;
uniform highp mat4 _Glow2Params;
uniform highp mat4 _Glow3Params;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  tmpvar_2.zw = vec2(0.0, 1.0);
  tmpvar_2.xy = ((_glesVertex.xy * 2.0) - 1.0);
  highp vec4 tmpvar_3;
  tmpvar_3 = (_UnprojectTM * tmpvar_2);
  highp vec3 tmpvar_4;
  tmpvar_4 = normalize(((tmpvar_3 * (1.0/(tmpvar_3.w))).xyz - _WorldSpaceCameraPos));
  highp vec4 tmpvar_5;
  tmpvar_5.zw = vec2(0.0, 1.0);
  tmpvar_5.xy = ((_glesVertex.xy * 2.0) - vec2(1.0, 1.0));
  vec4 v_6;
  v_6.x = _Glow0Params[0].x;
  v_6.y = _Glow0Params[1].x;
  v_6.z = _Glow0Params[2].x;
  v_6.w = _Glow0Params[3].x;
  vec4 v_7;
  v_7.x = _Glow1Params[0].x;
  v_7.y = _Glow1Params[1].x;
  v_7.z = _Glow1Params[2].x;
  v_7.w = _Glow1Params[3].x;
  vec4 v_8;
  v_8.x = _Glow2Params[0].x;
  v_8.y = _Glow2Params[1].x;
  v_8.z = _Glow2Params[2].x;
  v_8.w = _Glow2Params[3].x;
  vec4 v_9;
  v_9.x = _Glow3Params[0].x;
  v_9.y = _Glow3Params[1].x;
  v_9.z = _Glow3Params[2].x;
  v_9.w = _Glow3Params[3].x;
  highp vec4 tmpvar_10;
  tmpvar_10.x = dot (-(normalize((_WorldSpaceCameraPos - v_6.xyz))), tmpvar_4);
  tmpvar_10.y = dot (-(normalize((_WorldSpaceCameraPos - v_7.xyz))), tmpvar_4);
  tmpvar_10.z = dot (-(normalize((_WorldSpaceCameraPos - v_8.xyz))), tmpvar_4);
  tmpvar_10.w = dot (-(normalize((_WorldSpaceCameraPos - v_9.xyz))), tmpvar_4);
  highp vec4 tmpvar_11;
  tmpvar_11 = max (tmpvar_10, vec4(0.0, 0.0, 0.0, 0.0));
  highp vec4 tmpvar_12;
  tmpvar_12 = ((tmpvar_11 * tmpvar_11) * _GlowsIntensityMask);
  vec4 v_13;
  v_13.x = _Glow0Params[0].y;
  v_13.y = _Glow0Params[1].y;
  v_13.z = _Glow0Params[2].y;
  v_13.w = _Glow0Params[3].y;
  vec4 v_14;
  v_14.x = _Glow1Params[0].y;
  v_14.y = _Glow1Params[1].y;
  v_14.z = _Glow1Params[2].y;
  v_14.w = _Glow1Params[3].y;
  vec4 v_15;
  v_15.x = _Glow2Params[0].y;
  v_15.y = _Glow2Params[1].y;
  v_15.z = _Glow2Params[2].y;
  v_15.w = _Glow2Params[3].y;
  vec4 v_16;
  v_16.x = _Glow3Params[0].y;
  v_16.y = _Glow3Params[1].y;
  v_16.z = _Glow3Params[2].y;
  v_16.w = _Glow3Params[3].y;
  highp vec4 tmpvar_17;
  tmpvar_17 = ((((tmpvar_12.x * v_13.xyzz) + (tmpvar_12.y * v_14.xyzz)) + (tmpvar_12.z * v_15.xyzz)) + (tmpvar_12.w * v_16.xyzz));
  tmpvar_1 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_1 + _GlobalColor);
  tmpvar_1 = tmpvar_18;
  gl_Position = tmpvar_5;
  xlv_TEXCOORD0 = _glesVertex.xy;
  xlv_COLOR = tmpvar_1;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = (texture2D (_MainTex, xlv_TEXCOORD0) + xlv_COLOR);
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _GlowsIntensityMask;
uniform highp vec4 _GlobalColor;
uniform highp mat4 _UnprojectTM;
uniform highp mat4 _Glow0Params;
uniform highp mat4 _Glow1Params;
uniform highp mat4 _Glow2Params;
uniform highp mat4 _Glow3Params;
out highp vec2 xlv_TEXCOORD0;
out lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  tmpvar_2.zw = vec2(0.0, 1.0);
  tmpvar_2.xy = ((_glesVertex.xy * 2.0) - 1.0);
  highp vec4 tmpvar_3;
  tmpvar_3 = (_UnprojectTM * tmpvar_2);
  highp vec3 tmpvar_4;
  tmpvar_4 = normalize(((tmpvar_3 * (1.0/(tmpvar_3.w))).xyz - _WorldSpaceCameraPos));
  highp vec4 tmpvar_5;
  tmpvar_5.zw = vec2(0.0, 1.0);
  tmpvar_5.xy = ((_glesVertex.xy * 2.0) - vec2(1.0, 1.0));
  vec4 v_6;
  v_6.x = _Glow0Params[0].x;
  v_6.y = _Glow0Params[1].x;
  v_6.z = _Glow0Params[2].x;
  v_6.w = _Glow0Params[3].x;
  vec4 v_7;
  v_7.x = _Glow1Params[0].x;
  v_7.y = _Glow1Params[1].x;
  v_7.z = _Glow1Params[2].x;
  v_7.w = _Glow1Params[3].x;
  vec4 v_8;
  v_8.x = _Glow2Params[0].x;
  v_8.y = _Glow2Params[1].x;
  v_8.z = _Glow2Params[2].x;
  v_8.w = _Glow2Params[3].x;
  vec4 v_9;
  v_9.x = _Glow3Params[0].x;
  v_9.y = _Glow3Params[1].x;
  v_9.z = _Glow3Params[2].x;
  v_9.w = _Glow3Params[3].x;
  highp vec4 tmpvar_10;
  tmpvar_10.x = dot (-(normalize((_WorldSpaceCameraPos - v_6.xyz))), tmpvar_4);
  tmpvar_10.y = dot (-(normalize((_WorldSpaceCameraPos - v_7.xyz))), tmpvar_4);
  tmpvar_10.z = dot (-(normalize((_WorldSpaceCameraPos - v_8.xyz))), tmpvar_4);
  tmpvar_10.w = dot (-(normalize((_WorldSpaceCameraPos - v_9.xyz))), tmpvar_4);
  highp vec4 tmpvar_11;
  tmpvar_11 = max (tmpvar_10, vec4(0.0, 0.0, 0.0, 0.0));
  highp vec4 tmpvar_12;
  tmpvar_12 = ((tmpvar_11 * tmpvar_11) * _GlowsIntensityMask);
  vec4 v_13;
  v_13.x = _Glow0Params[0].y;
  v_13.y = _Glow0Params[1].y;
  v_13.z = _Glow0Params[2].y;
  v_13.w = _Glow0Params[3].y;
  vec4 v_14;
  v_14.x = _Glow1Params[0].y;
  v_14.y = _Glow1Params[1].y;
  v_14.z = _Glow1Params[2].y;
  v_14.w = _Glow1Params[3].y;
  vec4 v_15;
  v_15.x = _Glow2Params[0].y;
  v_15.y = _Glow2Params[1].y;
  v_15.z = _Glow2Params[2].y;
  v_15.w = _Glow2Params[3].y;
  vec4 v_16;
  v_16.x = _Glow3Params[0].y;
  v_16.y = _Glow3Params[1].y;
  v_16.z = _Glow3Params[2].y;
  v_16.w = _Glow3Params[3].y;
  highp vec4 tmpvar_17;
  tmpvar_17 = ((((tmpvar_12.x * v_13.xyzz) + (tmpvar_12.y * v_14.xyzz)) + (tmpvar_12.z * v_15.xyzz)) + (tmpvar_12.w * v_16.xyzz));
  tmpvar_1 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_1 + _GlobalColor);
  tmpvar_1 = tmpvar_18;
  gl_Position = tmpvar_5;
  xlv_TEXCOORD0 = _glesVertex.xy;
  xlv_COLOR = tmpvar_1;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
in highp vec2 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = (texture (_MainTex, xlv_TEXCOORD0) + xlv_COLOR);
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
"!!GLES"
}
SubProgram "gles3 " {
"!!GLES3"
}
}
 }
}
Fallback Off
}