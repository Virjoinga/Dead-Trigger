¥"Shader "Hidden/SeparableBlurPlus" {
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
uniform highp vec4 offsets;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  highp vec2 tmpvar_1;
  mediump vec2 tmpvar_2;
  tmpvar_2 = _glesMultiTexCoord0.xy;
  tmpvar_1 = tmpvar_2;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = (_glesMultiTexCoord0.xyxy + (offsets.xyxy * vec4(1.0, 1.0, -1.0, -1.0)));
  xlv_TEXCOORD2 = (_glesMultiTexCoord0.xyxy + (vec4(2.0, 2.0, -2.0, -2.0) * offsets.xyxy));
  xlv_TEXCOORD3 = (_glesMultiTexCoord0.xyxy + (vec4(3.0, 3.0, -3.0, -3.0) * offsets.xyxy));
  xlv_TEXCOORD4 = (_glesMultiTexCoord0.xyxy + (vec4(7.0, 7.0, -7.0, -7.0) * offsets.xyxy));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD1.xy);
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD1.zw);
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD2.xy);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD2.zw);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD3.xy);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD3.zw);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD4.zw);
  gl_FragData[0] = (((((((((0.25 * tmpvar_1) + (0.15 * tmpvar_2)) + (0.15 * tmpvar_3)) + (0.11 * tmpvar_4)) + (0.11 * tmpvar_5)) + (0.075 * tmpvar_6)) + (0.075 * tmpvar_7)) + (0.04 * tmpvar_8)) + (0.04 * tmpvar_9));
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 offsets;
out highp vec2 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec4 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
void main ()
{
  highp vec2 tmpvar_1;
  mediump vec2 tmpvar_2;
  tmpvar_2 = _glesMultiTexCoord0.xy;
  tmpvar_1 = tmpvar_2;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = (_glesMultiTexCoord0.xyxy + (offsets.xyxy * vec4(1.0, 1.0, -1.0, -1.0)));
  xlv_TEXCOORD2 = (_glesMultiTexCoord0.xyxy + (vec4(2.0, 2.0, -2.0, -2.0) * offsets.xyxy));
  xlv_TEXCOORD3 = (_glesMultiTexCoord0.xyxy + (vec4(3.0, 3.0, -3.0, -3.0) * offsets.xyxy));
  xlv_TEXCOORD4 = (_glesMultiTexCoord0.xyxy + (vec4(7.0, 7.0, -7.0, -7.0) * offsets.xyxy));
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
in highp vec2 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD3;
in highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD1.xy);
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD1.zw);
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_MainTex, xlv_TEXCOORD2.xy);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD2.zw);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_MainTex, xlv_TEXCOORD3.xy);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture (_MainTex, xlv_TEXCOORD3.zw);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture (_MainTex, xlv_TEXCOORD4.xy);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_MainTex, xlv_TEXCOORD4.zw);
  _glesFragData[0] = (((((((((0.25 * tmpvar_1) + (0.15 * tmpvar_2)) + (0.15 * tmpvar_3)) + (0.11 * tmpvar_4)) + (0.11 * tmpvar_5)) + (0.075 * tmpvar_6)) + (0.075 * tmpvar_7)) + (0.04 * tmpvar_8)) + (0.04 * tmpvar_9));
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