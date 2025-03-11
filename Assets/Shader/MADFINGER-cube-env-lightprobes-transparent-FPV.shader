èùShader "MADFINGER/Environment/Cube env map transparent (Supports LightProbes) FPV" {
Properties {
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
 _EnvTex ("Cube env tex", CUBE) = "black" {}
 _SHLightingScale ("LightProbe influence scale", Float) = 1
 _FadeoutDist ("Fadeout near (x), far (y)", Vector) = (0.5,1,0,0)
}
SubShader { 
 LOD 100
 Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Transparent" "RenderType"="Transparent" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Transparent" "RenderType"="Transparent" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
Program "vp" {
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_LOW" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
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
uniform highp vec4 _FadeoutDist;
uniform highp vec4 _ProjParams;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR2;
void main ()
{
  highp mat4 projTM_1;
  highp vec4 tmpvar_2;
  highp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  projTM_1 = glstate_matrix_projection;
  projTM_1[0] = glstate_matrix_projection[0]; projTM_1[0].x = (glstate_matrix_projection[0].x * _ProjParams.x);
  projTM_1[1] = projTM_1[1]; projTM_1[1].y = (projTM_1[1].y * _ProjParams.y);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_7;
  tmpvar_7 = (projTM_1 * tmpvar_6);
  tmpvar_2.xyw = tmpvar_7.xyw;
  tmpvar_2.z = (tmpvar_7.z * _ProjParams.z);
  tmpvar_2.z = (tmpvar_2.z + (_ProjParams.w * tmpvar_7.w));
  highp vec2 tmpvar_8;
  tmpvar_8 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 * normalize(_glesNormal));
  highp float tmpvar_11;
  highp vec3 arg0_12;
  arg0_12 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_11 = sqrt(dot (arg0_12, arg0_12));
  highp float v_13;
  v_13 = tmpvar_11;
  highp float r0_14;
  r0_14 = _FadeoutDist.x;
  highp float r1_15;
  r1_15 = _FadeoutDist.y;
  if ((tmpvar_11 < _FadeoutDist.x)) {
    v_13 = r0_14;
  } else {
    if ((v_13 > _FadeoutDist.y)) {
      v_13 = r1_15;
    };
  };
  highp vec3 i_16;
  i_16 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_3.xyz = (i_16 - (2.0 * (dot (tmpvar_10, i_16) * tmpvar_10)));
  tmpvar_3.x = -(tmpvar_3.x);
  tmpvar_3.w = ((v_13 - _FadeoutDist.x) / (_FadeoutDist.y - _FadeoutDist.x));
  highp vec4 tmpvar_17;
  tmpvar_17.w = 1.0;
  tmpvar_17.xyz = tmpvar_10;
  mediump vec3 tmpvar_18;
  mediump vec4 normal_19;
  normal_19 = tmpvar_17;
  highp float vC_20;
  mediump vec3 x3_21;
  mediump vec3 x2_22;
  mediump vec3 x1_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHAr, normal_19);
  x1_23.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHAg, normal_19);
  x1_23.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHAb, normal_19);
  x1_23.z = tmpvar_26;
  mediump vec4 tmpvar_27;
  tmpvar_27 = (normal_19.xyzz * normal_19.yzzx);
  highp float tmpvar_28;
  tmpvar_28 = dot (unity_SHBr, tmpvar_27);
  x2_22.x = tmpvar_28;
  highp float tmpvar_29;
  tmpvar_29 = dot (unity_SHBg, tmpvar_27);
  x2_22.y = tmpvar_29;
  highp float tmpvar_30;
  tmpvar_30 = dot (unity_SHBb, tmpvar_27);
  x2_22.z = tmpvar_30;
  mediump float tmpvar_31;
  tmpvar_31 = ((normal_19.x * normal_19.x) - (normal_19.y * normal_19.y));
  vC_20 = tmpvar_31;
  highp vec3 tmpvar_32;
  tmpvar_32 = (unity_SHC.xyz * vC_20);
  x3_21 = tmpvar_32;
  tmpvar_18 = ((x1_23 + x2_22) + x3_21);
  highp vec4 tmpvar_33;
  tmpvar_33.xyz = (tmpvar_18 * _SHLightingScale);
  tmpvar_33.w = _glesColor.w;
  tmpvar_4 = tmpvar_33;
  gl_Position = tmpvar_2;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_COLOR = tmpvar_4;
  xlv_COLOR2 = tmpvar_5;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.xyz = (tmpvar_2.xyz * xlv_COLOR.xyz);
  highp float tmpvar_3;
  tmpvar_3 = (tmpvar_2.w * xlv_TEXCOORD1.w);
  c_1.w = tmpvar_3;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_LOW" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
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
uniform highp vec4 _FadeoutDist;
uniform highp vec4 _ProjParams;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR2;
void main ()
{
  highp mat4 projTM_1;
  highp vec4 tmpvar_2;
  highp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  projTM_1 = glstate_matrix_projection;
  projTM_1[0] = glstate_matrix_projection[0]; projTM_1[0].x = (glstate_matrix_projection[0].x * _ProjParams.x);
  projTM_1[1] = projTM_1[1]; projTM_1[1].y = (projTM_1[1].y * _ProjParams.y);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_7;
  tmpvar_7 = (projTM_1 * tmpvar_6);
  tmpvar_2.xyw = tmpvar_7.xyw;
  tmpvar_2.z = (tmpvar_7.z * _ProjParams.z);
  tmpvar_2.z = (tmpvar_2.z + (_ProjParams.w * tmpvar_7.w));
  highp vec2 tmpvar_8;
  tmpvar_8 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 * normalize(_glesNormal));
  highp float tmpvar_11;
  highp vec3 arg0_12;
  arg0_12 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_11 = sqrt(dot (arg0_12, arg0_12));
  highp float v_13;
  v_13 = tmpvar_11;
  highp float r0_14;
  r0_14 = _FadeoutDist.x;
  highp float r1_15;
  r1_15 = _FadeoutDist.y;
  if ((tmpvar_11 < _FadeoutDist.x)) {
    v_13 = r0_14;
  } else {
    if ((v_13 > _FadeoutDist.y)) {
      v_13 = r1_15;
    };
  };
  highp vec3 i_16;
  i_16 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_3.xyz = (i_16 - (2.0 * (dot (tmpvar_10, i_16) * tmpvar_10)));
  tmpvar_3.x = -(tmpvar_3.x);
  tmpvar_3.w = ((v_13 - _FadeoutDist.x) / (_FadeoutDist.y - _FadeoutDist.x));
  highp vec4 tmpvar_17;
  tmpvar_17.w = 1.0;
  tmpvar_17.xyz = tmpvar_10;
  mediump vec3 tmpvar_18;
  mediump vec4 normal_19;
  normal_19 = tmpvar_17;
  highp float vC_20;
  mediump vec3 x3_21;
  mediump vec3 x2_22;
  mediump vec3 x1_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHAr, normal_19);
  x1_23.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHAg, normal_19);
  x1_23.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHAb, normal_19);
  x1_23.z = tmpvar_26;
  mediump vec4 tmpvar_27;
  tmpvar_27 = (normal_19.xyzz * normal_19.yzzx);
  highp float tmpvar_28;
  tmpvar_28 = dot (unity_SHBr, tmpvar_27);
  x2_22.x = tmpvar_28;
  highp float tmpvar_29;
  tmpvar_29 = dot (unity_SHBg, tmpvar_27);
  x2_22.y = tmpvar_29;
  highp float tmpvar_30;
  tmpvar_30 = dot (unity_SHBb, tmpvar_27);
  x2_22.z = tmpvar_30;
  mediump float tmpvar_31;
  tmpvar_31 = ((normal_19.x * normal_19.x) - (normal_19.y * normal_19.y));
  vC_20 = tmpvar_31;
  highp vec3 tmpvar_32;
  tmpvar_32 = (unity_SHC.xyz * vC_20);
  x3_21 = tmpvar_32;
  tmpvar_18 = ((x1_23 + x2_22) + x3_21);
  highp vec4 tmpvar_33;
  tmpvar_33.xyz = (tmpvar_18 * _SHLightingScale);
  tmpvar_33.w = _glesColor.w;
  tmpvar_4 = tmpvar_33;
  gl_Position = tmpvar_2;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_COLOR = tmpvar_4;
  xlv_COLOR2 = tmpvar_5;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
