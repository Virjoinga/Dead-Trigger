ÀShader "MADFINGER/PostFX/WaterScreenRefraction" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "" {}
 _EnvMap ("2D EnvMap", 2D) = "black" {}
 _ScrollingSpeed ("xy - Layer0, zw - Layer1", Vector) = (0,0.05,0,0.01)
 _Color ("Color", Color) = (0,0,0,0)
 _Params ("x = refraction strength, y = Layer 0 tiling, z = Layer 1 tiling", Vector) = (0.01,1.5,2,0)
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
uniform highp vec4 _ScreenParams;
uniform highp vec4 _Params;
uniform highp vec4 _Color;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
void main ()
{
  highp vec3 norm_1;
  highp vec2 tmpvar_2;
  lowp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  tmpvar_4.z = 0.25;
  tmpvar_4.xy = _glesMultiTexCoord0.xy;
  mediump vec3 tmpvar_5;
  tmpvar_5 = -(normalize(tmpvar_4));
  norm_1 = tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = vec2(0.0, 1.0);
  tmpvar_6.x = _glesVertex.x;
  tmpvar_6.y = -(_glesVertex.y);
  highp vec2 tmpvar_7;
  tmpvar_7 = ((((_glesVertex.xy * 0.5) + 0.5) + (0.5 / _ScreenParams.xy)) + (norm_1.xy * _Params.x));
  tmpvar_2.x = tmpvar_7.x;
  tmpvar_2.y = (1.0 - tmpvar_7.y);
  highp vec4 tmpvar_8;
  tmpvar_8 = ((_glesVertex.z * 2.0) * _Color);
  tmpvar_3 = tmpvar_8;
  gl_Position = tmpvar_6;
  xlv_TEXCOORD0 = tmpvar_2;
  xlv_TEXCOORD1 = norm_1.xy;
  xlv_COLOR = tmpvar_3;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _EnvMap;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = ((texture2D (_MainTex, xlv_TEXCOORD0) + texture2D (_EnvMap, xlv_TEXCOORD1)) + xlv_COLOR);
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
uniform highp vec4 _ScreenParams;
uniform highp vec4 _Params;
uniform highp vec4 _Color;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out lowp vec4 xlv_COLOR;
void main ()
{
  highp vec3 norm_1;
  highp vec2 tmpvar_2;
  lowp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  tmpvar_4.z = 0.25;
  tmpvar_4.xy = _glesMultiTexCoord0.xy;
  mediump vec3 tmpvar_5;
  tmpvar_5 = -(normalize(tmpvar_4));
  norm_1 = tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = vec2(0.0, 1.0);
  tmpvar_6.x = _glesVertex.x;
  tmpvar_6.y = -(_glesVertex.y);
  highp vec2 tmpvar_7;
  tmpvar_7 = ((((_glesVertex.xy * 0.5) + 0.5) + (0.5 / _ScreenParams.xy)) + (norm_1.xy * _Params.x));
  tmpvar_2.x = tmpvar_7.x;
  tmpvar_2.y = (1.0 - tmpvar_7.y);
  highp vec4 tmpvar_8;
  tmpvar_8 = ((_glesVertex.z * 2.0) * _Color);
  tmpvar_3 = tmpvar_8;
  gl_Position = tmpvar_6;
  xlv_TEXCOORD0 = tmpvar_2;
  xlv_TEXCOORD1 = norm_1.xy;
  xlv_COLOR = tmpvar_3;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _EnvMap;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = ((texture (_MainTex, xlv_TEXCOORD0) + texture (_EnvMap, xlv_TEXCOORD1)) + xlv_COLOR);
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