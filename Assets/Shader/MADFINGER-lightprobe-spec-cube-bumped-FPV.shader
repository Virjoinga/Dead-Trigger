£ÀShader "MADFINGER/Environment/Bumped cubemap specular + Lightprobe FPV" {
Properties {
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
 _NormalsTex ("Normalmap", 2D) = "bump" {}
 _SpecCubeTex ("SpecCube", CUBE) = "black" {}
 _SpecularStrength ("Specular strength", Float) = 1
 _SHLightingScale ("LightProbe influence scale", Float) = 1
}
SubShader { 
 LOD 100
 Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry-5" "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry-5" "RenderType"="Opaque" }
Program "vp" {
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_LOW" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
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
uniform highp mat4 glstate_matrix_projection;
uniform highp float _SHLightingScale;
uniform highp vec4 _ProjParams;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_COLOR;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp mat4 projTM_2;
  highp vec4 tmpvar_3;
  lowp vec3 tmpvar_4;
  projTM_2 = glstate_matrix_projection;
  projTM_2[0] = glstate_matrix_projection[0]; projTM_2[0].x = (glstate_matrix_projection[0].x * _ProjParams.x);
  projTM_2[1] = projTM_2[1]; projTM_2[1].y = (projTM_2[1].y * _ProjParams.y);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_6;
  tmpvar_6 = (projTM_2 * tmpvar_5);
  tmpvar_3.xyw = tmpvar_6.xyw;
  tmpvar_3.z = (tmpvar_6.z * _ProjParams.z);
  tmpvar_3.z = (tmpvar_3.z + (_ProjParams.w * tmpvar_6.w));
  mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((tmpvar_7 * tmpvar_1.xyz));
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((tmpvar_9 * normalize(_glesNormal)));
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize((((tmpvar_10.yzx * tmpvar_8.zxy) - (tmpvar_10.zxy * tmpvar_8.yzx)) * _glesTANGENT.w));
  highp vec3 tmpvar_12;
  tmpvar_12.x = tmpvar_8.x;
  tmpvar_12.y = tmpvar_11.x;
  tmpvar_12.z = tmpvar_10.x;
  highp vec3 tmpvar_13;
  tmpvar_13.x = tmpvar_8.y;
  tmpvar_13.y = tmpvar_11.y;
  tmpvar_13.z = tmpvar_10.y;
  highp vec3 tmpvar_14;
  tmpvar_14.x = tmpvar_8.z;
  tmpvar_14.y = tmpvar_11.z;
  tmpvar_14.z = tmpvar_10.z;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  mediump vec3 tmpvar_16;
  mediump vec4 normal_17;
  normal_17 = tmpvar_15;
  highp float vC_18;
  mediump vec3 x3_19;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAr, normal_17);
  x1_21.x = tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHAg, normal_17);
  x1_21.y = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHAb, normal_17);
  x1_21.z = tmpvar_24;
  mediump vec4 tmpvar_25;
  tmpvar_25 = (normal_17.xyzz * normal_17.yzzx);
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBr, tmpvar_25);
  x2_20.x = tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = dot (unity_SHBg, tmpvar_25);
  x2_20.y = tmpvar_27;
  highp float tmpvar_28;
  tmpvar_28 = dot (unity_SHBb, tmpvar_25);
  x2_20.z = tmpvar_28;
  mediump float tmpvar_29;
  tmpvar_29 = ((normal_17.x * normal_17.x) - (normal_17.y * normal_17.y));
  vC_18 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (unity_SHC.xyz * vC_18);
  x3_19 = tmpvar_30;
  tmpvar_16 = ((x1_21 + x2_20) + x3_19);
  highp vec3 tmpvar_31;
  tmpvar_31 = (tmpvar_16 * _SHLightingScale);
  tmpvar_4 = tmpvar_31;
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  xlv_TEXCOORD2 = tmpvar_12;
  xlv_TEXCOORD3 = tmpvar_13;
  xlv_TEXCOORD4 = tmpvar_14;
  xlv_COLOR = tmpvar_4;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz * xlv_COLOR);
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_LOW" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
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
uniform highp mat4 glstate_matrix_projection;
uniform highp float _SHLightingScale;
uniform highp vec4 _ProjParams;
out highp vec2 xlv_TEXCOORD0;
out highp vec3 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec3 xlv_TEXCOORD4;
out lowp vec3 xlv_COLOR;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp mat4 projTM_2;
  highp vec4 tmpvar_3;
  lowp vec3 tmpvar_4;
  projTM_2 = glstate_matrix_projection;
  projTM_2[0] = glstate_matrix_projection[0]; projTM_2[0].x = (glstate_matrix_projection[0].x * _ProjParams.x);
  projTM_2[1] = projTM_2[1]; projTM_2[1].y = (projTM_2[1].y * _ProjParams.y);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_6;
  tmpvar_6 = (projTM_2 * tmpvar_5);
  tmpvar_3.xyw = tmpvar_6.xyw;
  tmpvar_3.z = (tmpvar_6.z * _ProjParams.z);
  tmpvar_3.z = (tmpvar_3.z + (_ProjParams.w * tmpvar_6.w));
  mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((tmpvar_7 * tmpvar_1.xyz));
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((tmpvar_9 * normalize(_glesNormal)));
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize((((tmpvar_10.yzx * tmpvar_8.zxy) - (tmpvar_10.zxy * tmpvar_8.yzx)) * _glesTANGENT.w));
  highp vec3 tmpvar_12;
  tmpvar_12.x = tmpvar_8.x;
  tmpvar_12.y = tmpvar_11.x;
  tmpvar_12.z = tmpvar_10.x;
  highp vec3 tmpvar_13;
  tmpvar_13.x = tmpvar_8.y;
  tmpvar_13.y = tmpvar_11.y;
  tmpvar_13.z = tmpvar_10.y;
  highp vec3 tmpvar_14;
  tmpvar_14.x = tmpvar_8.z;
  tmpvar_14.y = tmpvar_11.z;
  tmpvar_14.z = tmpvar_10.z;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  mediump vec3 tmpvar_16;
  mediump vec4 normal_17;
  normal_17 = tmpvar_15;
  highp float vC_18;
  mediump vec3 x3_19;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAr, normal_17);
  x1_21.x = tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHAg, normal_17);
  x1_21.y = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHAb, normal_17);
  x1_21.z = tmpvar_24;
  mediump vec4 tmpvar_25;
  tmpvar_25 = (normal_17.xyzz * normal_17.yzzx);
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBr, tmpvar_25);
  x2_20.x = tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = dot (unity_SHBg, tmpvar_25);
  x2_20.y = tmpvar_27;
  highp float tmpvar_28;
  tmpvar_28 = dot (unity_SHBb, tmpvar_25);
  x2_20.z = tmpvar_28;
  mediump float tmpvar_29;
  tmpvar_29 = ((normal_17.x * normal_17.x) - (normal_17.y * normal_17.y));
  vC_18 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (unity_SHC.xyz * vC_18);
  x3_19 = tmpvar_30;
  tmpvar_16 = ((x1_21 + x2_20) + x3_19);
  highp vec3 tmpvar_31;
  tmpvar_31 = (tmpvar_16 * _SHLightingScale);
  tmpvar_4 = tmpvar_31;
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  xlv_TEXCOORD2 = tmpvar_12;
  xlv_TEXCOORD3 = tmpvar_13;
  xlv_TEXCOORD4 = tmpvar_14;
  xlv_COLOR = tmpvar_4;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