in highp vec2 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.xyz = (tmpvar_2.xyz * xlv_COLOR.xyz);
  highp float tmpvar_3;
  tmpvar_3 = (tmpvar_2.w * xlv_TEXCOORD1.w);
  c_1.w = tmpvar_3;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_MEDIUM" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
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
uniform highp vec4 _FadeoutDist;
uniform highp vec4 _ProjParams;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR2;
void main ()
{
  highp mat4 projTM_1;
  highp vec4 tmpvar_2;
  highp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  projTM_1 = glstate_matrix_projection;
  projTM_1[0] = glstate_matrix_projection[0]; projTM_1[0].x = (glstate_matrix_projection[0].x * _ProjParams.x);
  projTM_1[1] = projTM_1[1]; projTM_1[1].y = (projTM_1[1].y * _ProjParams.y);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_7;
  tmpvar_7 = (projTM_1 * tmpvar_6);
  tmpvar_2.xyw = tmpvar_7.xyw;
  tmpvar_2.z = (tmpvar_7.z * _ProjParams.z);
  tmpvar_2.z = (tmpvar_2.z + (_ProjParams.w * tmpvar_7.w));
  highp vec2 tmpvar_8;
  tmpvar_8 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 * normalize(_glesNormal));
  highp float tmpvar_11;
  highp vec3 arg0_12;
  arg0_12 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_11 = sqrt(dot (arg0_12, arg0_12));
  highp float v_13;
  v_13 = tmpvar_11;
  highp float r0_14;
  r0_14 = _FadeoutDist.x;
  highp float r1_15;
  r1_15 = _FadeoutDist.y;
  if ((tmpvar_11 < _FadeoutDist.x)) {
    v_13 = r0_14;
  } else {
    if ((v_13 > _FadeoutDist.y)) {
      v_13 = r1_15;
    };
  };
  highp vec3 i_16;
  i_16 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_3.xyz = (i_16 - (2.0 * (dot (tmpvar_10, i_16) * tmpvar_10)));
  tmpvar_3.x = -(tmpvar_3.x);
  tmpvar_3.w = ((v_13 - _FadeoutDist.x) / (_FadeoutDist.y - _FadeoutDist.x));
  highp vec4 tmpvar_17;
  tmpvar_17.w = 1.0;
  tmpvar_17.xyz = tmpvar_10;
  mediump vec3 tmpvar_18;
  mediump vec4 normal_19;
  normal_19 = tmpvar_17;
  highp float vC_20;
  mediump vec3 x3_21;
  mediump vec3 x2_22;
  mediump vec3 x1_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHAr, normal_19);
  x1_23.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHAg, normal_19);
  x1_23.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHAb, normal_19);
  x1_23.z = tmpvar_26;
  mediump vec4 tmpvar_27;
  tmpvar_27 = (normal_19.xyzz * normal_19.yzzx);
  highp float tmpvar_28;
  tmpvar_28 = dot (unity_SHBr, tmpvar_27);
  x2_22.x = tmpvar_28;
  highp float tmpvar_29;
  tmpvar_29 = dot (unity_SHBg, tmpvar_27);
  x2_22.y = tmpvar_29;
  highp float tmpvar_30;
  tmpvar_30 = dot (unity_SHBb, tmpvar_27);
  x2_22.z = tmpvar_30;
  mediump float tmpvar_31;
  tmpvar_31 = ((normal_19.x * normal_19.x) - (normal_19.y * normal_19.y));
  vC_20 = tmpvar_31;
  highp vec3 tmpvar_32;
  tmpvar_32 = (unity_SHC.xyz * vC_20);
  x3_21 = tmpvar_32;
  tmpvar_18 = ((x1_23 + x2_22) + x3_21);
  highp vec4 tmpvar_33;
  tmpvar_33.xyz = (tmpvar_18 * _SHLightingScale);
  tmpvar_33.w = _glesColor.w;
  tmpvar_4 = tmpvar_33;
  gl_Position = tmpvar_2;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_COLOR = tmpvar_4;
  xlv_COLOR2 = tmpvar_5;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform lowp samplerCube _EnvTex;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz + (textureCube (_EnvTex, xlv_TEXCOORD1.xyz) * xlv_COLOR.w).xyz);
  c_1.xyz = (c_1.xyz * xlv_COLOR.xyz);
  highp float tmpvar_3;
  tmpvar_3 = (tmpvar_2.w * xlv_TEXCOORD1.w);
  c_1.w = tmpvar_3;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_MEDIUM" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
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
uniform highp vec4 _FadeoutDist;
uniform highp vec4 _ProjParams;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR2;
void main ()
{
  highp mat4 projTM_1;
  highp vec4 tmpvar_2;
  highp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  projTM_1 = glstate_matrix_projection;
  projTM_1[0] = glstate_matrix_projection[0]; projTM_1[0].x = (glstate_matrix_projection[0].x * _ProjParams.x);
  projTM_1[1] = projTM_1[1]; projTM_1[1].y = (projTM_1[1].y * _ProjParams.y);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_7;
  tmpvar_7 = (projTM_1 * tmpvar_6);
  tmpvar_2.xyw = tmpvar_7.xyw;
  tmpvar_2.z = (tmpvar_7.z * _ProjParams.z);
  tmpvar_2.z = (tmpvar_2.z + (_ProjParams.w * tmpvar_7.w));
  highp vec2 tmpvar_8;
  tmpvar_8 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 * normalize(_glesNormal));
  highp float tmpvar_11;
  highp vec3 arg0_12;
  arg0_12 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_11 = sqrt(dot (arg0_12, arg0_12));
  highp float v_13;
  v_13 = tmpvar_11;
  highp float r0_14;
  r0_14 = _FadeoutDist.x;
  highp float r1_15;
  r1_15 = _FadeoutDist.y;
  if ((tmpvar_11 < _FadeoutDist.x)) {
    v_13 = r0_14;
  } else {
    if ((v_13 > _FadeoutDist.y)) {
      v_13 = r1_15;
    };
  };
  highp vec3 i_16;
  i_16 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_3.xyz = (i_16 - (2.0 * (dot (tmpvar_10, i_16) * tmpvar_10)));
  tmpvar_3.x = -(tmpvar_3.x);
  tmpvar_3.w = ((v_13 - _FadeoutDist.x) / (_FadeoutDist.y - _FadeoutDist.x));
  highp vec4 tmpvar_17;
  tmpvar_17.w = 1.0;
  tmpvar_17.xyz = tmpvar_10;
  mediump vec3 tmpvar_18;
  mediump vec4 normal_19;
  normal_19 = tmpvar_17;
  highp float vC_20;
  mediump vec3 x3_21;
  mediump vec3 x2_22;
  mediump vec3 x1_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHAr, normal_19);
  x1_23.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHAg, normal_19);
  x1_23.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHAb, normal_19);
  x1_23.z = tmpvar_26;
  mediump vec4 tmpvar_27;
  tmpvar_27 = (normal_19.xyzz * normal_19.yzzx);
  highp float tmpvar_28;
  tmpvar_28 = dot (unity_SHBr, tmpvar_27);
  x2_22.x = tmpvar_28;
  highp float tmpvar_29;
  tmpvar_29 = dot (unity_SHBg, tmpvar_27);
  x2_22.y = tmpvar_29;
  highp float tmpvar_30;
  tmpvar_30 = dot (unity_SHBb, tmpvar_27);
  x2_22.z = tmpvar_30;
  mediump float tmpvar_31;
  tmpvar_31 = ((normal_19.x * normal_19.x) - (normal_19.y * normal_19.y));
  vC_20 = tmpvar_31;
  highp vec3 tmpvar_32;
  tmpvar_32 = (unity_SHC.xyz * vC_20);
  x3_21 = tmpvar_32;
  tmpvar_18 = ((x1_23 + x2_22) + x3_21);
  highp vec4 tmpvar_33;
  tmpvar_33.xyz = (tmpvar_18 * _SHLightingScale);
  tmpvar_33.w = _glesColor.w;
  tmpvar_4 = tmpvar_33;
  gl_Position = tmpvar_2;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_COLOR = tmpvar_4;
  xlv_COLOR2 = tmpvar_5;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform lowp samplerCube _EnvTex;
