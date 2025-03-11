Ñ(Shader "Hidden/LensFlareCreate" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "" {}
}
SubShader { 
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
  Blend One One
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD0_1;
varying highp vec2 xlv_TEXCOORD0_2;
varying highp vec2 xlv_TEXCOORD0_3;
varying highp vec2 xlv_TEXCOORD0_4;
void main ()
{
  highp vec2 tmpvar_1;
  highp vec2 tmpvar_2;
  highp vec2 tmpvar_3;
  highp vec2 tmpvar_4;
  highp vec2 tmpvar_5;
  mediump vec2 tmpvar_6;
  tmpvar_6 = _glesMultiTexCoord0.xy;
  tmpvar_1 = tmpvar_6;
  mediump vec2 tmpvar_7;
  tmpvar_7 = (((_glesMultiTexCoord0.xy - 0.5) * -0.85) + 0.5);
  tmpvar_2 = tmpvar_7;
  mediump vec2 tmpvar_8;
  tmpvar_8 = (((_glesMultiTexCoord0.xy - 0.5) * -1.45) + 0.5);
  tmpvar_3 = tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9 = (((_glesMultiTexCoord0.xy - 0.5) * -2.55) + 0.5);
  tmpvar_4 = tmpvar_9;
  mediump vec2 tmpvar_10;
  tmpvar_10 = (((_glesMultiTexCoord0.xy - 0.5) * -4.15) + 0.5);
  tmpvar_5 = tmpvar_10;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD0_1 = tmpvar_2;
  xlv_TEXCOORD0_2 = tmpvar_3;
  xlv_TEXCOORD0_3 = tmpvar_4;
  xlv_TEXCOORD0_4 = tmpvar_5;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 color0;
uniform highp vec4 colorA;
uniform highp vec4 colorB;
uniform highp vec4 colorC;
uniform highp vec4 colorD;
uniform sampler2D _MainTex;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD0_1;
varying highp vec2 xlv_TEXCOORD0_2;
varying highp vec2 xlv_TEXCOORD0_3;
varying highp vec2 xlv_TEXCOORD0_4;
void main ()
{
  mediump vec4 color_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  highp vec4 tmpvar_3;
  tmpvar_3 = (tmpvar_2 * color0);
  color_1 = tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0_1);
  highp vec4 tmpvar_5;
  tmpvar_5 = (color_1 + (tmpvar_4 * colorA));
  color_1 = tmpvar_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0_2);
  highp vec4 tmpvar_7;
  tmpvar_7 = (color_1 + (tmpvar_6 * colorB));
  color_1 = tmpvar_7;
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0_3);
  highp vec4 tmpvar_9;
  tmpvar_9 = (color_1 + (tmpvar_8 * colorC));
  color_1 = tmpvar_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD0_4);
  highp vec4 tmpvar_11;
  tmpvar_11 = (color_1 + (tmpvar_10 * colorD));
  color_1 = tmpvar_11;
  gl_FragData[0] = color_1;
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
out highp vec2 xlv_TEXCOORD0_1;
out highp vec2 xlv_TEXCOORD0_2;
out highp vec2 xlv_TEXCOORD0_3;
out highp vec2 xlv_TEXCOORD0_4;
void main ()
{
  highp vec2 tmpvar_1;
  highp vec2 tmpvar_2;
  highp vec2 tmpvar_3;
  highp vec2 tmpvar_4;
  highp vec2 tmpvar_5;
  mediump vec2 tmpvar_6;
  tmpvar_6 = _glesMultiTexCoord0.xy;
  tmpvar_1 = tmpvar_6;
  mediump vec2 tmpvar_7;
  tmpvar_7 = (((_glesMultiTexCoord0.xy - 0.5) * -0.85) + 0.5);
  tmpvar_2 = tmpvar_7;
  mediump vec2 tmpvar_8;
  tmpvar_8 = (((_glesMultiTexCoord0.xy - 0.5) * -1.45) + 0.5);
  tmpvar_3 = tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9 = (((_glesMultiTexCoord0.xy - 0.5) * -2.55) + 0.5);
  tmpvar_4 = tmpvar_9;
  mediump vec2 tmpvar_10;
  tmpvar_10 = (((_glesMultiTexCoord0.xy - 0.5) * -4.15) + 0.5);
  tmpvar_5 = tmpvar_10;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD0_1 = tmpvar_2;
  xlv_TEXCOORD0_2 = tmpvar_3;
  xlv_TEXCOORD0_3 = tmpvar_4;
  xlv_TEXCOORD0_4 = tmpvar_5;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform highp vec4 color0;
uniform highp vec4 colorA;
uniform highp vec4 colorB;
uniform highp vec4 colorC;
uniform highp vec4 colorD;
uniform sampler2D _MainTex;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD0_1;
in highp vec2 xlv_TEXCOORD0_2;
in highp vec2 xlv_TEXCOORD0_3;
in highp vec2 xlv_TEXCOORD0_4;
void main ()
{
  mediump vec4 color_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  highp vec4 tmpvar_3;
  tmpvar_3 = (tmpvar_2 * color0);
  color_1 = tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_MainTex, xlv_TEXCOORD0_1);
  highp vec4 tmpvar_5;
  tmpvar_5 = (color_1 + (tmpvar_4 * colorA));
  color_1 = tmpvar_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_MainTex, xlv_TEXCOORD0_2);
  highp vec4 tmpvar_7;
  tmpvar_7 = (color_1 + (tmpvar_6 * colorB));
  color_1 = tmpvar_7;
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture (_MainTex, xlv_TEXCOORD0_3);
  highp vec4 tmpvar_9;
  tmpvar_9 = (color_1 + (tmpvar_8 * colorC));
  color_1 = tmpvar_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_MainTex, xlv_TEXCOORD0_4);
  highp vec4 tmpvar_11;
  tmpvar_11 = (color_1 + (tmpvar_10 * colorD));
  color_1 = tmpvar_11;
  _glesFragData[0] = color_1;
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