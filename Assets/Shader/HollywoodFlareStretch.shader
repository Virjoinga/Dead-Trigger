û(Shader "Hidden/HollywoodFlareStretchShader" {
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
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  highp vec2 tmpvar_1;
  mediump vec2 tmpvar_2;
  tmpvar_2 = _glesMultiTexCoord0.xy;
  tmpvar_1 = tmpvar_2;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 offsets;
uniform highp float stretchWidth;
uniform sampler2D _MainTex;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 color_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  color_2 = tmpvar_3;
  lowp vec4 tmpvar_4;
  highp vec2 P_5;
  P_5 = (xlv_TEXCOORD0 + ((stretchWidth * 2.0) * offsets.xy));
  tmpvar_4 = texture2D (_MainTex, P_5);
  lowp vec4 tmpvar_6;
  highp vec2 P_7;
  P_7 = (xlv_TEXCOORD0 - ((stretchWidth * 2.0) * offsets.xy));
  tmpvar_6 = texture2D (_MainTex, P_7);
  lowp vec4 tmpvar_8;
  highp vec2 P_9;
  P_9 = (xlv_TEXCOORD0 + ((stretchWidth * 4.0) * offsets.xy));
  tmpvar_8 = texture2D (_MainTex, P_9);
  lowp vec4 tmpvar_10;
  highp vec2 P_11;
  P_11 = (xlv_TEXCOORD0 - ((stretchWidth * 4.0) * offsets.xy));
  tmpvar_10 = texture2D (_MainTex, P_11);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD0 + ((stretchWidth * 8.0) * offsets.xy));
  tmpvar_12 = texture2D (_MainTex, P_13);
  lowp vec4 tmpvar_14;
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0 - ((stretchWidth * 8.0) * offsets.xy));
  tmpvar_14 = texture2D (_MainTex, P_15);
  lowp vec4 tmpvar_16;
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0 + ((stretchWidth * 14.0) * offsets.xy));
  tmpvar_16 = texture2D (_MainTex, P_17);
  lowp vec4 tmpvar_18;
  highp vec2 P_19;
  P_19 = (xlv_TEXCOORD0 - ((stretchWidth * 14.0) * offsets.xy));
  tmpvar_18 = texture2D (_MainTex, P_19);
  lowp vec4 tmpvar_20;
  highp vec2 P_21;
  P_21 = (xlv_TEXCOORD0 + ((stretchWidth * 20.0) * offsets.xy));
  tmpvar_20 = texture2D (_MainTex, P_21);
  lowp vec4 tmpvar_22;
  highp vec2 P_23;
  P_23 = (xlv_TEXCOORD0 - ((stretchWidth * 20.0) * offsets.xy));
  tmpvar_22 = texture2D (_MainTex, P_23);
  highp vec4 tmpvar_24;
  tmpvar_24 = max (max (max (max (max (max (max (max (max (max (color_2, tmpvar_4), tmpvar_6), tmpvar_8), tmpvar_10), tmpvar_12), tmpvar_14), tmpvar_16), tmpvar_18), tmpvar_20), tmpvar_22);
  color_2 = tmpvar_24;
  tmpvar_1 = tmpvar_24;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
out highp vec2 xlv_TEXCOORD0;
void main ()
{
  highp vec2 tmpvar_1;
  mediump vec2 tmpvar_2;
  tmpvar_2 = _glesMultiTexCoord0.xy;
  tmpvar_1 = tmpvar_2;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform highp vec4 offsets;
uniform highp float stretchWidth;
uniform sampler2D _MainTex;
in highp vec2 xlv_TEXCOORD0;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 color_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD0);
  color_2 = tmpvar_3;
  lowp vec4 tmpvar_4;
  highp vec2 P_5;
  P_5 = (xlv_TEXCOORD0 + ((stretchWidth * 2.0) * offsets.xy));
  tmpvar_4 = texture (_MainTex, P_5);
  lowp vec4 tmpvar_6;
  highp vec2 P_7;
  P_7 = (xlv_TEXCOORD0 - ((stretchWidth * 2.0) * offsets.xy));
  tmpvar_6 = texture (_MainTex, P_7);
  lowp vec4 tmpvar_8;
  highp vec2 P_9;
  P_9 = (xlv_TEXCOORD0 + ((stretchWidth * 4.0) * offsets.xy));
  tmpvar_8 = texture (_MainTex, P_9);
  lowp vec4 tmpvar_10;
  highp vec2 P_11;
  P_11 = (xlv_TEXCOORD0 - ((stretchWidth * 4.0) * offsets.xy));
  tmpvar_10 = texture (_MainTex, P_11);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD0 + ((stretchWidth * 8.0) * offsets.xy));
  tmpvar_12 = texture (_MainTex, P_13);
  lowp vec4 tmpvar_14;
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0 - ((stretchWidth * 8.0) * offsets.xy));
  tmpvar_14 = texture (_MainTex, P_15);
  lowp vec4 tmpvar_16;
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0 + ((stretchWidth * 14.0) * offsets.xy));
  tmpvar_16 = texture (_MainTex, P_17);
  lowp vec4 tmpvar_18;
  highp vec2 P_19;
  P_19 = (xlv_TEXCOORD0 - ((stretchWidth * 14.0) * offsets.xy));
  tmpvar_18 = texture (_MainTex, P_19);
  lowp vec4 tmpvar_20;
  highp vec2 P_21;
  P_21 = (xlv_TEXCOORD0 + ((stretchWidth * 20.0) * offsets.xy));
  tmpvar_20 = texture (_MainTex, P_21);
  lowp vec4 tmpvar_22;
  highp vec2 P_23;
  P_23 = (xlv_TEXCOORD0 - ((stretchWidth * 20.0) * offsets.xy));
  tmpvar_22 = texture (_MainTex, P_23);
  highp vec4 tmpvar_24;
  tmpvar_24 = max (max (max (max (max (max (max (max (max (max (color_2, tmpvar_4), tmpvar_6), tmpvar_8), tmpvar_10), tmpvar_12), tmpvar_14), tmpvar_16), tmpvar_18), tmpvar_20), tmpvar_22);
  color_2 = tmpvar_24;
  tmpvar_1 = tmpvar_24;
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