in highp vec2 xlv_TEXCOORD0;
in lowp vec3 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz * xlv_COLOR);
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_MEDIUM" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
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
uniform highp mat4 glstate_matrix_projection;
uniform highp float _SHLightingScale;
uniform highp vec4 _ProjParams;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_COLOR;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp mat4 projTM_2;
  highp vec4 tmpvar_3;
  lowp vec3 tmpvar_4;
  projTM_2 = glstate_matrix_projection;
  projTM_2[0] = glstate_matrix_projection[0]; projTM_2[0].x = (glstate_matrix_projection[0].x * _ProjParams.x);
  projTM_2[1] = projTM_2[1]; projTM_2[1].y = (projTM_2[1].y * _ProjParams.y);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_6;
  tmpvar_6 = (projTM_2 * tmpvar_5);
  tmpvar_3.xyw = tmpvar_6.xyw;
  tmpvar_3.z = (tmpvar_6.z * _ProjParams.z);
  tmpvar_3.z = (tmpvar_3.z + (_ProjParams.w * tmpvar_6.w));
  mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((tmpvar_7 * tmpvar_1.xyz));
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((tmpvar_9 * normalize(_glesNormal)));
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize((((tmpvar_10.yzx * tmpvar_8.zxy) - (tmpvar_10.zxy * tmpvar_8.yzx)) * _glesTANGENT.w));
  highp vec3 tmpvar_12;
  tmpvar_12.x = tmpvar_8.x;
  tmpvar_12.y = tmpvar_11.x;
  tmpvar_12.z = tmpvar_10.x;
  highp vec3 tmpvar_13;
  tmpvar_13.x = tmpvar_8.y;
  tmpvar_13.y = tmpvar_11.y;
  tmpvar_13.z = tmpvar_10.y;
  highp vec3 tmpvar_14;
  tmpvar_14.x = tmpvar_8.z;
  tmpvar_14.y = tmpvar_11.z;
  tmpvar_14.z = tmpvar_10.z;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  mediump vec3 tmpvar_16;
  mediump vec4 normal_17;
  normal_17 = tmpvar_15;
  highp float vC_18;
  mediump vec3 x3_19;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAr, normal_17);
  x1_21.x = tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHAg, normal_17);
  x1_21.y = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHAb, normal_17);
  x1_21.z = tmpvar_24;
  mediump vec4 tmpvar_25;
  tmpvar_25 = (normal_17.xyzz * normal_17.yzzx);
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBr, tmpvar_25);
  x2_20.x = tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = dot (unity_SHBg, tmpvar_25);
  x2_20.y = tmpvar_27;
  highp float tmpvar_28;
  tmpvar_28 = dot (unity_SHBb, tmpvar_25);
  x2_20.z = tmpvar_28;
  mediump float tmpvar_29;
  tmpvar_29 = ((normal_17.x * normal_17.x) - (normal_17.y * normal_17.y));
  vC_18 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (unity_SHC.xyz * vC_18);
  x3_19 = tmpvar_30;
  tmpvar_16 = ((x1_21 + x2_20) + x3_19);
  highp vec3 tmpvar_31;
  tmpvar_31 = (tmpvar_16 * _SHLightingScale);
  tmpvar_4 = tmpvar_31;
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  xlv_TEXCOORD2 = tmpvar_12;
  xlv_TEXCOORD3 = tmpvar_13;
  xlv_TEXCOORD4 = tmpvar_14;
  xlv_COLOR = tmpvar_4;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _NormalsTex;
