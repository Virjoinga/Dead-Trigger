˜/Shader "MADFINGER/Environment/Dual diffuse with lightmap" {
Properties {
 _MainTex0 ("Base layer0 (RGB)", 2D) = "white" {}
 _MainTex1 ("Base layer1 (RGB)", 2D) = "white" {}
}
SubShader { 
 LOD 100
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "RenderType"="Opaque" }
Program "vp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex0_ST;
uniform highp vec4 _MainTex1_ST;
uniform highp vec4 unity_LightmapST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex0_ST.xy) + _MainTex0_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex1_ST.xy) + _MainTex1_ST.zw);
  xlv_COLOR = _glesColor;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex0;
uniform sampler2D _MainTex1;
uniform sampler2D unity_Lightmap;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 res_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = mix (texture2D (_MainTex0, xlv_TEXCOORD0.xy), texture2D (_MainTex1, xlv_TEXCOORD1), xlv_COLOR.wwww);
  res_1.w = tmpvar_2.w;
  res_1.xyz = (tmpvar_2.xyz * (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD0.zw).xyz));
  gl_FragData[0] = res_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesColor;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex0_ST;
uniform highp vec4 _MainTex1_ST;
uniform highp vec4 unity_LightmapST;
out highp vec4 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out lowp vec4 xlv_COLOR;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex0_ST.xy) + _MainTex0_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex1_ST.xy) + _MainTex1_ST.zw);
  xlv_COLOR = _glesColor;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex0;
uniform sampler2D _MainTex1;
uniform sampler2D unity_Lightmap;
in highp vec4 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 res_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = mix (texture (_MainTex0, xlv_TEXCOORD0.xy), texture (_MainTex1, xlv_TEXCOORD1), xlv_COLOR.wwww);
  res_1.w = tmpvar_2.w;
  res_1.xyz = (tmpvar_2.xyz * (2.0 * texture (unity_Lightmap, xlv_TEXCOORD0.zw).xyz));
  _glesFragData[0] = res_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex0_ST;
uniform highp vec4 _MainTex1_ST;
uniform highp vec4 unity_LightmapST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex0_ST.xy) + _MainTex0_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex1_ST.xy) + _MainTex1_ST.zw);
  xlv_COLOR = _glesColor;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex0;
uniform sampler2D _MainTex1;
uniform sampler2D unity_Lightmap;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 res_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = mix (texture2D (_MainTex0, xlv_TEXCOORD0.xy), texture2D (_MainTex1, xlv_TEXCOORD1), xlv_COLOR.wwww);
  res_1.w = tmpvar_2.w;
  res_1.xyz = (tmpvar_2.xyz * (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD0.zw).xyz));
  gl_FragData[0] = res_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesColor;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex0_ST;
uniform highp vec4 _MainTex1_ST;
uniform highp vec4 unity_LightmapST;
out highp vec4 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out lowp vec4 xlv_COLOR;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex0_ST.xy) + _MainTex0_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex1_ST.xy) + _MainTex1_ST.zw);
  xlv_COLOR = _glesColor;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex0;
uniform sampler2D _MainTex1;
uniform sampler2D unity_Lightmap;
in highp vec4 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 res_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = mix (texture (_MainTex0, xlv_TEXCOORD0.xy), texture (_MainTex1, xlv_TEXCOORD1), xlv_COLOR.wwww);
  res_1.w = tmpvar_2.w;
  res_1.xyz = (tmpvar_2.xyz * (2.0 * texture (unity_Lightmap, xlv_TEXCOORD0.zw).xyz));
  _glesFragData[0] = res_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_ON" }
"!!GLES3"
}
}
 }
}
}