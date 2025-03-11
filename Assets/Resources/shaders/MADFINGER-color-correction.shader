½Shader "MADFINGER/PostFX/ColorCorrection" {
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
uniform highp mat4 _ColorMatrix;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec3 dst_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_3;
  tmpvar_3.w = 1.0;
  tmpvar_3.xyz = tmpvar_2.xyz;
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * _ColorMatrix).xyz;
  dst_1 = tmpvar_4;
  lowp vec4 tmpvar_5;
  tmpvar_5.xyz = dst_1;
  tmpvar_5.w = tmpvar_2.w;
  gl_FragData[0] = tmpvar_5;
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
uniform highp mat4 _ColorMatrix;
in highp vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec3 dst_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_3;
  tmpvar_3.w = 1.0;
  tmpvar_3.xyz = tmpvar_2.xyz;
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * _ColorMatrix).xyz;
  dst_1 = tmpvar_4;
  lowp vec4 tmpvar_5;
  tmpvar_5.xyz = dst_1;
  tmpvar_5.w = tmpvar_2.w;
  _glesFragData[0] = tmpvar_5;
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