uniform lowp samplerCube _SpecCubeTex;
uniform highp float _SpecularStrength;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_COLOR;
void main ()
{
  lowp vec3 spec_1;
  mediump vec3 refl_2;
  mediump vec3 wnrm_3;
  mediump vec3 nrm_4;
  lowp vec4 c_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_5.w = tmpvar_6.w;
  lowp vec3 tmpvar_7;
  tmpvar_7 = ((texture2D (_NormalsTex, xlv_TEXCOORD0) * 2.0) - 1.0).xyz;
  nrm_4 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8.x = dot (nrm_4, xlv_TEXCOORD2);
  tmpvar_8.y = dot (nrm_4, xlv_TEXCOORD3);
  tmpvar_8.z = dot (nrm_4, xlv_TEXCOORD4);
  wnrm_3 = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD1 - (2.0 * (dot (wnrm_3, xlv_TEXCOORD1) * wnrm_3)));
  refl_2 = tmpvar_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = textureCube (_SpecCubeTex, refl_2);
  highp vec3 tmpvar_11;
  tmpvar_11 = ((tmpvar_10 * _SpecularStrength) * tmpvar_6.w).xyz;
  spec_1 = tmpvar_11;
  c_5.xyz = (tmpvar_6.xyz + spec_1);
  c_5.xyz = (c_5.xyz * xlv_COLOR);
  gl_FragData[0] = c_5;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_MEDIUM" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
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
uniform highp mat4 glstate_matrix_projection;
uniform highp float _SHLightingScale;
uniform highp vec4 _ProjParams;
out highp vec2 xlv_TEXCOORD0;
out highp vec3 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec3 xlv_TEXCOORD4;
out lowp vec3 xlv_COLOR;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp mat4 projTM_2;
  highp vec4 tmpvar_3;
  lowp vec3 tmpvar_4;
  projTM_2 = glstate_matrix_projection;
  projTM_2[0] = glstate_matrix_projection[0]; projTM_2[0].x = (glstate_matrix_projection[0].x * _ProjParams.x);
  projTM_2[1] = projTM_2[1]; projTM_2[1].y = (projTM_2[1].y * _ProjParams.y);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_6;
  tmpvar_6 = (projTM_2 * tmpvar_5);
  tmpvar_3.xyw = tmpvar_6.xyw;
  tmpvar_3.z = (tmpvar_6.z * _ProjParams.z);
  tmpvar_3.z = (tmpvar_3.z + (_ProjParams.w * tmpvar_6.w));
  mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((tmpvar_7 * tmpvar_1.xyz));
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((tmpvar_9 * normalize(_glesNormal)));
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize((((tmpvar_10.yzx * tmpvar_8.zxy) - (tmpvar_10.zxy * tmpvar_8.yzx)) * _glesTANGENT.w));
  highp vec3 tmpvar_12;
  tmpvar_12.x = tmpvar_8.x;
  tmpvar_12.y = tmpvar_11.x;
  tmpvar_12.z = tmpvar_10.x;
  highp vec3 tmpvar_13;
  tmpvar_13.x = tmpvar_8.y;
  tmpvar_13.y = tmpvar_11.y;
  tmpvar_13.z = tmpvar_10.y;
  highp vec3 tmpvar_14;
  tmpvar_14.x = tmpvar_8.z;
  tmpvar_14.y = tmpvar_11.z;
  tmpvar_14.z = tmpvar_10.z;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  mediump vec3 tmpvar_16;
  mediump vec4 normal_17;
  normal_17 = tmpvar_15;
  highp float vC_18;
  mediump vec3 x3_19;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAr, normal_17);
  x1_21.x = tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHAg, normal_17);
  x1_21.y = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHAb, normal_17);
  x1_21.z = tmpvar_24;
  mediump vec4 tmpvar_25;
  tmpvar_25 = (normal_17.xyzz * normal_17.yzzx);
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBr, tmpvar_25);
  x2_20.x = tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = dot (unity_SHBg, tmpvar_25);
  x2_20.y = tmpvar_27;
  highp float tmpvar_28;
  tmpvar_28 = dot (unity_SHBb, tmpvar_25);
  x2_20.z = tmpvar_28;
  mediump float tmpvar_29;
  tmpvar_29 = ((normal_17.x * normal_17.x) - (normal_17.y * normal_17.y));
  vC_18 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (unity_SHC.xyz * vC_18);
  x3_19 = tmpvar_30;
  tmpvar_16 = ((x1_21 + x2_20) + x3_19);
  highp vec3 tmpvar_31;
  tmpvar_31 = (tmpvar_16 * _SHLightingScale);
  tmpvar_4 = tmpvar_31;
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  xlv_TEXCOORD2 = tmpvar_12;
  xlv_TEXCOORD3 = tmpvar_13;
  xlv_TEXCOORD4 = tmpvar_14;
  xlv_COLOR = tmpvar_4;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _NormalsTex;
