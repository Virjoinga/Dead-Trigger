üShader "MADFINGER/Particles/Multiply TwoSided FPV" {
Properties {
 _MainTex ("Texture", 2D) = "black" {}
 _TintColor ("Color", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Fog {
   Color (0,0,0,0)
  }
  Blend DstColor Zero
  ColorMask RGB
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 _TintColor;
uniform highp vec4 _ProjParams;
uniform highp vec4 _MainTex_ST;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  highp mat4 projTM_1;
  highp vec4 tmpvar_2;
  lowp vec4 tmpvar_3;
  projTM_1 = glstate_matrix_projection;
  projTM_1[0] = glstate_matrix_projection[0]; projTM_1[0].x = (glstate_matrix_projection[0].x * _ProjParams.x);
  projTM_1[1] = projTM_1[1]; projTM_1[1].y = (projTM_1[1].y * _ProjParams.y);
  highp vec4 tmpvar_4;
  tmpvar_4.w = 1.0;
  tmpvar_4.xyz = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_5;
  tmpvar_5 = (projTM_1 * tmpvar_4);
  tmpvar_2.xyw = tmpvar_5.xyw;
  tmpvar_2.z = (tmpvar_5.z * _ProjParams.z);
  tmpvar_2.z = (tmpvar_2.z + (_ProjParams.w * tmpvar_5.w));
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = (_glesColor.xyz * _TintColor.xyz);
  tmpvar_3 = tmpvar_6;
  gl_Position = tmpvar_2;
  xlv_COLOR = tmpvar_3;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = (texture2D (_MainTex, xlv_TEXCOORD0) * xlv_COLOR);
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesColor;
in vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 _TintColor;
uniform highp vec4 _ProjParams;
uniform highp vec4 _MainTex_ST;
out lowp vec4 xlv_COLOR;
out highp vec2 xlv_TEXCOORD0;
void main ()
{
  highp mat4 projTM_1;
  highp vec4 tmpvar_2;
  lowp vec4 tmpvar_3;
  projTM_1 = glstate_matrix_projection;
  projTM_1[0] = glstate_matrix_projection[0]; projTM_1[0].x = (glstate_matrix_projection[0].x * _ProjParams.x);
  projTM_1[1] = projTM_1[1]; projTM_1[1].y = (projTM_1[1].y * _ProjParams.y);
  highp vec4 tmpvar_4;
  tmpvar_4.w = 1.0;
  tmpvar_4.xyz = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_5;
  tmpvar_5 = (projTM_1 * tmpvar_4);
  tmpvar_2.xyw = tmpvar_5.xyw;
  tmpvar_2.z = (tmpvar_5.z * _ProjParams.z);
  tmpvar_2.z = (tmpvar_2.z + (_ProjParams.w * tmpvar_5.w));
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = (_glesColor.xyz * _TintColor.xyz);
  tmpvar_3 = tmpvar_6;
  gl_Position = tmpvar_2;
  xlv_COLOR = tmpvar_3;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in highp vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = (texture (_MainTex, xlv_TEXCOORD0) * xlv_COLOR);
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
}