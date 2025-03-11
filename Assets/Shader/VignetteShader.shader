–Shader "Hidden/VignetteShader" {
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

uniform sampler2D _MainTex;
uniform highp float vignetteIntensity;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  mediump vec4 tmpvar_1;
  mediump vec4 color_2;
  mediump vec2 uv_3;
  mediump vec2 coords_4;
  coords_4 = xlv_TEXCOORD0;
  uv_3 = xlv_TEXCOORD0;
  mediump vec2 tmpvar_5;
  tmpvar_5 = ((coords_4 - 0.5) * 2.0);
  coords_4 = tmpvar_5;
  mediump float tmpvar_6;
  tmpvar_6 = dot (tmpvar_5, tmpvar_5);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, uv_3);
  color_2 = tmpvar_7;
  highp float tmpvar_8;
  tmpvar_8 = (1.0 - (tmpvar_6 * vignetteIntensity));
  tmpvar_1 = (color_2 * tmpvar_8);
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
uniform sampler2D _MainTex;
uniform highp float vignetteIntensity;
in highp vec2 xlv_TEXCOORD0;
void main ()
{
  mediump vec4 tmpvar_1;
  mediump vec4 color_2;
  mediump vec2 uv_3;
  mediump vec2 coords_4;
  coords_4 = xlv_TEXCOORD0;
  uv_3 = xlv_TEXCOORD0;
  mediump vec2 tmpvar_5;
  tmpvar_5 = ((coords_4 - 0.5) * 2.0);
  coords_4 = tmpvar_5;
  mediump float tmpvar_6;
  tmpvar_6 = dot (tmpvar_5, tmpvar_5);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture (_MainTex, uv_3);
  color_2 = tmpvar_7;
  highp float tmpvar_8;
  tmpvar_8 = (1.0 - (tmpvar_6 * vignetteIntensity));
  tmpvar_1 = (color_2 * tmpvar_8);
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