uniform lowp samplerCube _SpecCubeTex;
uniform highp float _SpecularStrength;
in highp vec2 xlv_TEXCOORD0;
in highp vec3 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
in highp vec3 xlv_TEXCOORD3;
in highp vec3 xlv_TEXCOORD4;
in lowp vec3 xlv_COLOR;
void main ()
{
  lowp vec3 spec_1;
  mediump vec3 refl_2;
  mediump vec3 wnrm_3;
  mediump vec3 nrm_4;
  lowp vec4 c_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_MainTex, xlv_TEXCOORD0);
  c_5.w = tmpvar_6.w;
  lowp vec3 tmpvar_7;
  tmpvar_7 = ((texture (_NormalsTex, xlv_TEXCOORD0) * 2.0) - 1.0).xyz;
  nrm_4 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8.x = dot (nrm_4, xlv_TEXCOORD2);
  tmpvar_8.y = dot (nrm_4, xlv_TEXCOORD3);
  tmpvar_8.z = dot (nrm_4, xlv_TEXCOORD4);
  wnrm_3 = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD1 - (2.0 * (dot (wnrm_3, xlv_TEXCOORD1) * wnrm_3)));
  refl_2 = tmpvar_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_SpecCubeTex, refl_2);
  highp vec3 tmpvar_11;
  tmpvar_11 = ((tmpvar_10 * _SpecularStrength) * tmpvar_6.w).xyz;
  spec_1 = tmpvar_11;
  c_5.xyz = (tmpvar_6.xyz + spec_1);
  c_5.xyz = (c_5.xyz * xlv_COLOR);
  _glesFragData[0] = c_5;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_HIGH" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
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
uniform highp mat4 glstate_matrix_projection;
uniform highp float _SHLightingScale;
uniform highp vec4 _ProjParams;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_COLOR;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp mat4 projTM_2;
  highp vec4 tmpvar_3;
  lowp vec3 tmpvar_4;
  projTM_2 = glstate_matrix_projection;
  projTM_2[0] = glstate_matrix_projection[0]; projTM_2[0].x = (glstate_matrix_projection[0].x * _ProjParams.x);
  projTM_2[1] = projTM_2[1]; projTM_2[1].y = (projTM_2[1].y * _ProjParams.y);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_6;
  tmpvar_6 = (projTM_2 * tmpvar_5);
  tmpvar_3.xyw = tmpvar_6.xyw;
  tmpvar_3.z = (tmpvar_6.z * _ProjParams.z);
  tmpvar_3.z = (tmpvar_3.z + (_ProjParams.w * tmpvar_6.w));
  mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((tmpvar_7 * tmpvar_1.xyz));
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((tmpvar_9 * normalize(_glesNormal)));
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize((((tmpvar_10.yzx * tmpvar_8.zxy) - (tmpvar_10.zxy * tmpvar_8.yzx)) * _glesTANGENT.w));
  highp vec3 tmpvar_12;
  tmpvar_12.x = tmpvar_8.x;
  tmpvar_12.y = tmpvar_11.x;
  tmpvar_12.z = tmpvar_10.x;
  highp vec3 tmpvar_13;
  tmpvar_13.x = tmpvar_8.y;
  tmpvar_13.y = tmpvar_11.y;
  tmpvar_13.z = tmpvar_10.y;
  highp vec3 tmpvar_14;
  tmpvar_14.x = tmpvar_8.z;
  tmpvar_14.y = tmpvar_11.z;
  tmpvar_14.z = tmpvar_10.z;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  mediump vec3 tmpvar_16;
  mediump vec4 normal_17;
  normal_17 = tmpvar_15;
  highp float vC_18;
  mediump vec3 x3_19;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAr, normal_17);
  x1_21.x = tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHAg, normal_17);
  x1_21.y = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHAb, normal_17);
  x1_21.z = tmpvar_24;
  mediump vec4 tmpvar_25;
  tmpvar_25 = (normal_17.xyzz * normal_17.yzzx);
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBr, tmpvar_25);
  x2_20.x = tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = dot (unity_SHBg, tmpvar_25);
  x2_20.y = tmpvar_27;
  highp float tmpvar_28;
  tmpvar_28 = dot (unity_SHBb, tmpvar_25);
  x2_20.z = tmpvar_28;
  mediump float tmpvar_29;
  tmpvar_29 = ((normal_17.x * normal_17.x) - (normal_17.y * normal_17.y));
  vC_18 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (unity_SHC.xyz * vC_18);
  x3_19 = tmpvar_30;
  tmpvar_16 = ((x1_21 + x2_20) + x3_19);
  highp vec3 tmpvar_31;
  tmpvar_31 = (tmpvar_16 * _SHLightingScale);
  tmpvar_4 = tmpvar_31;
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  xlv_TEXCOORD2 = tmpvar_12;
  xlv_TEXCOORD3 = tmpvar_13;
  xlv_TEXCOORD4 = tmpvar_14;
  xlv_COLOR = tmpvar_4;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _NormalsTex;
