ËShader "Hidden/HollywoodFlareBlurShader" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "" {}
 _NonBlurredTex ("Base (RGB)", 2D) = "" {}
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

uniform highp vec4 tintColor;
uniform sampler2D _MainTex;
uniform sampler2D _NonBlurredTex;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  mediump vec4 tmpvar_1;
  mediump vec4 colorNb_2;
  mediump vec4 color_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0);
  color_3 = tmpvar_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_NonBlurredTex, xlv_TEXCOORD0);
  colorNb_2 = tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6 = normalize(tintColor);
  tmpvar_1 = (((color_3 * tintColor) * 0.5) + ((colorNb_2 * tmpvar_6) * 0.5));
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
uniform highp vec4 tintColor;
uniform sampler2D _MainTex;
uniform sampler2D _NonBlurredTex;
in highp vec2 xlv_TEXCOORD0;
void main ()
{
  mediump vec4 tmpvar_1;
  mediump vec4 colorNb_2;
  mediump vec4 color_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_MainTex, xlv_TEXCOORD0);
  color_3 = tmpvar_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture (_NonBlurredTex, xlv_TEXCOORD0);
  colorNb_2 = tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6 = normalize(tintColor);
  tmpvar_1 = (((color_3 * tintColor) * 0.5) + ((colorNb_2 * tmpvar_6) * 0.5));
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