in highp vec2 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz + (texture (_EnvTex, xlv_TEXCOORD1.xyz) * xlv_COLOR.w).xyz);
  c_1.xyz = (c_1.xyz * xlv_COLOR.xyz);
  highp float tmpvar_3;
  tmpvar_3 = (tmpvar_2.w * xlv_TEXCOORD1.w);
  c_1.w = tmpvar_3;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_HIGH" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
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
uniform highp vec4 _FadeoutDist;
uniform highp vec4 _ProjParams;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR2;
void main ()
{
  highp mat4 projTM_1;
  highp vec4 tmpvar_2;
  highp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  projTM_1 = glstate_matrix_projection;
  projTM_1[0] = glstate_matrix_projection[0]; projTM_1[0].x = (glstate_matrix_projection[0].x * _ProjParams.x);
  projTM_1[1] = projTM_1[1]; projTM_1[1].y = (projTM_1[1].y * _ProjParams.y);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_7;
  tmpvar_7 = (projTM_1 * tmpvar_6);
  tmpvar_2.xyw = tmpvar_7.xyw;
  tmpvar_2.z = (tmpvar_7.z * _ProjParams.z);
  tmpvar_2.z = (tmpvar_2.z + (_ProjParams.w * tmpvar_7.w));
  highp vec2 tmpvar_8;
  tmpvar_8 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 * normalize(_glesNormal));
  highp float tmpvar_11;
  highp vec3 arg0_12;
  arg0_12 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_11 = sqrt(dot (arg0_12, arg0_12));
  highp float v_13;
  v_13 = tmpvar_11;
  highp float r0_14;
  r0_14 = _FadeoutDist.x;
  highp float r1_15;
  r1_15 = _FadeoutDist.y;
  if ((tmpvar_11 < _FadeoutDist.x)) {
    v_13 = r0_14;
  } else {
    if ((v_13 > _FadeoutDist.y)) {
      v_13 = r1_15;
    };
  };
  highp vec3 i_16;
  i_16 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_3.xyz = (i_16 - (2.0 * (dot (tmpvar_10, i_16) * tmpvar_10)));
  tmpvar_3.x = -(tmpvar_3.x);
  tmpvar_3.w = ((v_13 - _FadeoutDist.x) / (_FadeoutDist.y - _FadeoutDist.x));
  highp vec4 tmpvar_17;
  tmpvar_17.w = 1.0;
  tmpvar_17.xyz = tmpvar_10;
  mediump vec3 tmpvar_18;
  mediump vec4 normal_19;
  normal_19 = tmpvar_17;
  highp float vC_20;
  mediump vec3 x3_21;
  mediump vec3 x2_22;
  mediump vec3 x1_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHAr, normal_19);
  x1_23.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHAg, normal_19);
  x1_23.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHAb, normal_19);
  x1_23.z = tmpvar_26;
  mediump vec4 tmpvar_27;
  tmpvar_27 = (normal_19.xyzz * normal_19.yzzx);
  highp float tmpvar_28;
  tmpvar_28 = dot (unity_SHBr, tmpvar_27);
  x2_22.x = tmpvar_28;
  highp float tmpvar_29;
  tmpvar_29 = dot (unity_SHBg, tmpvar_27);
  x2_22.y = tmpvar_29;
  highp float tmpvar_30;
  tmpvar_30 = dot (unity_SHBb, tmpvar_27);
  x2_22.z = tmpvar_30;
  mediump float tmpvar_31;
  tmpvar_31 = ((normal_19.x * normal_19.x) - (normal_19.y * normal_19.y));
  vC_20 = tmpvar_31;
  highp vec3 tmpvar_32;
  tmpvar_32 = (unity_SHC.xyz * vC_20);
  x3_21 = tmpvar_32;
  tmpvar_18 = ((x1_23 + x2_22) + x3_21);
  highp vec4 tmpvar_33;
  tmpvar_33.xyz = (tmpvar_18 * _SHLightingScale);
  tmpvar_33.w = _glesColor.w;
  tmpvar_4 = tmpvar_33;
  gl_Position = tmpvar_2;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_COLOR = tmpvar_4;
  xlv_COLOR2 = tmpvar_5;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform lowp samplerCube _EnvTex;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz + (textureCube (_EnvTex, xlv_TEXCOORD1.xyz) * xlv_COLOR.w).xyz);
  c_1.xyz = (c_1.xyz * xlv_COLOR.xyz);
  highp float tmpvar_3;
  tmpvar_3 = (tmpvar_2.w * xlv_TEXCOORD1.w);
  c_1.w = tmpvar_3;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_HIGH" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
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
uniform highp vec4 _FadeoutDist;
uniform highp vec4 _ProjParams;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR2;
void main ()
{
  highp mat4 projTM_1;
  highp vec4 tmpvar_2;
  highp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  projTM_1 = glstate_matrix_projection;
  projTM_1[0] = glstate_matrix_projection[0]; projTM_1[0].x = (glstate_matrix_projection[0].x * _ProjParams.x);
  projTM_1[1] = projTM_1[1]; projTM_1[1].y = (projTM_1[1].y * _ProjParams.y);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_7;
  tmpvar_7 = (projTM_1 * tmpvar_6);
  tmpvar_2.xyw = tmpvar_7.xyw;
  tmpvar_2.z = (tmpvar_7.z * _ProjParams.z);
  tmpvar_2.z = (tmpvar_2.z + (_ProjParams.w * tmpvar_7.w));
  highp vec2 tmpvar_8;
  tmpvar_8 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 * normalize(_glesNormal));
  highp float tmpvar_11;
  highp vec3 arg0_12;
  arg0_12 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_11 = sqrt(dot (arg0_12, arg0_12));
  highp float v_13;
  v_13 = tmpvar_11;
  highp float r0_14;
  r0_14 = _FadeoutDist.x;
  highp float r1_15;
  r1_15 = _FadeoutDist.y;
  if ((tmpvar_11 < _FadeoutDist.x)) {
    v_13 = r0_14;
  } else {
    if ((v_13 > _FadeoutDist.y)) {
      v_13 = r1_15;
    };
  };
  highp vec3 i_16;
  i_16 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_3.xyz = (i_16 - (2.0 * (dot (tmpvar_10, i_16) * tmpvar_10)));
  tmpvar_3.x = -(tmpvar_3.x);
  tmpvar_3.w = ((v_13 - _FadeoutDist.x) / (_FadeoutDist.y - _FadeoutDist.x));
  highp vec4 tmpvar_17;
  tmpvar_17.w = 1.0;
  tmpvar_17.xyz = tmpvar_10;
  mediump vec3 tmpvar_18;
  mediump vec4 normal_19;
  normal_19 = tmpvar_17;
  highp float vC_20;
  mediump vec3 x3_21;
  mediump vec3 x2_22;
  mediump vec3 x1_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHAr, normal_19);
  x1_23.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHAg, normal_19);
  x1_23.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHAb, normal_19);
  x1_23.z = tmpvar_26;
  mediump vec4 tmpvar_27;
  tmpvar_27 = (normal_19.xyzz * normal_19.yzzx);
  highp float tmpvar_28;
  tmpvar_28 = dot (unity_SHBr, tmpvar_27);
  x2_22.x = tmpvar_28;
  highp float tmpvar_29;
  tmpvar_29 = dot (unity_SHBg, tmpvar_27);
  x2_22.y = tmpvar_29;
  highp float tmpvar_30;
  tmpvar_30 = dot (unity_SHBb, tmpvar_27);
  x2_22.z = tmpvar_30;
  mediump float tmpvar_31;
  tmpvar_31 = ((normal_19.x * normal_19.x) - (normal_19.y * normal_19.y));
  vC_20 = tmpvar_31;
  highp vec3 tmpvar_32;
  tmpvar_32 = (unity_SHC.xyz * vC_20);
  x3_21 = tmpvar_32;
  tmpvar_18 = ((x1_23 + x2_22) + x3_21);
  highp vec4 tmpvar_33;
  tmpvar_33.xyz = (tmpvar_18 * _SHLightingScale);
  tmpvar_33.w = _glesColor.w;
  tmpvar_4 = tmpvar_33;
  gl_Position = tmpvar_2;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_COLOR = tmpvar_4;
  xlv_COLOR2 = tmpvar_5;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform lowp samplerCube _EnvTex;