uniform lowp samplerCube _SpecCubeTex;
uniform highp float _SpecularStrength;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_COLOR;
void main ()
{
  lowp vec3 spec_1;
  mediump vec3 refl_2;
  mediump vec3 wnrm_3;
  mediump vec3 nrm_4;
  lowp vec4 c_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_5.w = tmpvar_6.w;
  lowp vec3 tmpvar_7;
  tmpvar_7 = ((texture2D (_NormalsTex, xlv_TEXCOORD0) * 2.0) - 1.0).xyz;
  nrm_4 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8.x = dot (nrm_4, xlv_TEXCOORD2);
  tmpvar_8.y = dot (nrm_4, xlv_TEXCOORD3);
  tmpvar_8.z = dot (nrm_4, xlv_TEXCOORD4);
  wnrm_3 = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD1 - (2.0 * (dot (wnrm_3, xlv_TEXCOORD1) * wnrm_3)));
  refl_2 = tmpvar_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = textureCube (_SpecCubeTex, refl_2);
  highp vec3 tmpvar_11;
  tmpvar_11 = ((tmpvar_10 * _SpecularStrength) * tmpvar_6.w).xyz;
  spec_1 = tmpvar_11;
  c_5.xyz = (tmpvar_6.xyz + spec_1);
  c_5.xyz = (c_5.xyz * xlv_COLOR);
  gl_FragData[0] = c_5;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_HIGH" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
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
uniform highp mat4 glstate_matrix_projection;
uniform highp float _SHLightingScale;
uniform highp vec4 _ProjParams;
out highp vec2 xlv_TEXCOORD0;
out highp vec3 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec3 xlv_TEXCOORD4;
out lowp vec3 xlv_COLOR;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp mat4 projTM_2;
  highp vec4 tmpvar_3;
  lowp vec3 tmpvar_4;
  projTM_2 = glstate_matrix_projection;
  projTM_2[0] = glstate_matrix_projection[0]; projTM_2[0].x = (glstate_matrix_projection[0].x * _ProjParams.x);
  projTM_2[1] = projTM_2[1]; projTM_2[1].y = (projTM_2[1].y * _ProjParams.y);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_6;
  tmpvar_6 = (projTM_2 * tmpvar_5);
  tmpvar_3.xyw = tmpvar_6.xyw;
  tmpvar_3.z = (tmpvar_6.z * _ProjParams.z);
  tmpvar_3.z = (tmpvar_3.z + (_ProjParams.w * tmpvar_6.w));
  mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((tmpvar_7 * tmpvar_1.xyz));
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((tmpvar_9 * normalize(_glesNormal)));
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize((((tmpvar_10.yzx * tmpvar_8.zxy) - (tmpvar_10.zxy * tmpvar_8.yzx)) * _glesTANGENT.w));
  highp vec3 tmpvar_12;
  tmpvar_12.x = tmpvar_8.x;
  tmpvar_12.y = tmpvar_11.x;
  tmpvar_12.z = tmpvar_10.x;
  highp vec3 tmpvar_13;
  tmpvar_13.x = tmpvar_8.y;
  tmpvar_13.y = tmpvar_11.y;
  tmpvar_13.z = tmpvar_10.y;
  highp vec3 tmpvar_14;
  tmpvar_14.x = tmpvar_8.z;
  tmpvar_14.y = tmpvar_11.z;
  tmpvar_14.z = tmpvar_10.z;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  mediump vec3 tmpvar_16;
  mediump vec4 normal_17;
  normal_17 = tmpvar_15;
  highp float vC_18;
  mediump vec3 x3_19;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAr, normal_17);
  x1_21.x = tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHAg, normal_17);
  x1_21.y = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHAb, normal_17);
  x1_21.z = tmpvar_24;
  mediump vec4 tmpvar_25;
  tmpvar_25 = (normal_17.xyzz * normal_17.yzzx);
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBr, tmpvar_25);
  x2_20.x = tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = dot (unity_SHBg, tmpvar_25);
  x2_20.y = tmpvar_27;
  highp float tmpvar_28;
  tmpvar_28 = dot (unity_SHBb, tmpvar_25);
  x2_20.z = tmpvar_28;
  mediump float tmpvar_29;
  tmpvar_29 = ((normal_17.x * normal_17.x) - (normal_17.y * normal_17.y));
  vC_18 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (unity_SHC.xyz * vC_18);
  x3_19 = tmpvar_30;
  tmpvar_16 = ((x1_21 + x2_20) + x3_19);
  highp vec3 tmpvar_31;
  tmpvar_31 = (tmpvar_16 * _SHLightingScale);
  tmpvar_4 = tmpvar_31;
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  xlv_TEXCOORD2 = tmpvar_12;
  xlv_TEXCOORD3 = tmpvar_13;
  xlv_TEXCOORD4 = tmpvar_14;
  xlv_COLOR = tmpvar_4;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _NormalsTex;
