ßaShader "MADFINGER/Characters/BRDFLit (Supports Backlight) - custom glossingess mask - zombie" {
Properties {
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "grey" {}
 _BumpMap ("Normalmap", 2D) = "bump" {}
 _BRDFTex ("NdotL NdotH (RGB)", 2D) = "white" {}
 _LightProbesLightingAmount ("Light probes lighting amount", Range(0,1)) = 0.9
 _SpecularStrength ("Specular strength weights", Vector) = (0,0,0,1)
 _Params ("x = open holes, y = FPV projection", Vector) = (0,0,0,0)
}
SubShader { 
 LOD 400
 Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp float _LightProbesLightingAmount;
uniform highp vec4 _Params;
uniform highp vec4 _ProjParams;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec4 xlv_COLOR;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 SHLighting_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  lowp vec4 tmpvar_7;
  highp mat4 projTM_8;
  projTM_8 = glstate_matrix_projection;
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 * tmpvar_2);
  highp vec3 tmpvar_11;
  tmpvar_11 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_12;
  if ((_Params.y > 0.0)) {
    tmpvar_12 = _ProjParams;
  } else {
    tmpvar_12 = vec4(1.0, 1.0, 1.0, 0.0);
  };
  highp vec3 tmpvar_13;
  if ((_glesColor.w < _Params.x)) {
    tmpvar_13 = vec3(0.0, 0.0, 0.0);
  } else {
    tmpvar_13 = tmpvar_11;
  };
  projTM_8[0] = glstate_matrix_projection[0]; projTM_8[0].x = (glstate_matrix_projection[0].x * tmpvar_12.x);
  projTM_8[1] = projTM_8[1]; projTM_8[1].y = (projTM_8[1].y * tmpvar_12.y);
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_13;
  highp vec4 tmpvar_15;
  tmpvar_15 = (projTM_8 * tmpvar_14);
  tmpvar_4.xyw = tmpvar_15.xyw;
  tmpvar_4.z = (tmpvar_15.z * tmpvar_12.z);
  tmpvar_4.z = (tmpvar_4.z + (tmpvar_12.w * tmpvar_15.w));
  tmpvar_4.z = max (tmpvar_4.z, 0.0);
  highp vec3 tmpvar_16;
  highp vec3 tmpvar_17;
  tmpvar_16 = tmpvar_1.xyz;
  tmpvar_17 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_18;
  tmpvar_18[0].x = tmpvar_16.x;
  tmpvar_18[0].y = tmpvar_17.x;
  tmpvar_18[0].z = tmpvar_2.x;
  tmpvar_18[1].x = tmpvar_16.y;
  tmpvar_18[1].y = tmpvar_17.y;
  tmpvar_18[1].z = tmpvar_2.y;
  tmpvar_18[2].x = tmpvar_16.z;
  tmpvar_18[2].y = tmpvar_17.z;
  tmpvar_18[2].z = tmpvar_2.z;
  mat3 tmpvar_19;
  tmpvar_19[0] = _World2Object[0].xyz;
  tmpvar_19[1] = _World2Object[1].xyz;
  tmpvar_19[2] = _World2Object[2].xyz;
  mediump vec3 res_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (((unity_SHAr.xyz * 0.3) + (unity_SHAg.xyz * 0.59)) + (unity_SHAb.xyz * 0.11));
  res_20 = tmpvar_21;
  mediump vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * res_20);
  highp vec3 tmpvar_23;
  tmpvar_23 = normalize((tmpvar_18 * tmpvar_22));
  tmpvar_5 = tmpvar_23;
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_25;
  tmpvar_25 = normalize((normalize((tmpvar_18 * (((_World2Object * tmpvar_24).xyz * unity_Scale.w) - _glesVertex.xyz))) + tmpvar_5));
  tmpvar_6 = tmpvar_25;
  highp vec4 tmpvar_26;
  tmpvar_26.w = 1.0;
  tmpvar_26.xyz = tmpvar_10;
  mediump vec3 tmpvar_27;
  mediump vec4 normal_28;
  normal_28 = tmpvar_26;
  highp float vC_29;
  mediump vec3 x3_30;
  mediump vec3 x2_31;
  mediump vec3 x1_32;
  highp float tmpvar_33;
  tmpvar_33 = dot (unity_SHAr, normal_28);
  x1_32.x = tmpvar_33;
  highp float tmpvar_34;
  tmpvar_34 = dot (unity_SHAg, normal_28);
  x1_32.y = tmpvar_34;
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHAb, normal_28);
  x1_32.z = tmpvar_35;
  mediump vec4 tmpvar_36;
  tmpvar_36 = (normal_28.xyzz * normal_28.yzzx);
  highp float tmpvar_37;
  tmpvar_37 = dot (unity_SHBr, tmpvar_36);
  x2_31.x = tmpvar_37;
  highp float tmpvar_38;
  tmpvar_38 = dot (unity_SHBg, tmpvar_36);
  x2_31.y = tmpvar_38;
  highp float tmpvar_39;
  tmpvar_39 = dot (unity_SHBb, tmpvar_36);
  x2_31.z = tmpvar_39;
  mediump float tmpvar_40;
  tmpvar_40 = ((normal_28.x * normal_28.x) - (normal_28.y * normal_28.y));
  vC_29 = tmpvar_40;
  highp vec3 tmpvar_41;
  tmpvar_41 = (unity_SHC.xyz * vC_29);
  x3_30 = tmpvar_41;
  tmpvar_27 = ((x1_32 + x2_31) + x3_30);
  SHLighting_3 = tmpvar_27;
  highp vec4 tmpvar_42;
  tmpvar_42 = clamp ((SHLighting_3 + vec3((1.0 - _LightProbesLightingAmount))), 0.0, 1.0).xyzz;
  tmpvar_7 = tmpvar_42;
  gl_Position = tmpvar_4;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_5;
  xlv_TEXCOORD2 = tmpvar_6;
  xlv_COLOR = tmpvar_7;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _BRDFTex;