in highp vec2 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz + (texture (_EnvTex, xlv_TEXCOORD1.xyz) * xlv_COLOR.w).xyz);
  c_1.xyz = (c_1.xyz * xlv_COLOR.xyz);
  highp float tmpvar_3;
  tmpvar_3 = (tmpvar_2.w * xlv_TEXCOORD1.w);
  c_1.w = tmpvar_3;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_VERY_HIGH" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
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
uniform highp vec4 _FadeoutDist;
uniform highp vec4 _ProjParams;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR2;
void main ()
{
  highp mat4 projTM_1;
  highp vec4 tmpvar_2;
  highp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  projTM_1 = glstate_matrix_projection;
  projTM_1[0] = glstate_matrix_projection[0]; projTM_1[0].x = (glstate_matrix_projection[0].x * _ProjParams.x);
  projTM_1[1] = projTM_1[1]; projTM_1[1].y = (projTM_1[1].y * _ProjParams.y);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_7;
  tmpvar_7 = (projTM_1 * tmpvar_6);
  tmpvar_2.xyw = tmpvar_7.xyw;
  tmpvar_2.z = (tmpvar_7.z * _ProjParams.z);
  tmpvar_2.z = (tmpvar_2.z + (_ProjParams.w * tmpvar_7.w));
  highp vec2 tmpvar_8;
  tmpvar_8 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 * normalize(_glesNormal));
  highp float tmpvar_11;
  highp vec3 arg0_12;
  arg0_12 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_11 = sqrt(dot (arg0_12, arg0_12));
  highp float v_13;
  v_13 = tmpvar_11;
  highp float r0_14;
  r0_14 = _FadeoutDist.x;
  highp float r1_15;
  r1_15 = _FadeoutDist.y;
  if ((tmpvar_11 < _FadeoutDist.x)) {
    v_13 = r0_14;
  } else {
    if ((v_13 > _FadeoutDist.y)) {
      v_13 = r1_15;
    };
  };
  highp vec3 i_16;
  i_16 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_3.xyz = (i_16 - (2.0 * (dot (tmpvar_10, i_16) * tmpvar_10)));
  tmpvar_3.x = -(tmpvar_3.x);
  tmpvar_3.w = ((v_13 - _FadeoutDist.x) / (_FadeoutDist.y - _FadeoutDist.x));
  highp vec4 tmpvar_17;
  tmpvar_17.w = 1.0;
  tmpvar_17.xyz = tmpvar_10;
  mediump vec3 tmpvar_18;
  mediump vec4 normal_19;
  normal_19 = tmpvar_17;
  highp float vC_20;
  mediump vec3 x3_21;
  mediump vec3 x2_22;
  mediump vec3 x1_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHAr, normal_19);
  x1_23.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHAg, normal_19);
  x1_23.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHAb, normal_19);
  x1_23.z = tmpvar_26;
  mediump vec4 tmpvar_27;
  tmpvar_27 = (normal_19.xyzz * normal_19.yzzx);
  highp float tmpvar_28;
  tmpvar_28 = dot (unity_SHBr, tmpvar_27);
  x2_22.x = tmpvar_28;
  highp float tmpvar_29;
  tmpvar_29 = dot (unity_SHBg, tmpvar_27);
  x2_22.y = tmpvar_29;
  highp float tmpvar_30;
  tmpvar_30 = dot (unity_SHBb, tmpvar_27);
  x2_22.z = tmpvar_30;
  mediump float tmpvar_31;
  tmpvar_31 = ((normal_19.x * normal_19.x) - (normal_19.y * normal_19.y));
  vC_20 = tmpvar_31;
  highp vec3 tmpvar_32;
  tmpvar_32 = (unity_SHC.xyz * vC_20);
  x3_21 = tmpvar_32;
  tmpvar_18 = ((x1_23 + x2_22) + x3_21);
  highp vec4 tmpvar_33;
  tmpvar_33.xyz = (tmpvar_18 * _SHLightingScale);
  tmpvar_33.w = _glesColor.w;
  tmpvar_4 = tmpvar_33;
  gl_Position = tmpvar_2;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_COLOR = tmpvar_4;
  xlv_COLOR2 = tmpvar_5;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform lowp samplerCube _EnvTex;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz + (textureCube (_EnvTex, xlv_TEXCOORD1.xyz) * xlv_COLOR.w).xyz);
  c_1.xyz = (c_1.xyz * xlv_COLOR.xyz);
  highp float tmpvar_3;
  tmpvar_3 = (tmpvar_2.w * xlv_TEXCOORD1.w);
  c_1.w = tmpvar_3;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_VERY_HIGH" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
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
uniform highp vec4 _FadeoutDist;
uniform highp vec4 _ProjParams;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR2;
void main ()
{
  highp mat4 projTM_1;
  highp vec4 tmpvar_2;
  highp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  projTM_1 = glstate_matrix_projection;
  projTM_1[0] = glstate_matrix_projection[0]; projTM_1[0].x = (glstate_matrix_projection[0].x * _ProjParams.x);
  projTM_1[1] = projTM_1[1]; projTM_1[1].y = (projTM_1[1].y * _ProjParams.y);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp vec4 tmpvar_7;
  tmpvar_7 = (projTM_1 * tmpvar_6);
  tmpvar_2.xyw = tmpvar_7.xyw;
  tmpvar_2.z = (tmpvar_7.z * _ProjParams.z);
  tmpvar_2.z = (tmpvar_2.z + (_ProjParams.w * tmpvar_7.w));
  highp vec2 tmpvar_8;
  tmpvar_8 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 * normalize(_glesNormal));
  highp float tmpvar_11;
  highp vec3 arg0_12;
  arg0_12 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_11 = sqrt(dot (arg0_12, arg0_12));
  highp float v_13;
  v_13 = tmpvar_11;
  highp float r0_14;
  r0_14 = _FadeoutDist.x;
  highp float r1_15;
  r1_15 = _FadeoutDist.y;
  if ((tmpvar_11 < _FadeoutDist.x)) {
    v_13 = r0_14;
  } else {
    if ((v_13 > _FadeoutDist.y)) {
      v_13 = r1_15;
    };
  };
  highp vec3 i_16;
  i_16 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_3.xyz = (i_16 - (2.0 * (dot (tmpvar_10, i_16) * tmpvar_10)));
  tmpvar_3.x = -(tmpvar_3.x);
  tmpvar_3.w = ((v_13 - _FadeoutDist.x) / (_FadeoutDist.y - _FadeoutDist.x));
  highp vec4 tmpvar_17;
  tmpvar_17.w = 1.0;
  tmpvar_17.xyz = tmpvar_10;
  mediump vec3 tmpvar_18;
  mediump vec4 normal_19;
  normal_19 = tmpvar_17;
  highp float vC_20;
  mediump vec3 x3_21;
  mediump vec3 x2_22;
  mediump vec3 x1_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHAr, normal_19);
  x1_23.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHAg, normal_19);
  x1_23.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHAb, normal_19);
  x1_23.z = tmpvar_26;
  mediump vec4 tmpvar_27;
  tmpvar_27 = (normal_19.xyzz * normal_19.yzzx);
  highp float tmpvar_28;
  tmpvar_28 = dot (unity_SHBr, tmpvar_27);
  x2_22.x = tmpvar_28;
  highp float tmpvar_29;
  tmpvar_29 = dot (unity_SHBg, tmpvar_27);
  x2_22.y = tmpvar_29;
  highp float tmpvar_30;
  tmpvar_30 = dot (unity_SHBb, tmpvar_27);
  x2_22.z = tmpvar_30;
  mediump float tmpvar_31;
  tmpvar_31 = ((normal_19.x * normal_19.x) - (normal_19.y * normal_19.y));
  vC_20 = tmpvar_31;
  highp vec3 tmpvar_32;
  tmpvar_32 = (unity_SHC.xyz * vC_20);
  x3_21 = tmpvar_32;
  tmpvar_18 = ((x1_23 + x2_22) + x3_21);
  highp vec4 tmpvar_33;
  tmpvar_33.xyz = (tmpvar_18 * _SHLightingScale);
  tmpvar_33.w = _glesColor.w;
  tmpvar_4 = tmpvar_33;
  gl_Position = tmpvar_2;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_COLOR = tmpvar_4;
  xlv_COLOR2 = tmpvar_5;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform lowp samplerCube _EnvTex;
in highp vec2 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz + (texture (_EnvTex, xlv_TEXCOORD1.xyz) * xlv_COLOR.w).xyz);
  c_1.xyz = (c_1.xyz * xlv_COLOR.xyz);
  highp float tmpvar_3;
  tmpvar_3 = (tmpvar_2.w * xlv_TEXCOORD1.w);
  c_1.w = tmpvar_3;
  _glesFragData[0] = c_1;
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