uniform lowp samplerCube _SpecCubeTex;
uniform highp float _SpecularStrength;
in highp vec2 xlv_TEXCOORD0;
in highp vec3 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
in highp vec3 xlv_TEXCOORD3;
in highp vec3 xlv_TEXCOORD4;
in lowp vec3 xlv_COLOR;
void main ()
{
  lowp vec3 spec_1;
  mediump vec3 refl_2;
  mediump vec3 wnrm_3;
  mediump vec3 nrm_4;
  lowp vec4 c_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_MainTex, xlv_TEXCOORD0);
  c_5.w = tmpvar_6.w;
  lowp vec3 tmpvar_7;
  tmpvar_7 = ((texture (_NormalsTex, xlv_TEXCOORD0) * 2.0) - 1.0).xyz;
  nrm_4 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8.x = dot (nrm_4, xlv_TEXCOORD2);
  tmpvar_8.y = dot (nrm_4, xlv_TEXCOORD3);
  tmpvar_8.z = dot (nrm_4, xlv_TEXCOORD4);
  wnrm_3 = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD1 - (2.0 * (dot (wnrm_3, xlv_TEXCOORD1) * wnrm_3)));
  refl_2 = tmpvar_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_SpecCubeTex, refl_2);
  highp vec3 tmpvar_11;
  tmpvar_11 = ((tmpvar_10 * _SpecularStrength) * tmpvar_6.w).xyz;
  spec_1 = tmpvar_11;
  c_5.xyz = (tmpvar_6.xyz + spec_1);
  c_5.xyz = (c_5.xyz * xlv_COLOR);
  _glesFragData[0] = c_5;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_VERY_HIGH" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
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
uniform highp mat4 glstate_matrix_projection;
uniform highp float _SHLightingScale;
uniform highp vec4 _ProjParams;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_COLOR;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp mat4 projTM_2;
  highp vec4 tmpvar_3;
  lowp vec3 tmpvar_4;
  projTM_2 = glstate_matrix_projection;
  projTM_2[0] = glstate_matrix_projection[0]; projTM_2[0].x = (glstate_matrix_projection[0].x * _ProjParams.x);
  projTM_2[1] = projTM_2[1]; projTM_2[1].y = (projTM_2[1].y * _ProjParams.y);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_6;
  tmpvar_6 = (projTM_2 * tmpvar_5);
  tmpvar_3.xyw = tmpvar_6.xyw;
  tmpvar_3.z = (tmpvar_6.z * _ProjParams.z);
  tmpvar_3.z = (tmpvar_3.z + (_ProjParams.w * tmpvar_6.w));
  mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((tmpvar_7 * tmpvar_1.xyz));
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((tmpvar_9 * normalize(_glesNormal)));
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize((((tmpvar_10.yzx * tmpvar_8.zxy) - (tmpvar_10.zxy * tmpvar_8.yzx)) * _glesTANGENT.w));
  highp vec3 tmpvar_12;
  tmpvar_12.x = tmpvar_8.x;
  tmpvar_12.y = tmpvar_11.x;
  tmpvar_12.z = tmpvar_10.x;
  highp vec3 tmpvar_13;
  tmpvar_13.x = tmpvar_8.y;
  tmpvar_13.y = tmpvar_11.y;
  tmpvar_13.z = tmpvar_10.y;
  highp vec3 tmpvar_14;
  tmpvar_14.x = tmpvar_8.z;
  tmpvar_14.y = tmpvar_11.z;
  tmpvar_14.z = tmpvar_10.z;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  mediump vec3 tmpvar_16;
  mediump vec4 normal_17;
  normal_17 = tmpvar_15;
  highp float vC_18;
  mediump vec3 x3_19;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAr, normal_17);
  x1_21.x = tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHAg, normal_17);
  x1_21.y = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHAb, normal_17);
  x1_21.z = tmpvar_24;
  mediump vec4 tmpvar_25;
  tmpvar_25 = (normal_17.xyzz * normal_17.yzzx);
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBr, tmpvar_25);
  x2_20.x = tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = dot (unity_SHBg, tmpvar_25);
  x2_20.y = tmpvar_27;
  highp float tmpvar_28;
  tmpvar_28 = dot (unity_SHBb, tmpvar_25);
  x2_20.z = tmpvar_28;
  mediump float tmpvar_29;
  tmpvar_29 = ((normal_17.x * normal_17.x) - (normal_17.y * normal_17.y));
  vC_18 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (unity_SHC.xyz * vC_18);
  x3_19 = tmpvar_30;
  tmpvar_16 = ((x1_21 + x2_20) + x3_19);
  highp vec3 tmpvar_31;
  tmpvar_31 = (tmpvar_16 * _SHLightingScale);
  tmpvar_4 = tmpvar_31;
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  xlv_TEXCOORD2 = tmpvar_12;
  xlv_TEXCOORD3 = tmpvar_13;
  xlv_TEXCOORD4 = tmpvar_14;
  xlv_COLOR = tmpvar_4;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _NormalsTex;
