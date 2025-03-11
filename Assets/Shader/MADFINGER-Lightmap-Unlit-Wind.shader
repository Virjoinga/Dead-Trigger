ê/Shader "MADFINGER/Environment/Lightmap + Wind" {
Properties {
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
 _Wind ("Wind params", Vector) = (1,1,1,1)
 _WindEdgeFlutter ("Wind edge fultter factor", Float) = 0.5
 _WindEdgeFlutterFreqScale ("Wind edge fultter freq scale", Float) = 0.5
}
SubShader { 
 LOD 100
 Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Transparent" "RenderType"="Transparent" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Transparent" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _Wind;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_LightmapST;
uniform highp float _WindEdgeFlutter;
uniform highp float _WindEdgeFlutterFreqScale;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
void main ()
{
  highp float bendingFact_1;
  highp vec4 wind_2;
  lowp float tmpvar_3;
  tmpvar_3 = _glesColor.w;
  bendingFact_1 = tmpvar_3;
  mat3 tmpvar_4;
  tmpvar_4[0] = _World2Object[0].xyz;
  tmpvar_4[1] = _World2Object[1].xyz;
  tmpvar_4[2] = _World2Object[2].xyz;
  wind_2.xyz = (tmpvar_4 * _Wind.xyz);
  wind_2.w = (_Wind.w * bendingFact_1);
  highp vec2 tmpvar_5;
  tmpvar_5.y = 1.0;
  tmpvar_5.x = _WindEdgeFlutterFreqScale;
  highp vec4 pos_6;
  pos_6.w = _glesVertex.w;
  highp vec3 bend_7;
  vec4 v_8;
  v_8.x = _Object2World[0].w;
  v_8.y = _Object2World[1].w;
  v_8.z = _Object2World[2].w;
  v_8.w = _Object2World[3].w;
  highp float tmpvar_9;
  tmpvar_9 = dot (v_8.xyz, vec3(1.0, 1.0, 1.0));
  highp vec2 tmpvar_10;
  tmpvar_10.x = dot (_glesVertex.xyz, vec3((_WindEdgeFlutter + tmpvar_9)));
  tmpvar_10.y = tmpvar_9;
  highp vec4 tmpvar_11;
  tmpvar_11 = abs(((fract((((fract((((_Time.y * tmpvar_5).xx + tmpvar_10).xxyy * vec4(1.975, 0.793, 0.375, 0.193))) * 2.0) - 1.0) + 0.5)) * 2.0) - 1.0));
  highp vec4 tmpvar_12;
  tmpvar_12 = ((tmpvar_11 * tmpvar_11) * (3.0 - (2.0 * tmpvar_11)));
  highp vec2 tmpvar_13;
  tmpvar_13 = (tmpvar_12.xz + tmpvar_12.yw);
  bend_7.xz = ((_WindEdgeFlutter * 0.1) * normalize(_glesNormal)).xz;
  bend_7.y = (bendingFact_1 * 0.3);
  pos_6.xyz = (_glesVertex.xyz + (((tmpvar_13.xyx * bend_7) + ((wind_2.xyz * tmpvar_13.y) * bendingFact_1)) * wind_2.w));
  pos_6.xyz = (pos_6.xyz + (bendingFact_1 * wind_2.xyz));
  gl_Position = (glstate_matrix_mvp * pos_6);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = _glesColor.xyz;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D unity_Lightmap;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz * (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz));
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _Wind;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_LightmapST;
uniform highp float _WindEdgeFlutter;
uniform highp float _WindEdgeFlutterFreqScale;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out lowp vec3 xlv_TEXCOORD2;
void main ()
{
  highp float bendingFact_1;
  highp vec4 wind_2;
  lowp float tmpvar_3;
  tmpvar_3 = _glesColor.w;
  bendingFact_1 = tmpvar_3;
  mat3 tmpvar_4;
  tmpvar_4[0] = _World2Object[0].xyz;
  tmpvar_4[1] = _World2Object[1].xyz;
  tmpvar_4[2] = _World2Object[2].xyz;
  wind_2.xyz = (tmpvar_4 * _Wind.xyz);
  wind_2.w = (_Wind.w * bendingFact_1);
  highp vec2 tmpvar_5;
  tmpvar_5.y = 1.0;
  tmpvar_5.x = _WindEdgeFlutterFreqScale;
  highp vec4 pos_6;
  pos_6.w = _glesVertex.w;
  highp vec3 bend_7;
  vec4 v_8;
  v_8.x = _Object2World[0].w;
  v_8.y = _Object2World[1].w;
  v_8.z = _Object2World[2].w;
  v_8.w = _Object2World[3].w;
  highp float tmpvar_9;
  tmpvar_9 = dot (v_8.xyz, vec3(1.0, 1.0, 1.0));
  highp vec2 tmpvar_10;
  tmpvar_10.x = dot (_glesVertex.xyz, vec3((_WindEdgeFlutter + tmpvar_9)));
  tmpvar_10.y = tmpvar_9;
  highp vec4 tmpvar_11;
  tmpvar_11 = abs(((fract((((fract((((_Time.y * tmpvar_5).xx + tmpvar_10).xxyy * vec4(1.975, 0.793, 0.375, 0.193))) * 2.0) - 1.0) + 0.5)) * 2.0) - 1.0));
  highp vec4 tmpvar_12;
  tmpvar_12 = ((tmpvar_11 * tmpvar_11) * (3.0 - (2.0 * tmpvar_11)));
  highp vec2 tmpvar_13;
  tmpvar_13 = (tmpvar_12.xz + tmpvar_12.yw);
  bend_7.xz = ((_WindEdgeFlutter * 0.1) * normalize(_glesNormal)).xz;
  bend_7.y = (bendingFact_1 * 0.3);
  pos_6.xyz = (_glesVertex.xyz + (((tmpvar_13.xyx * bend_7) + ((wind_2.xyz * tmpvar_13.y) * bendingFact_1)) * wind_2.w));
  pos_6.xyz = (pos_6.xyz + (bendingFact_1 * wind_2.xyz));
  gl_Position = (glstate_matrix_mvp * pos_6);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = _glesColor.xyz;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D unity_Lightmap;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz * (2.0 * texture (unity_Lightmap, xlv_TEXCOORD1).xyz));
  _glesFragData[0] = c_1;
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