uniform highp vec4 _SpecularStrength;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp float gloss_1;
  lowp vec4 c_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_2.w = tmpvar_3.w;
  lowp vec3 tmpvar_4;
  tmpvar_4 = ((texture2D (_BumpMap, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  highp float tmpvar_5;
  tmpvar_5 = dot (_SpecularStrength, tmpvar_3);
  gloss_1 = tmpvar_5;
  c_2.xyz = (tmpvar_3.xyz * xlv_COLOR.xyz);
  lowp vec2 tmpvar_6;
  tmpvar_6.x = ((dot (tmpvar_4, xlv_TEXCOORD1) * 0.5) + 0.5);
  tmpvar_6.y = dot (tmpvar_4, xlv_TEXCOORD2);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_BRDFTex, tmpvar_6);
  c_2.xyz = (c_2.xyz * ((tmpvar_7.xyz + (gloss_1 * tmpvar_7.w)) * 2.0));
  gl_FragData[0] = c_2;
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
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp float _LightProbesLightingAmount;
uniform highp vec4 _Params;
uniform highp vec4 _ProjParams;
out highp vec2 xlv_TEXCOORD0;
out lowp vec3 xlv_TEXCOORD1;
out lowp vec3 xlv_TEXCOORD2;
out lowp vec4 xlv_COLOR;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 SHLighting_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  lowp vec4 tmpvar_7;
  highp mat4 projTM_8;
  projTM_8 = glstate_matrix_projection;
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 * tmpvar_2);
  highp vec3 tmpvar_11;
  tmpvar_11 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_12;
  if ((_Params.y > 0.0)) {
    tmpvar_12 = _ProjParams;
  } else {
    tmpvar_12 = vec4(1.0, 1.0, 1.0, 0.0);
  };
  highp vec3 tmpvar_13;
  if ((_glesColor.w < _Params.x)) {
    tmpvar_13 = vec3(0.0, 0.0, 0.0);
  } else {
    tmpvar_13 = tmpvar_11;
  };
  projTM_8[0] = glstate_matrix_projection[0]; projTM_8[0].x = (glstate_matrix_projection[0].x * tmpvar_12.x);
  projTM_8[1] = projTM_8[1]; projTM_8[1].y = (projTM_8[1].y * tmpvar_12.y);
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_13;
  highp vec4 tmpvar_15;
  tmpvar_15 = (projTM_8 * tmpvar_14);
  tmpvar_4.xyw = tmpvar_15.xyw;
  tmpvar_4.z = (tmpvar_15.z * tmpvar_12.z);
  tmpvar_4.z = (tmpvar_4.z + (tmpvar_12.w * tmpvar_15.w));
  tmpvar_4.z = max (tmpvar_4.z, 0.0);
  highp vec3 tmpvar_16;
  highp vec3 tmpvar_17;
  tmpvar_16 = tmpvar_1.xyz;
  tmpvar_17 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_18;
  tmpvar_18[0].x = tmpvar_16.x;
  tmpvar_18[0].y = tmpvar_17.x;
  tmpvar_18[0].z = tmpvar_2.x;
  tmpvar_18[1].x = tmpvar_16.y;
  tmpvar_18[1].y = tmpvar_17.y;
  tmpvar_18[1].z = tmpvar_2.y;
  tmpvar_18[2].x = tmpvar_16.z;
  tmpvar_18[2].y = tmpvar_17.z;
  tmpvar_18[2].z = tmpvar_2.z;
  mat3 tmpvar_19;
  tmpvar_19[0] = _World2Object[0].xyz;
  tmpvar_19[1] = _World2Object[1].xyz;
  tmpvar_19[2] = _World2Object[2].xyz;
  mediump vec3 res_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (((unity_SHAr.xyz * 0.3) + (unity_SHAg.xyz * 0.59)) + (unity_SHAb.xyz * 0.11));
  res_20 = tmpvar_21;
  mediump vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * res_20);
  highp vec3 tmpvar_23;
  tmpvar_23 = normalize((tmpvar_18 * tmpvar_22));
  tmpvar_5 = tmpvar_23;
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_25;
  tmpvar_25 = normalize((normalize((tmpvar_18 * (((_World2Object * tmpvar_24).xyz * unity_Scale.w) - _glesVertex.xyz))) + tmpvar_5));
  tmpvar_6 = tmpvar_25;
  highp vec4 tmpvar_26;
  tmpvar_26.w = 1.0;
  tmpvar_26.xyz = tmpvar_10;
  mediump vec3 tmpvar_27;
  mediump vec4 normal_28;
  normal_28 = tmpvar_26;
  highp float vC_29;
  mediump vec3 x3_30;
  mediump vec3 x2_31;
  mediump vec3 x1_32;
  highp float tmpvar_33;
  tmpvar_33 = dot (unity_SHAr, normal_28);
  x1_32.x = tmpvar_33;
  highp float tmpvar_34;
  tmpvar_34 = dot (unity_SHAg, normal_28);
  x1_32.y = tmpvar_34;
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHAb, normal_28);
  x1_32.z = tmpvar_35;
  mediump vec4 tmpvar_36;
  tmpvar_36 = (normal_28.xyzz * normal_28.yzzx);
  highp float tmpvar_37;
  tmpvar_37 = dot (unity_SHBr, tmpvar_36);
  x2_31.x = tmpvar_37;
  highp float tmpvar_38;
  tmpvar_38 = dot (unity_SHBg, tmpvar_36);
  x2_31.y = tmpvar_38;
  highp float tmpvar_39;
  tmpvar_39 = dot (unity_SHBb, tmpvar_36);
  x2_31.z = tmpvar_39;
  mediump float tmpvar_40;
  tmpvar_40 = ((normal_28.x * normal_28.x) - (normal_28.y * normal_28.y));
  vC_29 = tmpvar_40;
  highp vec3 tmpvar_41;
  tmpvar_41 = (unity_SHC.xyz * vC_29);
  x3_30 = tmpvar_41;
  tmpvar_27 = ((x1_32 + x2_31) + x3_30);
  SHLighting_3 = tmpvar_27;
  highp vec4 tmpvar_42;
  tmpvar_42 = clamp ((SHLighting_3 + vec3((1.0 - _LightProbesLightingAmount))), 0.0, 1.0).xyzz;
  tmpvar_7 = tmpvar_42;
  gl_Position = tmpvar_4;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_5;
  xlv_TEXCOORD2 = tmpvar_6;
  xlv_COLOR = tmpvar_7;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _BRDFTex;
uniform highp vec4 _SpecularStrength;
in highp vec2 xlv_TEXCOORD0;
in lowp vec3 xlv_TEXCOORD1;
in lowp vec3 xlv_TEXCOORD2;
in lowp vec4 xlv_COLOR;
void main ()
{
  lowp float gloss_1;
  lowp vec4 c_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD0);
  c_2.w = tmpvar_3.w;
  lowp vec3 tmpvar_4;
  tmpvar_4 = ((texture (_BumpMap, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  highp float tmpvar_5;
  tmpvar_5 = dot (_SpecularStrength, tmpvar_3);
  gloss_1 = tmpvar_5;
  c_2.xyz = (tmpvar_3.xyz * xlv_COLOR.xyz);
  lowp vec2 tmpvar_6;
  tmpvar_6.x = ((dot (tmpvar_4, xlv_TEXCOORD1) * 0.5) + 0.5);
  tmpvar_6.y = dot (tmpvar_4, xlv_TEXCOORD2);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture (_BRDFTex, tmpvar_6);
  c_2.xyz = (c_2.xyz * ((tmpvar_7.xyz + (gloss_1 * tmpvar_7.w)) * 2.0));
  _glesFragData[0] = c_2;
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