uniform lowp samplerCube _SpecCubeTex;
uniform highp float _SpecularStrength;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying lowp vec3 xlv_COLOR;
void main ()
{
  lowp vec3 spec_1;
  mediump vec3 refl_2;
  mediump vec3 wnrm_3;
  mediump vec3 nrm_4;
  lowp vec4 c_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_5.w = tmpvar_6.w;
  lowp vec3 tmpvar_7;
  tmpvar_7 = ((texture2D (_NormalsTex, xlv_TEXCOORD0) * 2.0) - 1.0).xyz;
  nrm_4 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8.x = dot (nrm_4, xlv_TEXCOORD2);
  tmpvar_8.y = dot (nrm_4, xlv_TEXCOORD3);
  tmpvar_8.z = dot (nrm_4, xlv_TEXCOORD4);
  wnrm_3 = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD1 - (2.0 * (dot (wnrm_3, xlv_TEXCOORD1) * wnrm_3)));
  refl_2 = tmpvar_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = textureCube (_SpecCubeTex, refl_2);
  highp vec3 tmpvar_11;
  tmpvar_11 = ((tmpvar_10 * _SpecularStrength) * tmpvar_6.w).xyz;
  spec_1 = tmpvar_11;
  c_5.xyz = (tmpvar_6.xyz + spec_1);
  c_5.xyz = (c_5.xyz * xlv_COLOR);
  gl_FragData[0] = c_5;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_VERY_HIGH" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
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
uniform highp mat4 glstate_matrix_projection;
uniform highp float _SHLightingScale;
uniform highp vec4 _ProjParams;
out highp vec2 xlv_TEXCOORD0;
out highp vec3 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec3 xlv_TEXCOORD4;
out lowp vec3 xlv_COLOR;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp mat4 projTM_2;
  highp vec4 tmpvar_3;
  lowp vec3 tmpvar_4;
  projTM_2 = glstate_matrix_projection;
  projTM_2[0] = glstate_matrix_projection[0]; projTM_2[0].x = (glstate_matrix_projection[0].x * _ProjParams.x);
  projTM_2[1] = projTM_2[1]; projTM_2[1].y = (projTM_2[1].y * _ProjParams.y);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_6;
  tmpvar_6 = (projTM_2 * tmpvar_5);
  tmpvar_3.xyw = tmpvar_6.xyw;
  tmpvar_3.z = (tmpvar_6.z * _ProjParams.z);
  tmpvar_3.z = (tmpvar_3.z + (_ProjParams.w * tmpvar_6.w));
  mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((tmpvar_7 * tmpvar_1.xyz));
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((tmpvar_9 * normalize(_glesNormal)));
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize((((tmpvar_10.yzx * tmpvar_8.zxy) - (tmpvar_10.zxy * tmpvar_8.yzx)) * _glesTANGENT.w));
  highp vec3 tmpvar_12;
  tmpvar_12.x = tmpvar_8.x;
  tmpvar_12.y = tmpvar_11.x;
  tmpvar_12.z = tmpvar_10.x;
  highp vec3 tmpvar_13;
  tmpvar_13.x = tmpvar_8.y;
  tmpvar_13.y = tmpvar_11.y;
  tmpvar_13.z = tmpvar_10.y;
  highp vec3 tmpvar_14;
  tmpvar_14.x = tmpvar_8.z;
  tmpvar_14.y = tmpvar_11.z;
  tmpvar_14.z = tmpvar_10.z;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  mediump vec3 tmpvar_16;
  mediump vec4 normal_17;
  normal_17 = tmpvar_15;
  highp float vC_18;
  mediump vec3 x3_19;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAr, normal_17);
  x1_21.x = tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHAg, normal_17);
  x1_21.y = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHAb, normal_17);
  x1_21.z = tmpvar_24;
  mediump vec4 tmpvar_25;
  tmpvar_25 = (normal_17.xyzz * normal_17.yzzx);
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBr, tmpvar_25);
  x2_20.x = tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = dot (unity_SHBg, tmpvar_25);
  x2_20.y = tmpvar_27;
  highp float tmpvar_28;
  tmpvar_28 = dot (unity_SHBb, tmpvar_25);
  x2_20.z = tmpvar_28;
  mediump float tmpvar_29;
  tmpvar_29 = ((normal_17.x * normal_17.x) - (normal_17.y * normal_17.y));
  vC_18 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (unity_SHC.xyz * vC_18);
  x3_19 = tmpvar_30;
  tmpvar_16 = ((x1_21 + x2_20) + x3_19);
  highp vec3 tmpvar_31;
  tmpvar_31 = (tmpvar_16 * _SHLightingScale);
  tmpvar_4 = tmpvar_31;
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  xlv_TEXCOORD2 = tmpvar_12;
  xlv_TEXCOORD3 = tmpvar_13;
  xlv_TEXCOORD4 = tmpvar_14;
  xlv_COLOR = tmpvar_4;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _NormalsTex;
