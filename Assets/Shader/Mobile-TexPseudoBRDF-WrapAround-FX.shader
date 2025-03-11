ÿ‹Shader "MADFINGER/Characters/BRDFLit FX (Supports Backlight)" {
Properties {
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "grey" {}
 _BumpMap ("Normalmap", 2D) = "bump" {}
 _BRDFTex ("NdotL NdotH (RGB)", 2D) = "white" {}
 _NoiseTex ("Noise tex", 2D) = "white" {}
 _LightProbesLightingAmount ("Light probes lighting amount", Range(0,1)) = 0.9
 _FXColor ("FXColor", Color) = (0,0.97,0.89,1)
 _TimeOffs ("Time offs", Float) = 0
 _Duration ("Duration", Float) = 2
 _Invert ("Invert", Float) = 0
}
SubShader { 
 LOD 400
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardBase" "SHADOWSUPPORT"="true" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha
Program "vp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp float _TimeOffs;
uniform highp float _Duration;
uniform highp float _LightProbesLightingAmount;
uniform highp float _Invert;
uniform highp float _GlobalTime;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _BumpMap_ST;
varying highp vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump float xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_TEXCOORD5;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  lowp vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  lowp float tmpvar_9;
  highp vec3 SHLighting_10;
  mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = (tmpvar_11 * tmpvar_2);
  mediump vec3 tmpvar_13;
  mediump vec4 normal_14;
  normal_14 = tmpvar_12;
  highp float vC_15;
  mediump vec3 x3_16;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHAr, normal_14);
  x1_18.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAg, normal_14);
  x1_18.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAb, normal_14);
  x1_18.z = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22 = (normal_14.xyzz * normal_14.yzzx);
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHBr, tmpvar_22);
  x2_17.x = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBg, tmpvar_22);
  x2_17.y = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBb, tmpvar_22);
  x2_17.z = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = ((normal_14.x * normal_14.x) - (normal_14.y * normal_14.y));
  vC_15 = tmpvar_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (unity_SHC.xyz * vC_15);
  x3_16 = tmpvar_27;
  tmpvar_13 = ((x1_18 + x2_17) + x3_16);
  SHLighting_10 = tmpvar_13;
  highp float tmpvar_28;
  tmpvar_28 = clamp (((_TimeOffs + _GlobalTime) / _Duration), 0.0, 1.0);
  highp vec3 tmpvar_29;
  tmpvar_29 = clamp ((SHLighting_10 + vec3((1.0 - _LightProbesLightingAmount))), 0.0, 1.0);
  tmpvar_8 = tmpvar_29;
  highp float tmpvar_30;
  if ((_Invert > 0.0)) {
    tmpvar_30 = (1.0 - tmpvar_28);
  } else {
    tmpvar_30 = tmpvar_28;
  };
  tmpvar_9 = tmpvar_30;
  tmpvar_4 = tmpvar_8;
  tmpvar_5 = tmpvar_9;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  highp vec3 tmpvar_31;
  highp vec3 tmpvar_32;
  tmpvar_31 = tmpvar_1.xyz;
  tmpvar_32 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_33;
  tmpvar_33[0].x = tmpvar_31.x;
  tmpvar_33[0].y = tmpvar_32.x;
  tmpvar_33[0].z = tmpvar_2.x;
  tmpvar_33[1].x = tmpvar_31.y;
  tmpvar_33[1].y = tmpvar_32.y;
  tmpvar_33[1].z = tmpvar_2.y;
  tmpvar_33[2].x = tmpvar_31.z;
  tmpvar_33[2].y = tmpvar_32.z;
  tmpvar_33[2].z = tmpvar_2.z;
  highp vec3 tmpvar_34;
  tmpvar_34 = (tmpvar_33 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_6 = tmpvar_34;
  highp vec4 tmpvar_35;
  tmpvar_35.w = 1.0;
  tmpvar_35.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_36;
  tmpvar_36 = normalize((tmpvar_33 * (((_World2Object * tmpvar_35).xyz * unity_Scale.w) - _glesVertex.xyz)));
  tmpvar_7 = tmpvar_36;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD5 = tmpvar_7;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _BRDFTex;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTex;
uniform lowp vec4 _FXColor;
varying highp vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump float xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp float tmpvar_3;
  tmpvar_2 = xlv_TEXCOORD1;
  tmpvar_3 = xlv_TEXCOORD2;
  lowp vec4 tmpvar_4;
  highp vec2 P_5;
  P_5 = (xlv_TEXCOORD0.xy * 2.0);
  tmpvar_4 = texture2D (_NoiseTex, P_5);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  lowp float tmpvar_7;
  tmpvar_7 = (1.0 - clamp (((tmpvar_4.x - tmpvar_3) * 4.0), 0.0, 1.0));
  lowp float tmpvar_8;
  tmpvar_8 = (tmpvar_7 * tmpvar_7);
  lowp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_6.xyz * tmpvar_2);
  lowp vec3 tmpvar_10;
  tmpvar_10 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  lowp vec4 c_11;
  lowp vec2 tmpvar_12;
  tmpvar_12.x = ((dot (tmpvar_10, xlv_TEXCOORD3) * 0.5) + 0.5);
  tmpvar_12.y = dot (tmpvar_10, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD5)));
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_BRDFTex, tmpvar_12);
  c_11.xyz = ((tmpvar_9 * (tmpvar_13.xyz + (tmpvar_6.w * tmpvar_13.w))) * 2.0);
  c_11.w = float((tmpvar_4.x > tmpvar_3));
  c_1.w = c_11.w;
  c_1.xyz = (c_11.xyz + (tmpvar_9 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + (_FXColor * (tmpvar_8 * tmpvar_8)).xyz);
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp float _TimeOffs;
uniform highp float _Duration;
uniform highp float _LightProbesLightingAmount;
uniform highp float _Invert;
uniform highp float _GlobalTime;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _BumpMap_ST;
out highp vec4 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out mediump float xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
out lowp vec3 xlv_TEXCOORD5;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  lowp vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  lowp float tmpvar_9;
  highp vec3 SHLighting_10;
  mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = (tmpvar_11 * tmpvar_2);
  mediump vec3 tmpvar_13;
  mediump vec4 normal_14;
  normal_14 = tmpvar_12;
  highp float vC_15;
  mediump vec3 x3_16;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHAr, normal_14);
  x1_18.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAg, normal_14);
  x1_18.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAb, normal_14);
  x1_18.z = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22 = (normal_14.xyzz * normal_14.yzzx);
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHBr, tmpvar_22);
  x2_17.x = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBg, tmpvar_22);
  x2_17.y = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBb, tmpvar_22);
  x2_17.z = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = ((normal_14.x * normal_14.x) - (normal_14.y * normal_14.y));
  vC_15 = tmpvar_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (unity_SHC.xyz * vC_15);
  x3_16 = tmpvar_27;
  tmpvar_13 = ((x1_18 + x2_17) + x3_16);
  SHLighting_10 = tmpvar_13;
  highp float tmpvar_28;
  tmpvar_28 = clamp (((_TimeOffs + _GlobalTime) / _Duration), 0.0, 1.0);
  highp vec3 tmpvar_29;
  tmpvar_29 = clamp ((SHLighting_10 + vec3((1.0 - _LightProbesLightingAmount))), 0.0, 1.0);
  tmpvar_8 = tmpvar_29;
  highp float tmpvar_30;
  if ((_Invert > 0.0)) {
    tmpvar_30 = (1.0 - tmpvar_28);
  } else {
    tmpvar_30 = tmpvar_28;
  };
  tmpvar_9 = tmpvar_30;
  tmpvar_4 = tmpvar_8;
  tmpvar_5 = tmpvar_9;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  highp vec3 tmpvar_31;
  highp vec3 tmpvar_32;
  tmpvar_31 = tmpvar_1.xyz;
  tmpvar_32 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_33;
  tmpvar_33[0].x = tmpvar_31.x;
  tmpvar_33[0].y = tmpvar_32.x;
  tmpvar_33[0].z = tmpvar_2.x;
  tmpvar_33[1].x = tmpvar_31.y;
  tmpvar_33[1].y = tmpvar_32.y;
  tmpvar_33[1].z = tmpvar_2.y;
  tmpvar_33[2].x = tmpvar_31.z;
  tmpvar_33[2].y = tmpvar_32.z;
  tmpvar_33[2].z = tmpvar_2.z;
  highp vec3 tmpvar_34;
  tmpvar_34 = (tmpvar_33 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_6 = tmpvar_34;
  highp vec4 tmpvar_35;
  tmpvar_35.w = 1.0;
  tmpvar_35.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_36;
  tmpvar_36 = normalize((tmpvar_33 * (((_World2Object * tmpvar_35).xyz * unity_Scale.w) - _glesVertex.xyz)));
  tmpvar_7 = tmpvar_36;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD5 = tmpvar_7;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _BRDFTex;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTex;
uniform lowp vec4 _FXColor;
in highp vec4 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in mediump float xlv_TEXCOORD2;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
in lowp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp float tmpvar_3;
  tmpvar_2 = xlv_TEXCOORD1;
  tmpvar_3 = xlv_TEXCOORD2;
  lowp vec4 tmpvar_4;
  highp vec2 P_5;
  P_5 = (xlv_TEXCOORD0.xy * 2.0);
  tmpvar_4 = texture (_NoiseTex, P_5);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_MainTex, xlv_TEXCOORD0.xy);
  lowp float tmpvar_7;
  tmpvar_7 = (1.0 - clamp (((tmpvar_4.x - tmpvar_3) * 4.0), 0.0, 1.0));
  lowp float tmpvar_8;
  tmpvar_8 = (tmpvar_7 * tmpvar_7);
  lowp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_6.xyz * tmpvar_2);
  lowp vec3 tmpvar_10;
  tmpvar_10 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  lowp vec4 c_11;
  lowp vec2 tmpvar_12;
  tmpvar_12.x = ((dot (tmpvar_10, xlv_TEXCOORD3) * 0.5) + 0.5);
  tmpvar_12.y = dot (tmpvar_10, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD5)));
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture (_BRDFTex, tmpvar_12);
  c_11.xyz = ((tmpvar_9 * (tmpvar_13.xyz + (tmpvar_6.w * tmpvar_13.w))) * 2.0);
  c_11.w = float((tmpvar_4.x > tmpvar_3));
  c_1.w = c_11.w;
  c_1.xyz = (c_11.xyz + (tmpvar_9 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + (_FXColor * (tmpvar_8 * tmpvar_8)).xyz);
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp float _TimeOffs;
uniform highp float _Duration;
uniform highp float _LightProbesLightingAmount;
uniform highp float _Invert;
uniform highp float _GlobalTime;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _BumpMap_ST;
varying highp vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump float xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_TEXCOORD5;
varying highp vec4 xlv_TEXCOORD6;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  lowp vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  lowp float tmpvar_9;
  highp vec3 SHLighting_10;
  mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = (tmpvar_11 * tmpvar_2);
  mediump vec3 tmpvar_13;
  mediump vec4 normal_14;
  normal_14 = tmpvar_12;
  highp float vC_15;
  mediump vec3 x3_16;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHAr, normal_14);
  x1_18.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAg, normal_14);
  x1_18.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAb, normal_14);
  x1_18.z = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22 = (normal_14.xyzz * normal_14.yzzx);
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHBr, tmpvar_22);
  x2_17.x = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBg, tmpvar_22);
  x2_17.y = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBb, tmpvar_22);
  x2_17.z = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = ((normal_14.x * normal_14.x) - (normal_14.y * normal_14.y));
  vC_15 = tmpvar_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (unity_SHC.xyz * vC_15);
  x3_16 = tmpvar_27;
  tmpvar_13 = ((x1_18 + x2_17) + x3_16);
  SHLighting_10 = tmpvar_13;
  highp float tmpvar_28;
  tmpvar_28 = clamp (((_TimeOffs + _GlobalTime) / _Duration), 0.0, 1.0);
  highp vec3 tmpvar_29;
  tmpvar_29 = clamp ((SHLighting_10 + vec3((1.0 - _LightProbesLightingAmount))), 0.0, 1.0);
  tmpvar_8 = tmpvar_29;
  highp float tmpvar_30;
  if ((_Invert > 0.0)) {
    tmpvar_30 = (1.0 - tmpvar_28);
  } else {
    tmpvar_30 = tmpvar_28;
  };
  tmpvar_9 = tmpvar_30;
  tmpvar_4 = tmpvar_8;
  tmpvar_5 = tmpvar_9;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  highp vec3 tmpvar_31;
  highp vec3 tmpvar_32;
  tmpvar_31 = tmpvar_1.xyz;
  tmpvar_32 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_33;
  tmpvar_33[0].x = tmpvar_31.x;
  tmpvar_33[0].y = tmpvar_32.x;
  tmpvar_33[0].z = tmpvar_2.x;
  tmpvar_33[1].x = tmpvar_31.y;
  tmpvar_33[1].y = tmpvar_32.y;
  tmpvar_33[1].z = tmpvar_2.y;
  tmpvar_33[2].x = tmpvar_31.z;
  tmpvar_33[2].y = tmpvar_32.z;
  tmpvar_33[2].z = tmpvar_2.z;
  highp vec3 tmpvar_34;
  tmpvar_34 = (tmpvar_33 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_6 = tmpvar_34;
  highp vec4 tmpvar_35;
  tmpvar_35.w = 1.0;
  tmpvar_35.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_36;
  tmpvar_36 = normalize((tmpvar_33 * (((_World2Object * tmpvar_35).xyz * unity_Scale.w) - _glesVertex.xyz)));
  tmpvar_7 = tmpvar_36;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD5 = tmpvar_7;
  xlv_TEXCOORD6 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _BRDFTex;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTex;
uniform lowp vec4 _FXColor;
varying highp vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump float xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp float tmpvar_3;
  tmpvar_2 = xlv_TEXCOORD1;
  tmpvar_3 = xlv_TEXCOORD2;
  lowp vec4 tmpvar_4;
  highp vec2 P_5;
  P_5 = (xlv_TEXCOORD0.xy * 2.0);
  tmpvar_4 = texture2D (_NoiseTex, P_5);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  lowp float tmpvar_7;
  tmpvar_7 = (1.0 - clamp (((tmpvar_4.x - tmpvar_3) * 4.0), 0.0, 1.0));
  lowp float tmpvar_8;
  tmpvar_8 = (tmpvar_7 * tmpvar_7);
  lowp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_6.xyz * tmpvar_2);
  lowp vec3 tmpvar_10;
  tmpvar_10 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  lowp vec4 c_11;
  lowp vec2 tmpvar_12;
  tmpvar_12.x = ((dot (tmpvar_10, xlv_TEXCOORD3) * 0.5) + 0.5);
  tmpvar_12.y = dot (tmpvar_10, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD5)));
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_BRDFTex, tmpvar_12);
  c_11.xyz = ((tmpvar_9 * (tmpvar_13.xyz + (tmpvar_6.w * tmpvar_13.w))) * 2.0);
  c_11.w = float((tmpvar_4.x > tmpvar_3));
  c_1.w = c_11.w;
  c_1.xyz = (c_11.xyz + (tmpvar_9 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + (_FXColor * (tmpvar_8 * tmpvar_8)).xyz);
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp float _TimeOffs;
uniform highp float _Duration;
uniform highp float _LightProbesLightingAmount;
uniform highp float _Invert;
uniform highp float _GlobalTime;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _BumpMap_ST;
out highp vec4 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out mediump float xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
out lowp vec3 xlv_TEXCOORD5;
out highp vec4 xlv_TEXCOORD6;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  lowp vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  lowp float tmpvar_9;
  highp vec3 SHLighting_10;
  mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = (tmpvar_11 * tmpvar_2);
  mediump vec3 tmpvar_13;
  mediump vec4 normal_14;
  normal_14 = tmpvar_12;
  highp float vC_15;
  mediump vec3 x3_16;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHAr, normal_14);
  x1_18.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAg, normal_14);
  x1_18.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAb, normal_14);
  x1_18.z = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22 = (normal_14.xyzz * normal_14.yzzx);
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHBr, tmpvar_22);
  x2_17.x = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBg, tmpvar_22);
  x2_17.y = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBb, tmpvar_22);
  x2_17.z = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = ((normal_14.x * normal_14.x) - (normal_14.y * normal_14.y));
  vC_15 = tmpvar_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (unity_SHC.xyz * vC_15);
  x3_16 = tmpvar_27;
  tmpvar_13 = ((x1_18 + x2_17) + x3_16);
  SHLighting_10 = tmpvar_13;
  highp float tmpvar_28;
  tmpvar_28 = clamp (((_TimeOffs + _GlobalTime) / _Duration), 0.0, 1.0);
  highp vec3 tmpvar_29;
  tmpvar_29 = clamp ((SHLighting_10 + vec3((1.0 - _LightProbesLightingAmount))), 0.0, 1.0);
  tmpvar_8 = tmpvar_29;
  highp float tmpvar_30;
  if ((_Invert > 0.0)) {
    tmpvar_30 = (1.0 - tmpvar_28);
  } else {
    tmpvar_30 = tmpvar_28;
  };
  tmpvar_9 = tmpvar_30;
  tmpvar_4 = tmpvar_8;
  tmpvar_5 = tmpvar_9;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  highp vec3 tmpvar_31;
  highp vec3 tmpvar_32;
  tmpvar_31 = tmpvar_1.xyz;
  tmpvar_32 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_33;
  tmpvar_33[0].x = tmpvar_31.x;
  tmpvar_33[0].y = tmpvar_32.x;
  tmpvar_33[0].z = tmpvar_2.x;
  tmpvar_33[1].x = tmpvar_31.y;
  tmpvar_33[1].y = tmpvar_32.y;
  tmpvar_33[1].z = tmpvar_2.y;
  tmpvar_33[2].x = tmpvar_31.z;
  tmpvar_33[2].y = tmpvar_32.z;
  tmpvar_33[2].z = tmpvar_2.z;
  highp vec3 tmpvar_34;
  tmpvar_34 = (tmpvar_33 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_6 = tmpvar_34;
  highp vec4 tmpvar_35;
  tmpvar_35.w = 1.0;
  tmpvar_35.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_36;
  tmpvar_36 = normalize((tmpvar_33 * (((_World2Object * tmpvar_35).xyz * unity_Scale.w) - _glesVertex.xyz)));
  tmpvar_7 = tmpvar_36;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD5 = tmpvar_7;
  xlv_TEXCOORD6 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _BRDFTex;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTex;
uniform lowp vec4 _FXColor;
in highp vec4 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in mediump float xlv_TEXCOORD2;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
in lowp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp float tmpvar_3;
  tmpvar_2 = xlv_TEXCOORD1;
  tmpvar_3 = xlv_TEXCOORD2;
  lowp vec4 tmpvar_4;
  highp vec2 P_5;
  P_5 = (xlv_TEXCOORD0.xy * 2.0);
  tmpvar_4 = texture (_NoiseTex, P_5);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_MainTex, xlv_TEXCOORD0.xy);
  lowp float tmpvar_7;
  tmpvar_7 = (1.0 - clamp (((tmpvar_4.x - tmpvar_3) * 4.0), 0.0, 1.0));
  lowp float tmpvar_8;
  tmpvar_8 = (tmpvar_7 * tmpvar_7);
  lowp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_6.xyz * tmpvar_2);
  lowp vec3 tmpvar_10;
  tmpvar_10 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  lowp vec4 c_11;
  lowp vec2 tmpvar_12;
  tmpvar_12.x = ((dot (tmpvar_10, xlv_TEXCOORD3) * 0.5) + 0.5);
  tmpvar_12.y = dot (tmpvar_10, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD5)));
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture (_BRDFTex, tmpvar_12);
  c_11.xyz = ((tmpvar_9 * (tmpvar_13.xyz + (tmpvar_6.w * tmpvar_13.w))) * 2.0);
  c_11.w = float((tmpvar_4.x > tmpvar_3));
  c_1.w = c_11.w;
  c_1.xyz = (c_11.xyz + (tmpvar_9 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + (_FXColor * (tmpvar_8 * tmpvar_8)).xyz);
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp float _TimeOffs;
uniform highp float _Duration;
uniform highp float _LightProbesLightingAmount;
uniform highp float _Invert;
uniform highp float _GlobalTime;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _BumpMap_ST;
varying highp vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump float xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_TEXCOORD5;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  lowp vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  lowp vec3 tmpvar_9;
  lowp float tmpvar_10;
  highp vec3 SHLighting_11;
  mat3 tmpvar_12;
  tmpvar_12[0] = _Object2World[0].xyz;
  tmpvar_12[1] = _Object2World[1].xyz;
  tmpvar_12[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (tmpvar_12 * tmpvar_2);
  mediump vec3 tmpvar_14;
  mediump vec4 normal_15;
  normal_15 = tmpvar_13;
  highp float vC_16;
  mediump vec3 x3_17;
  mediump vec3 x2_18;
  mediump vec3 x1_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAr, normal_15);
  x1_19.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAg, normal_15);
  x1_19.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAb, normal_15);
  x1_19.z = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (normal_15.xyzz * normal_15.yzzx);
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBr, tmpvar_23);
  x2_18.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBg, tmpvar_23);
  x2_18.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBb, tmpvar_23);
  x2_18.z = tmpvar_26;
  mediump float tmpvar_27;
  tmpvar_27 = ((normal_15.x * normal_15.x) - (normal_15.y * normal_15.y));
  vC_16 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (unity_SHC.xyz * vC_16);
  x3_17 = tmpvar_28;
  tmpvar_14 = ((x1_19 + x2_18) + x3_17);
  SHLighting_11 = tmpvar_14;
  highp float tmpvar_29;
  tmpvar_29 = clamp (((_TimeOffs + _GlobalTime) / _Duration), 0.0, 1.0);
  highp vec3 tmpvar_30;
  tmpvar_30 = clamp ((SHLighting_11 + vec3((1.0 - _LightProbesLightingAmount))), 0.0, 1.0);
  tmpvar_9 = tmpvar_30;
  highp float tmpvar_31;
  if ((_Invert > 0.0)) {
    tmpvar_31 = (1.0 - tmpvar_29);
  } else {
    tmpvar_31 = tmpvar_29;
  };
  tmpvar_10 = tmpvar_31;
  tmpvar_4 = tmpvar_9;
  tmpvar_5 = tmpvar_10;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  mat3 tmpvar_32;
  tmpvar_32[0] = _Object2World[0].xyz;
  tmpvar_32[1] = _Object2World[1].xyz;
  tmpvar_32[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_33;
  tmpvar_33 = (tmpvar_32 * (tmpvar_2 * unity_Scale.w));
  highp vec3 tmpvar_34;
  highp vec3 tmpvar_35;
  tmpvar_34 = tmpvar_1.xyz;
  tmpvar_35 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_36;
  tmpvar_36[0].x = tmpvar_34.x;
  tmpvar_36[0].y = tmpvar_35.x;
  tmpvar_36[0].z = tmpvar_2.x;
  tmpvar_36[1].x = tmpvar_34.y;
  tmpvar_36[1].y = tmpvar_35.y;
  tmpvar_36[1].z = tmpvar_2.y;
  tmpvar_36[2].x = tmpvar_34.z;
  tmpvar_36[2].y = tmpvar_35.z;
  tmpvar_36[2].z = tmpvar_2.z;
  highp vec3 tmpvar_37;
  tmpvar_37 = (tmpvar_36 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_6 = tmpvar_37;
  highp vec4 tmpvar_38;
  tmpvar_38.w = 1.0;
  tmpvar_38.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_39;
  tmpvar_39 = normalize((tmpvar_36 * (((_World2Object * tmpvar_38).xyz * unity_Scale.w) - _glesVertex.xyz)));
  tmpvar_8 = tmpvar_39;
  highp vec3 tmpvar_40;
  tmpvar_40 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_41;
  tmpvar_41 = (unity_4LightPosX0 - tmpvar_40.x);
  highp vec4 tmpvar_42;
  tmpvar_42 = (unity_4LightPosY0 - tmpvar_40.y);
  highp vec4 tmpvar_43;
  tmpvar_43 = (unity_4LightPosZ0 - tmpvar_40.z);
  highp vec4 tmpvar_44;
  tmpvar_44 = (((tmpvar_41 * tmpvar_41) + (tmpvar_42 * tmpvar_42)) + (tmpvar_43 * tmpvar_43));
  highp vec4 tmpvar_45;
  tmpvar_45 = (max (vec4(0.0, 0.0, 0.0, 0.0), ((((tmpvar_41 * tmpvar_33.x) + (tmpvar_42 * tmpvar_33.y)) + (tmpvar_43 * tmpvar_33.z)) * inversesqrt(tmpvar_44))) * (1.0/((1.0 + (tmpvar_44 * unity_4LightAtten0)))));
  highp vec3 tmpvar_46;
  tmpvar_46 = ((((unity_LightColor[0].xyz * tmpvar_45.x) + (unity_LightColor[1].xyz * tmpvar_45.y)) + (unity_LightColor[2].xyz * tmpvar_45.z)) + (unity_LightColor[3].xyz * tmpvar_45.w));
  tmpvar_7 = tmpvar_46;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = tmpvar_7;
  xlv_TEXCOORD5 = tmpvar_8;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _BRDFTex;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTex;
uniform lowp vec4 _FXColor;
varying highp vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump float xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp float tmpvar_3;
  tmpvar_2 = xlv_TEXCOORD1;
  tmpvar_3 = xlv_TEXCOORD2;
  lowp vec4 tmpvar_4;
  highp vec2 P_5;
  P_5 = (xlv_TEXCOORD0.xy * 2.0);
  tmpvar_4 = texture2D (_NoiseTex, P_5);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  lowp float tmpvar_7;
  tmpvar_7 = (1.0 - clamp (((tmpvar_4.x - tmpvar_3) * 4.0), 0.0, 1.0));
  lowp float tmpvar_8;
  tmpvar_8 = (tmpvar_7 * tmpvar_7);
  lowp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_6.xyz * tmpvar_2);
  lowp vec3 tmpvar_10;
  tmpvar_10 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  lowp vec4 c_11;
  lowp vec2 tmpvar_12;
  tmpvar_12.x = ((dot (tmpvar_10, xlv_TEXCOORD3) * 0.5) + 0.5);
  tmpvar_12.y = dot (tmpvar_10, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD5)));
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_BRDFTex, tmpvar_12);
  c_11.xyz = ((tmpvar_9 * (tmpvar_13.xyz + (tmpvar_6.w * tmpvar_13.w))) * 2.0);
  c_11.w = float((tmpvar_4.x > tmpvar_3));
  c_1.w = c_11.w;
  c_1.xyz = (c_11.xyz + (tmpvar_9 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + (_FXColor * (tmpvar_8 * tmpvar_8)).xyz);
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp float _TimeOffs;
uniform highp float _Duration;
uniform highp float _LightProbesLightingAmount;
uniform highp float _Invert;
uniform highp float _GlobalTime;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _BumpMap_ST;
out highp vec4 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out mediump float xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
out lowp vec3 xlv_TEXCOORD5;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  lowp vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  lowp vec3 tmpvar_9;
  lowp float tmpvar_10;
  highp vec3 SHLighting_11;
  mat3 tmpvar_12;
  tmpvar_12[0] = _Object2World[0].xyz;
  tmpvar_12[1] = _Object2World[1].xyz;
  tmpvar_12[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (tmpvar_12 * tmpvar_2);
  mediump vec3 tmpvar_14;
  mediump vec4 normal_15;
  normal_15 = tmpvar_13;
  highp float vC_16;
  mediump vec3 x3_17;
  mediump vec3 x2_18;
  mediump vec3 x1_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAr, normal_15);
  x1_19.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAg, normal_15);
  x1_19.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAb, normal_15);
  x1_19.z = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (normal_15.xyzz * normal_15.yzzx);
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBr, tmpvar_23);
  x2_18.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBg, tmpvar_23);
  x2_18.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBb, tmpvar_23);
  x2_18.z = tmpvar_26;
  mediump float tmpvar_27;
  tmpvar_27 = ((normal_15.x * normal_15.x) - (normal_15.y * normal_15.y));
  vC_16 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (unity_SHC.xyz * vC_16);
  x3_17 = tmpvar_28;
  tmpvar_14 = ((x1_19 + x2_18) + x3_17);
  SHLighting_11 = tmpvar_14;
  highp float tmpvar_29;
  tmpvar_29 = clamp (((_TimeOffs + _GlobalTime) / _Duration), 0.0, 1.0);
  highp vec3 tmpvar_30;
  tmpvar_30 = clamp ((SHLighting_11 + vec3((1.0 - _LightProbesLightingAmount))), 0.0, 1.0);
  tmpvar_9 = tmpvar_30;
  highp float tmpvar_31;
  if ((_Invert > 0.0)) {
    tmpvar_31 = (1.0 - tmpvar_29);
  } else {
    tmpvar_31 = tmpvar_29;
  };
  tmpvar_10 = tmpvar_31;
  tmpvar_4 = tmpvar_9;
  tmpvar_5 = tmpvar_10;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  mat3 tmpvar_32;
  tmpvar_32[0] = _Object2World[0].xyz;
  tmpvar_32[1] = _Object2World[1].xyz;
  tmpvar_32[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_33;
  tmpvar_33 = (tmpvar_32 * (tmpvar_2 * unity_Scale.w));
  highp vec3 tmpvar_34;
  highp vec3 tmpvar_35;
  tmpvar_34 = tmpvar_1.xyz;
  tmpvar_35 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_36;
  tmpvar_36[0].x = tmpvar_34.x;
  tmpvar_36[0].y = tmpvar_35.x;
  tmpvar_36[0].z = tmpvar_2.x;
  tmpvar_36[1].x = tmpvar_34.y;
  tmpvar_36[1].y = tmpvar_35.y;
  tmpvar_36[1].z = tmpvar_2.y;
  tmpvar_36[2].x = tmpvar_34.z;
  tmpvar_36[2].y = tmpvar_35.z;
  tmpvar_36[2].z = tmpvar_2.z;
  highp vec3 tmpvar_37;
  tmpvar_37 = (tmpvar_36 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_6 = tmpvar_37;
  highp vec4 tmpvar_38;
  tmpvar_38.w = 1.0;
  tmpvar_38.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_39;
  tmpvar_39 = normalize((tmpvar_36 * (((_World2Object * tmpvar_38).xyz * unity_Scale.w) - _glesVertex.xyz)));
  tmpvar_8 = tmpvar_39;
  highp vec3 tmpvar_40;
  tmpvar_40 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_41;
  tmpvar_41 = (unity_4LightPosX0 - tmpvar_40.x);
  highp vec4 tmpvar_42;
  tmpvar_42 = (unity_4LightPosY0 - tmpvar_40.y);
  highp vec4 tmpvar_43;
  tmpvar_43 = (unity_4LightPosZ0 - tmpvar_40.z);
  highp vec4 tmpvar_44;
  tmpvar_44 = (((tmpvar_41 * tmpvar_41) + (tmpvar_42 * tmpvar_42)) + (tmpvar_43 * tmpvar_43));
  highp vec4 tmpvar_45;
  tmpvar_45 = (max (vec4(0.0, 0.0, 0.0, 0.0), ((((tmpvar_41 * tmpvar_33.x) + (tmpvar_42 * tmpvar_33.y)) + (tmpvar_43 * tmpvar_33.z)) * inversesqrt(tmpvar_44))) * (1.0/((1.0 + (tmpvar_44 * unity_4LightAtten0)))));
  highp vec3 tmpvar_46;
  tmpvar_46 = ((((unity_LightColor[0].xyz * tmpvar_45.x) + (unity_LightColor[1].xyz * tmpvar_45.y)) + (unity_LightColor[2].xyz * tmpvar_45.z)) + (unity_LightColor[3].xyz * tmpvar_45.w));
  tmpvar_7 = tmpvar_46;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = tmpvar_7;
  xlv_TEXCOORD5 = tmpvar_8;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _BRDFTex;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTex;
uniform lowp vec4 _FXColor;
in highp vec4 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in mediump float xlv_TEXCOORD2;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
in lowp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp float tmpvar_3;
  tmpvar_2 = xlv_TEXCOORD1;
  tmpvar_3 = xlv_TEXCOORD2;
  lowp vec4 tmpvar_4;
  highp vec2 P_5;
  P_5 = (xlv_TEXCOORD0.xy * 2.0);
  tmpvar_4 = texture (_NoiseTex, P_5);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_MainTex, xlv_TEXCOORD0.xy);
  lowp float tmpvar_7;
  tmpvar_7 = (1.0 - clamp (((tmpvar_4.x - tmpvar_3) * 4.0), 0.0, 1.0));
  lowp float tmpvar_8;
  tmpvar_8 = (tmpvar_7 * tmpvar_7);
  lowp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_6.xyz * tmpvar_2);
  lowp vec3 tmpvar_10;
  tmpvar_10 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  lowp vec4 c_11;
  lowp vec2 tmpvar_12;
  tmpvar_12.x = ((dot (tmpvar_10, xlv_TEXCOORD3) * 0.5) + 0.5);
  tmpvar_12.y = dot (tmpvar_10, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD5)));
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture (_BRDFTex, tmpvar_12);
  c_11.xyz = ((tmpvar_9 * (tmpvar_13.xyz + (tmpvar_6.w * tmpvar_13.w))) * 2.0);
  c_11.w = float((tmpvar_4.x > tmpvar_3));
  c_1.w = c_11.w;
  c_1.xyz = (c_11.xyz + (tmpvar_9 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + (_FXColor * (tmpvar_8 * tmpvar_8)).xyz);
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp float _TimeOffs;
uniform highp float _Duration;
uniform highp float _LightProbesLightingAmount;
uniform highp float _Invert;
uniform highp float _GlobalTime;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _BumpMap_ST;
varying highp vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump float xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_TEXCOORD5;
varying highp vec4 xlv_TEXCOORD6;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  lowp vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  lowp vec3 tmpvar_9;
  lowp float tmpvar_10;
  highp vec3 SHLighting_11;
  mat3 tmpvar_12;
  tmpvar_12[0] = _Object2World[0].xyz;
  tmpvar_12[1] = _Object2World[1].xyz;
  tmpvar_12[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (tmpvar_12 * tmpvar_2);
  mediump vec3 tmpvar_14;
  mediump vec4 normal_15;
  normal_15 = tmpvar_13;
  highp float vC_16;
  mediump vec3 x3_17;
  mediump vec3 x2_18;
  mediump vec3 x1_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAr, normal_15);
  x1_19.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAg, normal_15);
  x1_19.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAb, normal_15);
  x1_19.z = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (normal_15.xyzz * normal_15.yzzx);
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBr, tmpvar_23);
  x2_18.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBg, tmpvar_23);
  x2_18.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBb, tmpvar_23);
  x2_18.z = tmpvar_26;
  mediump float tmpvar_27;
  tmpvar_27 = ((normal_15.x * normal_15.x) - (normal_15.y * normal_15.y));
  vC_16 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (unity_SHC.xyz * vC_16);
  x3_17 = tmpvar_28;
  tmpvar_14 = ((x1_19 + x2_18) + x3_17);
  SHLighting_11 = tmpvar_14;
  highp float tmpvar_29;
  tmpvar_29 = clamp (((_TimeOffs + _GlobalTime) / _Duration), 0.0, 1.0);
  highp vec3 tmpvar_30;
  tmpvar_30 = clamp ((SHLighting_11 + vec3((1.0 - _LightProbesLightingAmount))), 0.0, 1.0);
  tmpvar_9 = tmpvar_30;
  highp float tmpvar_31;
  if ((_Invert > 0.0)) {
    tmpvar_31 = (1.0 - tmpvar_29);
  } else {
    tmpvar_31 = tmpvar_29;
  };
  tmpvar_10 = tmpvar_31;
  tmpvar_4 = tmpvar_9;
  tmpvar_5 = tmpvar_10;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  mat3 tmpvar_32;
  tmpvar_32[0] = _Object2World[0].xyz;
  tmpvar_32[1] = _Object2World[1].xyz;
  tmpvar_32[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_33;
  tmpvar_33 = (tmpvar_32 * (tmpvar_2 * unity_Scale.w));
  highp vec3 tmpvar_34;
  highp vec3 tmpvar_35;
  tmpvar_34 = tmpvar_1.xyz;
  tmpvar_35 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_36;
  tmpvar_36[0].x = tmpvar_34.x;
  tmpvar_36[0].y = tmpvar_35.x;
  tmpvar_36[0].z = tmpvar_2.x;
  tmpvar_36[1].x = tmpvar_34.y;
  tmpvar_36[1].y = tmpvar_35.y;
  tmpvar_36[1].z = tmpvar_2.y;
  tmpvar_36[2].x = tmpvar_34.z;
  tmpvar_36[2].y = tmpvar_35.z;
  tmpvar_36[2].z = tmpvar_2.z;
  highp vec3 tmpvar_37;
  tmpvar_37 = (tmpvar_36 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_6 = tmpvar_37;
  highp vec4 tmpvar_38;
  tmpvar_38.w = 1.0;
  tmpvar_38.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_39;
  tmpvar_39 = normalize((tmpvar_36 * (((_World2Object * tmpvar_38).xyz * unity_Scale.w) - _glesVertex.xyz)));
  tmpvar_8 = tmpvar_39;
  highp vec3 tmpvar_40;
  tmpvar_40 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_41;
  tmpvar_41 = (unity_4LightPosX0 - tmpvar_40.x);
  highp vec4 tmpvar_42;
  tmpvar_42 = (unity_4LightPosY0 - tmpvar_40.y);
  highp vec4 tmpvar_43;
  tmpvar_43 = (unity_4LightPosZ0 - tmpvar_40.z);
  highp vec4 tmpvar_44;
  tmpvar_44 = (((tmpvar_41 * tmpvar_41) + (tmpvar_42 * tmpvar_42)) + (tmpvar_43 * tmpvar_43));
  highp vec4 tmpvar_45;
  tmpvar_45 = (max (vec4(0.0, 0.0, 0.0, 0.0), ((((tmpvar_41 * tmpvar_33.x) + (tmpvar_42 * tmpvar_33.y)) + (tmpvar_43 * tmpvar_33.z)) * inversesqrt(tmpvar_44))) * (1.0/((1.0 + (tmpvar_44 * unity_4LightAtten0)))));
  highp vec3 tmpvar_46;
  tmpvar_46 = ((((unity_LightColor[0].xyz * tmpvar_45.x) + (unity_LightColor[1].xyz * tmpvar_45.y)) + (unity_LightColor[2].xyz * tmpvar_45.z)) + (unity_LightColor[3].xyz * tmpvar_45.w));
  tmpvar_7 = tmpvar_46;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = tmpvar_7;
  xlv_TEXCOORD5 = tmpvar_8;
  xlv_TEXCOORD6 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _BRDFTex;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTex;
uniform lowp vec4 _FXColor;
varying highp vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump float xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp float tmpvar_3;
  tmpvar_2 = xlv_TEXCOORD1;
  tmpvar_3 = xlv_TEXCOORD2;
  lowp vec4 tmpvar_4;
  highp vec2 P_5;
  P_5 = (xlv_TEXCOORD0.xy * 2.0);
  tmpvar_4 = texture2D (_NoiseTex, P_5);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  lowp float tmpvar_7;
  tmpvar_7 = (1.0 - clamp (((tmpvar_4.x - tmpvar_3) * 4.0), 0.0, 1.0));
  lowp float tmpvar_8;
  tmpvar_8 = (tmpvar_7 * tmpvar_7);
  lowp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_6.xyz * tmpvar_2);
  lowp vec3 tmpvar_10;
  tmpvar_10 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  lowp vec4 c_11;
  lowp vec2 tmpvar_12;
  tmpvar_12.x = ((dot (tmpvar_10, xlv_TEXCOORD3) * 0.5) + 0.5);
  tmpvar_12.y = dot (tmpvar_10, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD5)));
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_BRDFTex, tmpvar_12);
  c_11.xyz = ((tmpvar_9 * (tmpvar_13.xyz + (tmpvar_6.w * tmpvar_13.w))) * 2.0);
  c_11.w = float((tmpvar_4.x > tmpvar_3));
  c_1.w = c_11.w;
  c_1.xyz = (c_11.xyz + (tmpvar_9 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + (_FXColor * (tmpvar_8 * tmpvar_8)).xyz);
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp float _TimeOffs;
uniform highp float _Duration;
uniform highp float _LightProbesLightingAmount;
uniform highp float _Invert;
uniform highp float _GlobalTime;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _BumpMap_ST;
out highp vec4 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out mediump float xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
out lowp vec3 xlv_TEXCOORD5;
out highp vec4 xlv_TEXCOORD6;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  lowp vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  lowp vec3 tmpvar_9;
  lowp float tmpvar_10;
  highp vec3 SHLighting_11;
  mat3 tmpvar_12;
  tmpvar_12[0] = _Object2World[0].xyz;
  tmpvar_12[1] = _Object2World[1].xyz;
  tmpvar_12[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (tmpvar_12 * tmpvar_2);
  mediump vec3 tmpvar_14;
  mediump vec4 normal_15;
  normal_15 = tmpvar_13;
  highp float vC_16;
  mediump vec3 x3_17;
  mediump vec3 x2_18;
  mediump vec3 x1_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAr, normal_15);
  x1_19.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAg, normal_15);
  x1_19.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAb, normal_15);
  x1_19.z = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (normal_15.xyzz * normal_15.yzzx);
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBr, tmpvar_23);
  x2_18.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBg, tmpvar_23);
  x2_18.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBb, tmpvar_23);
  x2_18.z = tmpvar_26;
  mediump float tmpvar_27;
  tmpvar_27 = ((normal_15.x * normal_15.x) - (normal_15.y * normal_15.y));
  vC_16 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (unity_SHC.xyz * vC_16);
  x3_17 = tmpvar_28;
  tmpvar_14 = ((x1_19 + x2_18) + x3_17);
  SHLighting_11 = tmpvar_14;
  highp float tmpvar_29;
  tmpvar_29 = clamp (((_TimeOffs + _GlobalTime) / _Duration), 0.0, 1.0);
  highp vec3 tmpvar_30;
  tmpvar_30 = clamp ((SHLighting_11 + vec3((1.0 - _LightProbesLightingAmount))), 0.0, 1.0);
  tmpvar_9 = tmpvar_30;
  highp float tmpvar_31;
  if ((_Invert > 0.0)) {
    tmpvar_31 = (1.0 - tmpvar_29);
  } else {
    tmpvar_31 = tmpvar_29;
  };
  tmpvar_10 = tmpvar_31;
  tmpvar_4 = tmpvar_9;
  tmpvar_5 = tmpvar_10;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  mat3 tmpvar_32;
  tmpvar_32[0] = _Object2World[0].xyz;
  tmpvar_32[1] = _Object2World[1].xyz;
  tmpvar_32[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_33;
  tmpvar_33 = (tmpvar_32 * (tmpvar_2 * unity_Scale.w));
  highp vec3 tmpvar_34;
  highp vec3 tmpvar_35;
  tmpvar_34 = tmpvar_1.xyz;
  tmpvar_35 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_36;
  tmpvar_36[0].x = tmpvar_34.x;
  tmpvar_36[0].y = tmpvar_35.x;
  tmpvar_36[0].z = tmpvar_2.x;
  tmpvar_36[1].x = tmpvar_34.y;
  tmpvar_36[1].y = tmpvar_35.y;
  tmpvar_36[1].z = tmpvar_2.y;
  tmpvar_36[2].x = tmpvar_34.z;
  tmpvar_36[2].y = tmpvar_35.z;
  tmpvar_36[2].z = tmpvar_2.z;
  highp vec3 tmpvar_37;
  tmpvar_37 = (tmpvar_36 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_6 = tmpvar_37;
  highp vec4 tmpvar_38;
  tmpvar_38.w = 1.0;
  tmpvar_38.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_39;
  tmpvar_39 = normalize((tmpvar_36 * (((_World2Object * tmpvar_38).xyz * unity_Scale.w) - _glesVertex.xyz)));
  tmpvar_8 = tmpvar_39;
  highp vec3 tmpvar_40;
  tmpvar_40 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_41;
  tmpvar_41 = (unity_4LightPosX0 - tmpvar_40.x);
  highp vec4 tmpvar_42;
  tmpvar_42 = (unity_4LightPosY0 - tmpvar_40.y);
  highp vec4 tmpvar_43;
  tmpvar_43 = (unity_4LightPosZ0 - tmpvar_40.z);
  highp vec4 tmpvar_44;
  tmpvar_44 = (((tmpvar_41 * tmpvar_41) + (tmpvar_42 * tmpvar_42)) + (tmpvar_43 * tmpvar_43));
  highp vec4 tmpvar_45;
  tmpvar_45 = (max (vec4(0.0, 0.0, 0.0, 0.0), ((((tmpvar_41 * tmpvar_33.x) + (tmpvar_42 * tmpvar_33.y)) + (tmpvar_43 * tmpvar_33.z)) * inversesqrt(tmpvar_44))) * (1.0/((1.0 + (tmpvar_44 * unity_4LightAtten0)))));
  highp vec3 tmpvar_46;
  tmpvar_46 = ((((unity_LightColor[0].xyz * tmpvar_45.x) + (unity_LightColor[1].xyz * tmpvar_45.y)) + (unity_LightColor[2].xyz * tmpvar_45.z)) + (unity_LightColor[3].xyz * tmpvar_45.w));
  tmpvar_7 = tmpvar_46;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = tmpvar_7;
  xlv_TEXCOORD5 = tmpvar_8;
  xlv_TEXCOORD6 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _BRDFTex;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTex;
uniform lowp vec4 _FXColor;
in highp vec4 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in mediump float xlv_TEXCOORD2;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
in lowp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp float tmpvar_3;
  tmpvar_2 = xlv_TEXCOORD1;
  tmpvar_3 = xlv_TEXCOORD2;
  lowp vec4 tmpvar_4;
  highp vec2 P_5;
  P_5 = (xlv_TEXCOORD0.xy * 2.0);
  tmpvar_4 = texture (_NoiseTex, P_5);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_MainTex, xlv_TEXCOORD0.xy);
  lowp float tmpvar_7;
  tmpvar_7 = (1.0 - clamp (((tmpvar_4.x - tmpvar_3) * 4.0), 0.0, 1.0));
  lowp float tmpvar_8;
  tmpvar_8 = (tmpvar_7 * tmpvar_7);
  lowp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_6.xyz * tmpvar_2);
  lowp vec3 tmpvar_10;
  tmpvar_10 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  lowp vec4 c_11;
  lowp vec2 tmpvar_12;
  tmpvar_12.x = ((dot (tmpvar_10, xlv_TEXCOORD3) * 0.5) + 0.5);
  tmpvar_12.y = dot (tmpvar_10, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD5)));
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture (_BRDFTex, tmpvar_12);
  c_11.xyz = ((tmpvar_9 * (tmpvar_13.xyz + (tmpvar_6.w * tmpvar_13.w))) * 2.0);
  c_11.w = float((tmpvar_4.x > tmpvar_3));
  c_1.w = c_11.w;
  c_1.xyz = (c_11.xyz + (tmpvar_9 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + (_FXColor * (tmpvar_8 * tmpvar_8)).xyz);
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp float _TimeOffs;
uniform highp float _Duration;
uniform highp float _LightProbesLightingAmount;
uniform highp float _Invert;
uniform highp float _GlobalTime;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _BumpMap_ST;
varying highp vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump float xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_TEXCOORD5;
varying highp vec4 xlv_TEXCOORD6;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  lowp vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  lowp float tmpvar_9;
  highp vec3 SHLighting_10;
  mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = (tmpvar_11 * tmpvar_2);
  mediump vec3 tmpvar_13;
  mediump vec4 normal_14;
  normal_14 = tmpvar_12;
  highp float vC_15;
  mediump vec3 x3_16;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHAr, normal_14);
  x1_18.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAg, normal_14);
  x1_18.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAb, normal_14);
  x1_18.z = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22 = (normal_14.xyzz * normal_14.yzzx);
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHBr, tmpvar_22);
  x2_17.x = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBg, tmpvar_22);
  x2_17.y = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBb, tmpvar_22);
  x2_17.z = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = ((normal_14.x * normal_14.x) - (normal_14.y * normal_14.y));
  vC_15 = tmpvar_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (unity_SHC.xyz * vC_15);
  x3_16 = tmpvar_27;
  tmpvar_13 = ((x1_18 + x2_17) + x3_16);
  SHLighting_10 = tmpvar_13;
  highp float tmpvar_28;
  tmpvar_28 = clamp (((_TimeOffs + _GlobalTime) / _Duration), 0.0, 1.0);
  highp vec3 tmpvar_29;
  tmpvar_29 = clamp ((SHLighting_10 + vec3((1.0 - _LightProbesLightingAmount))), 0.0, 1.0);
  tmpvar_8 = tmpvar_29;
  highp float tmpvar_30;
  if ((_Invert > 0.0)) {
    tmpvar_30 = (1.0 - tmpvar_28);
  } else {
    tmpvar_30 = tmpvar_28;
  };
  tmpvar_9 = tmpvar_30;
  tmpvar_4 = tmpvar_8;
  tmpvar_5 = tmpvar_9;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  highp vec3 tmpvar_31;
  highp vec3 tmpvar_32;
  tmpvar_31 = tmpvar_1.xyz;
  tmpvar_32 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_33;
  tmpvar_33[0].x = tmpvar_31.x;
  tmpvar_33[0].y = tmpvar_32.x;
  tmpvar_33[0].z = tmpvar_2.x;
  tmpvar_33[1].x = tmpvar_31.y;
  tmpvar_33[1].y = tmpvar_32.y;
  tmpvar_33[1].z = tmpvar_2.y;
  tmpvar_33[2].x = tmpvar_31.z;
  tmpvar_33[2].y = tmpvar_32.z;
  tmpvar_33[2].z = tmpvar_2.z;
  highp vec3 tmpvar_34;
  tmpvar_34 = (tmpvar_33 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_6 = tmpvar_34;
  highp vec4 tmpvar_35;
  tmpvar_35.w = 1.0;
  tmpvar_35.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_36;
  tmpvar_36 = normalize((tmpvar_33 * (((_World2Object * tmpvar_35).xyz * unity_Scale.w) - _glesVertex.xyz)));
  tmpvar_7 = tmpvar_36;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD5 = tmpvar_7;
  xlv_TEXCOORD6 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
uniform sampler2D _BRDFTex;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTex;
uniform lowp vec4 _FXColor;
varying highp vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump float xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp float tmpvar_3;
  tmpvar_2 = xlv_TEXCOORD1;
  tmpvar_3 = xlv_TEXCOORD2;
  lowp vec4 tmpvar_4;
  highp vec2 P_5;
  P_5 = (xlv_TEXCOORD0.xy * 2.0);
  tmpvar_4 = texture2D (_NoiseTex, P_5);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  lowp float tmpvar_7;
  tmpvar_7 = (1.0 - clamp (((tmpvar_4.x - tmpvar_3) * 4.0), 0.0, 1.0));
  lowp float tmpvar_8;
  tmpvar_8 = (tmpvar_7 * tmpvar_7);
  lowp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_6.xyz * tmpvar_2);
  lowp vec3 tmpvar_10;
  tmpvar_10 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  lowp vec4 c_11;
  lowp vec2 tmpvar_12;
  tmpvar_12.x = ((dot (tmpvar_10, xlv_TEXCOORD3) * 0.5) + 0.5);
  tmpvar_12.y = dot (tmpvar_10, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD5)));
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_BRDFTex, tmpvar_12);
  c_11.xyz = ((tmpvar_9 * (tmpvar_13.xyz + (tmpvar_6.w * tmpvar_13.w))) * 2.0);
  c_11.w = float((tmpvar_4.x > tmpvar_3));
  c_1.w = c_11.w;
  c_1.xyz = (c_11.xyz + (tmpvar_9 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + (_FXColor * (tmpvar_8 * tmpvar_8)).xyz);
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp float _TimeOffs;
uniform highp float _Duration;
uniform highp float _LightProbesLightingAmount;
uniform highp float _Invert;
uniform highp float _GlobalTime;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _BumpMap_ST;
out highp vec4 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out mediump float xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
out lowp vec3 xlv_TEXCOORD5;
out highp vec4 xlv_TEXCOORD6;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  lowp vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  lowp float tmpvar_9;
  highp vec3 SHLighting_10;
  mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = (tmpvar_11 * tmpvar_2);
  mediump vec3 tmpvar_13;
  mediump vec4 normal_14;
  normal_14 = tmpvar_12;
  highp float vC_15;
  mediump vec3 x3_16;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHAr, normal_14);
  x1_18.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAg, normal_14);
  x1_18.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAb, normal_14);
  x1_18.z = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22 = (normal_14.xyzz * normal_14.yzzx);
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHBr, tmpvar_22);
  x2_17.x = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBg, tmpvar_22);
  x2_17.y = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBb, tmpvar_22);
  x2_17.z = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = ((normal_14.x * normal_14.x) - (normal_14.y * normal_14.y));
  vC_15 = tmpvar_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (unity_SHC.xyz * vC_15);
  x3_16 = tmpvar_27;
  tmpvar_13 = ((x1_18 + x2_17) + x3_16);
  SHLighting_10 = tmpvar_13;
  highp float tmpvar_28;
  tmpvar_28 = clamp (((_TimeOffs + _GlobalTime) / _Duration), 0.0, 1.0);
  highp vec3 tmpvar_29;
  tmpvar_29 = clamp ((SHLighting_10 + vec3((1.0 - _LightProbesLightingAmount))), 0.0, 1.0);
  tmpvar_8 = tmpvar_29;
  highp float tmpvar_30;
  if ((_Invert > 0.0)) {
    tmpvar_30 = (1.0 - tmpvar_28);
  } else {
    tmpvar_30 = tmpvar_28;
  };
  tmpvar_9 = tmpvar_30;
  tmpvar_4 = tmpvar_8;
  tmpvar_5 = tmpvar_9;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  highp vec3 tmpvar_31;
  highp vec3 tmpvar_32;
  tmpvar_31 = tmpvar_1.xyz;
  tmpvar_32 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_33;
  tmpvar_33[0].x = tmpvar_31.x;
  tmpvar_33[0].y = tmpvar_32.x;
  tmpvar_33[0].z = tmpvar_2.x;
  tmpvar_33[1].x = tmpvar_31.y;
  tmpvar_33[1].y = tmpvar_32.y;
  tmpvar_33[1].z = tmpvar_2.y;
  tmpvar_33[2].x = tmpvar_31.z;
  tmpvar_33[2].y = tmpvar_32.z;
  tmpvar_33[2].z = tmpvar_2.z;
  highp vec3 tmpvar_34;
  tmpvar_34 = (tmpvar_33 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_6 = tmpvar_34;
  highp vec4 tmpvar_35;
  tmpvar_35.w = 1.0;
  tmpvar_35.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_36;
  tmpvar_36 = normalize((tmpvar_33 * (((_World2Object * tmpvar_35).xyz * unity_Scale.w) - _glesVertex.xyz)));
  tmpvar_7 = tmpvar_36;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD5 = tmpvar_7;
  xlv_TEXCOORD6 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform highp vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _BRDFTex;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTex;
uniform lowp vec4 _FXColor;
in highp vec4 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in mediump float xlv_TEXCOORD2;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
in lowp vec3 xlv_TEXCOORD5;
in highp vec4 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp float tmpvar_3;
  tmpvar_2 = xlv_TEXCOORD1;
  tmpvar_3 = xlv_TEXCOORD2;
  lowp vec4 tmpvar_4;
  highp vec2 P_5;
  P_5 = (xlv_TEXCOORD0.xy * 2.0);
  tmpvar_4 = texture (_NoiseTex, P_5);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_MainTex, xlv_TEXCOORD0.xy);
  lowp float tmpvar_7;
  tmpvar_7 = (1.0 - clamp (((tmpvar_4.x - tmpvar_3) * 4.0), 0.0, 1.0));
  lowp float tmpvar_8;
  tmpvar_8 = (tmpvar_7 * tmpvar_7);
  lowp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_6.xyz * tmpvar_2);
  lowp vec3 tmpvar_10;
  tmpvar_10 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  lowp float shadow_11;
  mediump float tmpvar_12;
  tmpvar_12 = texture (_ShadowMapTexture, xlv_TEXCOORD6.xyz);
  shadow_11 = tmpvar_12;
  highp float tmpvar_13;
  tmpvar_13 = (_LightShadowData.x + (shadow_11 * (1.0 - _LightShadowData.x)));
  shadow_11 = tmpvar_13;
  lowp vec4 c_14;
  lowp vec2 tmpvar_15;
  tmpvar_15.x = ((dot (tmpvar_10, xlv_TEXCOORD3) * 0.5) + 0.5);
  tmpvar_15.y = dot (tmpvar_10, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD5)));
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture (_BRDFTex, tmpvar_15);
  c_14.xyz = ((tmpvar_9 * (tmpvar_16.xyz + (tmpvar_6.w * tmpvar_16.w))) * 2.0);
  c_14.w = float((tmpvar_4.x > tmpvar_3));
  c_1.w = c_14.w;
  c_1.xyz = (c_14.xyz + (tmpvar_9 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + (_FXColor * (tmpvar_8 * tmpvar_8)).xyz);
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp float _TimeOffs;
uniform highp float _Duration;
uniform highp float _LightProbesLightingAmount;
uniform highp float _Invert;
uniform highp float _GlobalTime;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _BumpMap_ST;
varying highp vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump float xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_TEXCOORD5;
varying highp vec4 xlv_TEXCOORD6;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  lowp vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  lowp vec3 tmpvar_9;
  lowp float tmpvar_10;
  highp vec3 SHLighting_11;
  mat3 tmpvar_12;
  tmpvar_12[0] = _Object2World[0].xyz;
  tmpvar_12[1] = _Object2World[1].xyz;
  tmpvar_12[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (tmpvar_12 * tmpvar_2);
  mediump vec3 tmpvar_14;
  mediump vec4 normal_15;
  normal_15 = tmpvar_13;
  highp float vC_16;
  mediump vec3 x3_17;
  mediump vec3 x2_18;
  mediump vec3 x1_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAr, normal_15);
  x1_19.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAg, normal_15);
  x1_19.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAb, normal_15);
  x1_19.z = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (normal_15.xyzz * normal_15.yzzx);
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBr, tmpvar_23);
  x2_18.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBg, tmpvar_23);
  x2_18.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBb, tmpvar_23);
  x2_18.z = tmpvar_26;
  mediump float tmpvar_27;
  tmpvar_27 = ((normal_15.x * normal_15.x) - (normal_15.y * normal_15.y));
  vC_16 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (unity_SHC.xyz * vC_16);
  x3_17 = tmpvar_28;
  tmpvar_14 = ((x1_19 + x2_18) + x3_17);
  SHLighting_11 = tmpvar_14;
  highp float tmpvar_29;
  tmpvar_29 = clamp (((_TimeOffs + _GlobalTime) / _Duration), 0.0, 1.0);
  highp vec3 tmpvar_30;
  tmpvar_30 = clamp ((SHLighting_11 + vec3((1.0 - _LightProbesLightingAmount))), 0.0, 1.0);
  tmpvar_9 = tmpvar_30;
  highp float tmpvar_31;
  if ((_Invert > 0.0)) {
    tmpvar_31 = (1.0 - tmpvar_29);
  } else {
    tmpvar_31 = tmpvar_29;
  };
  tmpvar_10 = tmpvar_31;
  tmpvar_4 = tmpvar_9;
  tmpvar_5 = tmpvar_10;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  mat3 tmpvar_32;
  tmpvar_32[0] = _Object2World[0].xyz;
  tmpvar_32[1] = _Object2World[1].xyz;
  tmpvar_32[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_33;
  tmpvar_33 = (tmpvar_32 * (tmpvar_2 * unity_Scale.w));
  highp vec3 tmpvar_34;
  highp vec3 tmpvar_35;
  tmpvar_34 = tmpvar_1.xyz;
  tmpvar_35 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_36;
  tmpvar_36[0].x = tmpvar_34.x;
  tmpvar_36[0].y = tmpvar_35.x;
  tmpvar_36[0].z = tmpvar_2.x;
  tmpvar_36[1].x = tmpvar_34.y;
  tmpvar_36[1].y = tmpvar_35.y;
  tmpvar_36[1].z = tmpvar_2.y;
  tmpvar_36[2].x = tmpvar_34.z;
  tmpvar_36[2].y = tmpvar_35.z;
  tmpvar_36[2].z = tmpvar_2.z;
  highp vec3 tmpvar_37;
  tmpvar_37 = (tmpvar_36 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_6 = tmpvar_37;
  highp vec4 tmpvar_38;
  tmpvar_38.w = 1.0;
  tmpvar_38.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_39;
  tmpvar_39 = normalize((tmpvar_36 * (((_World2Object * tmpvar_38).xyz * unity_Scale.w) - _glesVertex.xyz)));
  tmpvar_8 = tmpvar_39;
  highp vec3 tmpvar_40;
  tmpvar_40 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_41;
  tmpvar_41 = (unity_4LightPosX0 - tmpvar_40.x);
  highp vec4 tmpvar_42;
  tmpvar_42 = (unity_4LightPosY0 - tmpvar_40.y);
  highp vec4 tmpvar_43;
  tmpvar_43 = (unity_4LightPosZ0 - tmpvar_40.z);
  highp vec4 tmpvar_44;
  tmpvar_44 = (((tmpvar_41 * tmpvar_41) + (tmpvar_42 * tmpvar_42)) + (tmpvar_43 * tmpvar_43));
  highp vec4 tmpvar_45;
  tmpvar_45 = (max (vec4(0.0, 0.0, 0.0, 0.0), ((((tmpvar_41 * tmpvar_33.x) + (tmpvar_42 * tmpvar_33.y)) + (tmpvar_43 * tmpvar_33.z)) * inversesqrt(tmpvar_44))) * (1.0/((1.0 + (tmpvar_44 * unity_4LightAtten0)))));
  highp vec3 tmpvar_46;
  tmpvar_46 = ((((unity_LightColor[0].xyz * tmpvar_45.x) + (unity_LightColor[1].xyz * tmpvar_45.y)) + (unity_LightColor[2].xyz * tmpvar_45.z)) + (unity_LightColor[3].xyz * tmpvar_45.w));
  tmpvar_7 = tmpvar_46;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = tmpvar_7;
  xlv_TEXCOORD5 = tmpvar_8;
  xlv_TEXCOORD6 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
uniform sampler2D _BRDFTex;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTex;
uniform lowp vec4 _FXColor;
varying highp vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump float xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp float tmpvar_3;
  tmpvar_2 = xlv_TEXCOORD1;
  tmpvar_3 = xlv_TEXCOORD2;
  lowp vec4 tmpvar_4;
  highp vec2 P_5;
  P_5 = (xlv_TEXCOORD0.xy * 2.0);
  tmpvar_4 = texture2D (_NoiseTex, P_5);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  lowp float tmpvar_7;
  tmpvar_7 = (1.0 - clamp (((tmpvar_4.x - tmpvar_3) * 4.0), 0.0, 1.0));
  lowp float tmpvar_8;
  tmpvar_8 = (tmpvar_7 * tmpvar_7);
  lowp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_6.xyz * tmpvar_2);
  lowp vec3 tmpvar_10;
  tmpvar_10 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  lowp vec4 c_11;
  lowp vec2 tmpvar_12;
  tmpvar_12.x = ((dot (tmpvar_10, xlv_TEXCOORD3) * 0.5) + 0.5);
  tmpvar_12.y = dot (tmpvar_10, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD5)));
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_BRDFTex, tmpvar_12);
  c_11.xyz = ((tmpvar_9 * (tmpvar_13.xyz + (tmpvar_6.w * tmpvar_13.w))) * 2.0);
  c_11.w = float((tmpvar_4.x > tmpvar_3));
  c_1.w = c_11.w;
  c_1.xyz = (c_11.xyz + (tmpvar_9 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + (_FXColor * (tmpvar_8 * tmpvar_8)).xyz);
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp float _TimeOffs;
uniform highp float _Duration;
uniform highp float _LightProbesLightingAmount;
uniform highp float _Invert;
uniform highp float _GlobalTime;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _BumpMap_ST;
out highp vec4 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out mediump float xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
out lowp vec3 xlv_TEXCOORD5;
out highp vec4 xlv_TEXCOORD6;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  lowp vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  lowp vec3 tmpvar_9;
  lowp float tmpvar_10;
  highp vec3 SHLighting_11;
  mat3 tmpvar_12;
  tmpvar_12[0] = _Object2World[0].xyz;
  tmpvar_12[1] = _Object2World[1].xyz;
  tmpvar_12[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (tmpvar_12 * tmpvar_2);
  mediump vec3 tmpvar_14;
  mediump vec4 normal_15;
  normal_15 = tmpvar_13;
  highp float vC_16;
  mediump vec3 x3_17;
  mediump vec3 x2_18;
  mediump vec3 x1_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAr, normal_15);
  x1_19.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAg, normal_15);
  x1_19.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAb, normal_15);
  x1_19.z = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (normal_15.xyzz * normal_15.yzzx);
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBr, tmpvar_23);
  x2_18.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBg, tmpvar_23);
  x2_18.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBb, tmpvar_23);
  x2_18.z = tmpvar_26;
  mediump float tmpvar_27;
  tmpvar_27 = ((normal_15.x * normal_15.x) - (normal_15.y * normal_15.y));
  vC_16 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (unity_SHC.xyz * vC_16);
  x3_17 = tmpvar_28;
  tmpvar_14 = ((x1_19 + x2_18) + x3_17);
  SHLighting_11 = tmpvar_14;
  highp float tmpvar_29;
  tmpvar_29 = clamp (((_TimeOffs + _GlobalTime) / _Duration), 0.0, 1.0);
  highp vec3 tmpvar_30;
  tmpvar_30 = clamp ((SHLighting_11 + vec3((1.0 - _LightProbesLightingAmount))), 0.0, 1.0);
  tmpvar_9 = tmpvar_30;
  highp float tmpvar_31;
  if ((_Invert > 0.0)) {
    tmpvar_31 = (1.0 - tmpvar_29);
  } else {
    tmpvar_31 = tmpvar_29;
  };
  tmpvar_10 = tmpvar_31;
  tmpvar_4 = tmpvar_9;
  tmpvar_5 = tmpvar_10;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  mat3 tmpvar_32;
  tmpvar_32[0] = _Object2World[0].xyz;
  tmpvar_32[1] = _Object2World[1].xyz;
  tmpvar_32[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_33;
  tmpvar_33 = (tmpvar_32 * (tmpvar_2 * unity_Scale.w));
  highp vec3 tmpvar_34;
  highp vec3 tmpvar_35;
  tmpvar_34 = tmpvar_1.xyz;
  tmpvar_35 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_36;
  tmpvar_36[0].x = tmpvar_34.x;
  tmpvar_36[0].y = tmpvar_35.x;
  tmpvar_36[0].z = tmpvar_2.x;
  tmpvar_36[1].x = tmpvar_34.y;
  tmpvar_36[1].y = tmpvar_35.y;
  tmpvar_36[1].z = tmpvar_2.y;
  tmpvar_36[2].x = tmpvar_34.z;
  tmpvar_36[2].y = tmpvar_35.z;
  tmpvar_36[2].z = tmpvar_2.z;
  highp vec3 tmpvar_37;
  tmpvar_37 = (tmpvar_36 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_6 = tmpvar_37;
  highp vec4 tmpvar_38;
  tmpvar_38.w = 1.0;
  tmpvar_38.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_39;
  tmpvar_39 = normalize((tmpvar_36 * (((_World2Object * tmpvar_38).xyz * unity_Scale.w) - _glesVertex.xyz)));
  tmpvar_8 = tmpvar_39;
  highp vec3 tmpvar_40;
  tmpvar_40 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_41;
  tmpvar_41 = (unity_4LightPosX0 - tmpvar_40.x);
  highp vec4 tmpvar_42;
  tmpvar_42 = (unity_4LightPosY0 - tmpvar_40.y);
  highp vec4 tmpvar_43;
  tmpvar_43 = (unity_4LightPosZ0 - tmpvar_40.z);
  highp vec4 tmpvar_44;
  tmpvar_44 = (((tmpvar_41 * tmpvar_41) + (tmpvar_42 * tmpvar_42)) + (tmpvar_43 * tmpvar_43));
  highp vec4 tmpvar_45;
  tmpvar_45 = (max (vec4(0.0, 0.0, 0.0, 0.0), ((((tmpvar_41 * tmpvar_33.x) + (tmpvar_42 * tmpvar_33.y)) + (tmpvar_43 * tmpvar_33.z)) * inversesqrt(tmpvar_44))) * (1.0/((1.0 + (tmpvar_44 * unity_4LightAtten0)))));
  highp vec3 tmpvar_46;
  tmpvar_46 = ((((unity_LightColor[0].xyz * tmpvar_45.x) + (unity_LightColor[1].xyz * tmpvar_45.y)) + (unity_LightColor[2].xyz * tmpvar_45.z)) + (unity_LightColor[3].xyz * tmpvar_45.w));
  tmpvar_7 = tmpvar_46;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = tmpvar_7;
  xlv_TEXCOORD5 = tmpvar_8;
  xlv_TEXCOORD6 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform highp vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _BRDFTex;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTex;
uniform lowp vec4 _FXColor;
in highp vec4 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in mediump float xlv_TEXCOORD2;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
in lowp vec3 xlv_TEXCOORD5;
in highp vec4 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp float tmpvar_3;
  tmpvar_2 = xlv_TEXCOORD1;
  tmpvar_3 = xlv_TEXCOORD2;
  lowp vec4 tmpvar_4;
  highp vec2 P_5;
  P_5 = (xlv_TEXCOORD0.xy * 2.0);
  tmpvar_4 = texture (_NoiseTex, P_5);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_MainTex, xlv_TEXCOORD0.xy);
  lowp float tmpvar_7;
  tmpvar_7 = (1.0 - clamp (((tmpvar_4.x - tmpvar_3) * 4.0), 0.0, 1.0));
  lowp float tmpvar_8;
  tmpvar_8 = (tmpvar_7 * tmpvar_7);
  lowp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_6.xyz * tmpvar_2);
  lowp vec3 tmpvar_10;
  tmpvar_10 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  lowp float shadow_11;
  mediump float tmpvar_12;
  tmpvar_12 = texture (_ShadowMapTexture, xlv_TEXCOORD6.xyz);
  shadow_11 = tmpvar_12;
  highp float tmpvar_13;
  tmpvar_13 = (_LightShadowData.x + (shadow_11 * (1.0 - _LightShadowData.x)));
  shadow_11 = tmpvar_13;
  lowp vec4 c_14;
  lowp vec2 tmpvar_15;
  tmpvar_15.x = ((dot (tmpvar_10, xlv_TEXCOORD3) * 0.5) + 0.5);
  tmpvar_15.y = dot (tmpvar_10, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD5)));
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture (_BRDFTex, tmpvar_15);
  c_14.xyz = ((tmpvar_9 * (tmpvar_16.xyz + (tmpvar_6.w * tmpvar_16.w))) * 2.0);
  c_14.w = float((tmpvar_4.x > tmpvar_3));
  c_1.w = c_14.w;
  c_1.xyz = (c_14.xyz + (tmpvar_9 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + (_FXColor * (tmpvar_8 * tmpvar_8)).xyz);
  _glesFragData[0] = c_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3"
}
}
 }
}
Fallback "Diffuse"
}