uniform lowp samplerCube _SpecCubeTex;
uniform highp float _SpecularStrength;
in highp vec2 xlv_TEXCOORD0;
in highp vec3 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
in highp vec3 xlv_TEXCOORD3;
in highp vec3 xlv_TEXCOORD4;
in lowp vec3 xlv_COLOR;
void main ()
{
  lowp vec3 spec_1;
  mediump vec3 refl_2;
  mediump vec3 wnrm_3;
  mediump vec3 nrm_4;
  lowp vec4 c_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_MainTex, xlv_TEXCOORD0);
  c_5.w = tmpvar_6.w;
  lowp vec3 tmpvar_7;
  tmpvar_7 = ((texture (_NormalsTex, xlv_TEXCOORD0) * 2.0) - 1.0).xyz;
  nrm_4 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8.x = dot (nrm_4, xlv_TEXCOORD2);
  tmpvar_8.y = dot (nrm_4, xlv_TEXCOORD3);
  tmpvar_8.z = dot (nrm_4, xlv_TEXCOORD4);
  wnrm_3 = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD1 - (2.0 * (dot (wnrm_3, xlv_TEXCOORD1) * wnrm_3)));
  refl_2 = tmpvar_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_SpecCubeTex, refl_2);
  highp vec3 tmpvar_11;
  tmpvar_11 = ((tmpvar_10 * _SpecularStrength) * tmpvar_6.w).xyz;
  spec_1 = tmpvar_11;
  c_5.xyz = (tmpvar_6.xyz + spec_1);
  c_5.xyz = (c_5.xyz * xlv_COLOR);
  _glesFragData[0] = c_5;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_LOW" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_LOW" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_MEDIUM" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_MEDIUM" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_HIGH" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_HIGH" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_VERY_HIGH" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_VERY_HIGH" }
"!!GLES3"
}
}
 }
}
}