±†Shader "MADFINGER/Environment/Cube env map (Supports LightProbes) FPV" {
Properties {
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
 _EnvTex ("Cube env tex", CUBE) = "black" {}
 _SHLightingScale ("LightProbe influence scale", Float) = 1
 _EnvStrength ("Env strength weights", Vector) = (0,0,0,2)
 _Params ("x - FPV proj", Vector) = (1,0,0,0)
 _UVScrollSpeed ("UV scroll speed XY", Vector) = (0,0,0,0)
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
uniform highp vec4 _Time;
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
uniform highp vec4 _MainTex_ST;
uniform highp float _SHLightingScale;
uniform highp vec4 _ProjParams;
uniform highp vec4 _Params;
uniform highp vec4 _UVScrollSpeed;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_COLOR;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp mat4 projTM_2;
  highp vec4 tmpvar_3;
  highp vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  projTM_2 = glstate_matrix_projection;
  highp vec4 tmpvar_6;
  if ((_Params.x > 0.0)) {
    tmpvar_6 = _ProjParams;
  } else {
    tmpvar_6 = vec4(1.0, 1.0, 1.0, 0.0);
  };
  projTM_2[0] = glstate_matrix_projection[0]; projTM_2[0].x = (glstate_matrix_projection[0].x * tmpvar_6.x);
  projTM_2[1] = projTM_2[1]; projTM_2[1].y = (projTM_2[1].y * tmpvar_6.y);
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = tmpvar_5;
  highp vec4 tmpvar_8;
  tmpvar_8 = (projTM_2 * tmpvar_7);
  tmpvar_3.xyw = tmpvar_8.xyw;
  tmpvar_3.z = (tmpvar_8.z * tmpvar_6.z);
  tmpvar_3.z = (tmpvar_3.z + (tmpvar_6.w * tmpvar_8.w));
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 * tmpvar_1);
  highp vec3 tmpvar_11;
  highp vec3 i_12;
  i_12 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_11 = (i_12 - (2.0 * (dot (tmpvar_10, i_12) * tmpvar_10)));
  tmpvar_4.yz = tmpvar_11.yz;
  tmpvar_4.x = -(tmpvar_11.x);
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = tmpvar_10;
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
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((_UVScrollSpeed.xy * _Time.xy)));
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_COLOR = (tmpvar_14 * _SHLightingScale);
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  highp vec3 tmpvar_3;
  tmpvar_3 = (tmpvar_2.xyz * xlv_COLOR);
  c_1.xyz = tmpvar_3;
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
uniform highp vec4 _Time;
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
uniform highp vec4 _MainTex_ST;
uniform highp float _SHLightingScale;
uniform highp vec4 _ProjParams;
uniform highp vec4 _Params;
uniform highp vec4 _UVScrollSpeed;
out highp vec2 xlv_TEXCOORD0;
out highp vec3 xlv_TEXCOORD1;
out highp vec3 xlv_COLOR;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp mat4 projTM_2;
  highp vec4 tmpvar_3;
  highp vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  projTM_2 = glstate_matrix_projection;
  highp vec4 tmpvar_6;
  if ((_Params.x > 0.0)) {
    tmpvar_6 = _ProjParams;
  } else {
    tmpvar_6 = vec4(1.0, 1.0, 1.0, 0.0);
  };
  projTM_2[0] = glstate_matrix_projection[0]; projTM_2[0].x = (glstate_matrix_projection[0].x * tmpvar_6.x);
  projTM_2[1] = projTM_2[1]; projTM_2[1].y = (projTM_2[1].y * tmpvar_6.y);
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = tmpvar_5;
  highp vec4 tmpvar_8;
  tmpvar_8 = (projTM_2 * tmpvar_7);
  tmpvar_3.xyw = tmpvar_8.xyw;
  tmpvar_3.z = (tmpvar_8.z * tmpvar_6.z);
  tmpvar_3.z = (tmpvar_3.z + (tmpvar_6.w * tmpvar_8.w));
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 * tmpvar_1);
  highp vec3 tmpvar_11;
  highp vec3 i_12;
  i_12 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_11 = (i_12 - (2.0 * (dot (tmpvar_10, i_12) * tmpvar_10)));
  tmpvar_4.yz = tmpvar_11.yz;
  tmpvar_4.x = -(tmpvar_11.x);
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = tmpvar_10;
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
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((_UVScrollSpeed.xy * _Time.xy)));
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_COLOR = (tmpvar_14 * _SHLightingScale);
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
in highp vec2 xlv_TEXCOORD0;
in highp vec3 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  highp vec3 tmpvar_3;
  tmpvar_3 = (tmpvar_2.xyz * xlv_COLOR);
  c_1.xyz = tmpvar_3;
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
uniform highp vec4 _Time;
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
uniform highp vec4 _MainTex_ST;
uniform highp float _SHLightingScale;
uniform highp vec4 _ProjParams;
uniform highp vec4 _Params;
uniform highp vec4 _UVScrollSpeed;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_COLOR;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp mat4 projTM_2;
  highp vec4 tmpvar_3;
  highp vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  projTM_2 = glstate_matrix_projection;
  highp vec4 tmpvar_6;
  if ((_Params.x > 0.0)) {
    tmpvar_6 = _ProjParams;
  } else {
    tmpvar_6 = vec4(1.0, 1.0, 1.0, 0.0);
  };
  projTM_2[0] = glstate_matrix_projection[0]; projTM_2[0].x = (glstate_matrix_projection[0].x * tmpvar_6.x);
  projTM_2[1] = projTM_2[1]; projTM_2[1].y = (projTM_2[1].y * tmpvar_6.y);
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = tmpvar_5;
  highp vec4 tmpvar_8;
  tmpvar_8 = (projTM_2 * tmpvar_7);
  tmpvar_3.xyw = tmpvar_8.xyw;
  tmpvar_3.z = (tmpvar_8.z * tmpvar_6.z);
  tmpvar_3.z = (tmpvar_3.z + (tmpvar_6.w * tmpvar_8.w));
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 * tmpvar_1);
  highp vec3 tmpvar_11;
  highp vec3 i_12;
  i_12 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_11 = (i_12 - (2.0 * (dot (tmpvar_10, i_12) * tmpvar_10)));
  tmpvar_4.yz = tmpvar_11.yz;
  tmpvar_4.x = -(tmpvar_11.x);
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = tmpvar_10;
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
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((_UVScrollSpeed.xy * _Time.xy)));
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_COLOR = (tmpvar_14 * _SHLightingScale);
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform lowp samplerCube _EnvTex;
uniform highp vec4 _EnvStrength;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  lowp vec4 tmpvar_3;
  tmpvar_3 = textureCube (_EnvTex, xlv_TEXCOORD1);
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_2.xyz + (tmpvar_3 * dot (tmpvar_2, _EnvStrength)).xyz);
  c_1.xyz = tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (c_1.xyz * xlv_COLOR);
  c_1.xyz = tmpvar_5;
  gl_FragData[0] = c_1;
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
uniform highp vec4 _Time;
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
uniform highp vec4 _MainTex_ST;
uniform highp float _SHLightingScale;
uniform highp vec4 _ProjParams;
uniform highp vec4 _Params;
uniform highp vec4 _UVScrollSpeed;
out highp vec2 xlv_TEXCOORD0;
out highp vec3 xlv_TEXCOORD1;
out highp vec3 xlv_COLOR;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp mat4 projTM_2;
  highp vec4 tmpvar_3;
  highp vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  projTM_2 = glstate_matrix_projection;
  highp vec4 tmpvar_6;
  if ((_Params.x > 0.0)) {
    tmpvar_6 = _ProjParams;
  } else {
    tmpvar_6 = vec4(1.0, 1.0, 1.0, 0.0);
  };
  projTM_2[0] = glstate_matrix_projection[0]; projTM_2[0].x = (glstate_matrix_projection[0].x * tmpvar_6.x);
  projTM_2[1] = projTM_2[1]; projTM_2[1].y = (projTM_2[1].y * tmpvar_6.y);
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = tmpvar_5;
  highp vec4 tmpvar_8;
  tmpvar_8 = (projTM_2 * tmpvar_7);
  tmpvar_3.xyw = tmpvar_8.xyw;
  tmpvar_3.z = (tmpvar_8.z * tmpvar_6.z);
  tmpvar_3.z = (tmpvar_3.z + (tmpvar_6.w * tmpvar_8.w));
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 * tmpvar_1);
  highp vec3 tmpvar_11;
  highp vec3 i_12;
  i_12 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_11 = (i_12 - (2.0 * (dot (tmpvar_10, i_12) * tmpvar_10)));
  tmpvar_4.yz = tmpvar_11.yz;
  tmpvar_4.x = -(tmpvar_11.x);
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = tmpvar_10;
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
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((_UVScrollSpeed.xy * _Time.xy)));
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_COLOR = (tmpvar_14 * _SHLightingScale);
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform lowp samplerCube _EnvTex;
uniform highp vec4 _EnvStrength;
in highp vec2 xlv_TEXCOORD0;
in highp vec3 xlv_TEXCOORD1;
in highp vec3 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_EnvTex, xlv_TEXCOORD1);
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_2.xyz + (tmpvar_3 * dot (tmpvar_2, _EnvStrength)).xyz);
  c_1.xyz = tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (c_1.xyz * xlv_COLOR);
  c_1.xyz = tmpvar_5;
  _glesFragData[0] = c_1;
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
uniform highp vec4 _Time;
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
uniform highp vec4 _MainTex_ST;
uniform highp float _SHLightingScale;
uniform highp vec4 _ProjParams;
uniform highp vec4 _Params;
uniform highp vec4 _UVScrollSpeed;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_COLOR;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp mat4 projTM_2;
  highp vec4 tmpvar_3;
  highp vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  projTM_2 = glstate_matrix_projection;
  highp vec4 tmpvar_6;
  if ((_Params.x > 0.0)) {
    tmpvar_6 = _ProjParams;
  } else {
    tmpvar_6 = vec4(1.0, 1.0, 1.0, 0.0);
  };
  projTM_2[0] = glstate_matrix_projection[0]; projTM_2[0].x = (glstate_matrix_projection[0].x * tmpvar_6.x);
  projTM_2[1] = projTM_2[1]; projTM_2[1].y = (projTM_2[1].y * tmpvar_6.y);
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = tmpvar_5;
  highp vec4 tmpvar_8;
  tmpvar_8 = (projTM_2 * tmpvar_7);
  tmpvar_3.xyw = tmpvar_8.xyw;
  tmpvar_3.z = (tmpvar_8.z * tmpvar_6.z);
  tmpvar_3.z = (tmpvar_3.z + (tmpvar_6.w * tmpvar_8.w));
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 * tmpvar_1);
  highp vec3 tmpvar_11;
  highp vec3 i_12;
  i_12 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_11 = (i_12 - (2.0 * (dot (tmpvar_10, i_12) * tmpvar_10)));
  tmpvar_4.yz = tmpvar_11.yz;
  tmpvar_4.x = -(tmpvar_11.x);
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = tmpvar_10;
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
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((_UVScrollSpeed.xy * _Time.xy)));
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_COLOR = (tmpvar_14 * _SHLightingScale);
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform lowp samplerCube _EnvTex;
uniform highp vec4 _EnvStrength;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  lowp vec4 tmpvar_3;
  tmpvar_3 = textureCube (_EnvTex, xlv_TEXCOORD1);
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_2.xyz + (tmpvar_3 * dot (tmpvar_2, _EnvStrength)).xyz);
  c_1.xyz = tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (c_1.xyz * xlv_COLOR);
  c_1.xyz = tmpvar_5;
  gl_FragData[0] = c_1;
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
uniform highp vec4 _Time;
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
uniform highp vec4 _MainTex_ST;
uniform highp float _SHLightingScale;
uniform highp vec4 _ProjParams;
uniform highp vec4 _Params;
uniform highp vec4 _UVScrollSpeed;
out highp vec2 xlv_TEXCOORD0;
out highp vec3 xlv_TEXCOORD1;
out highp vec3 xlv_COLOR;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp mat4 projTM_2;
  highp vec4 tmpvar_3;
  highp vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  projTM_2 = glstate_matrix_projection;
  highp vec4 tmpvar_6;
  if ((_Params.x > 0.0)) {
    tmpvar_6 = _ProjParams;
  } else {
    tmpvar_6 = vec4(1.0, 1.0, 1.0, 0.0);
  };
  projTM_2[0] = glstate_matrix_projection[0]; projTM_2[0].x = (glstate_matrix_projection[0].x * tmpvar_6.x);
  projTM_2[1] = projTM_2[1]; projTM_2[1].y = (projTM_2[1].y * tmpvar_6.y);
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = tmpvar_5;
  highp vec4 tmpvar_8;
  tmpvar_8 = (projTM_2 * tmpvar_7);
  tmpvar_3.xyw = tmpvar_8.xyw;
  tmpvar_3.z = (tmpvar_8.z * tmpvar_6.z);
  tmpvar_3.z = (tmpvar_3.z + (tmpvar_6.w * tmpvar_8.w));
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 * tmpvar_1);
  highp vec3 tmpvar_11;
  highp vec3 i_12;
  i_12 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_11 = (i_12 - (2.0 * (dot (tmpvar_10, i_12) * tmpvar_10)));
  tmpvar_4.yz = tmpvar_11.yz;
  tmpvar_4.x = -(tmpvar_11.x);
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = tmpvar_10;
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
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((_UVScrollSpeed.xy * _Time.xy)));
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_COLOR = (tmpvar_14 * _SHLightingScale);
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform lowp samplerCube _EnvTex;
uniform highp vec4 _EnvStrength;
in highp vec2 xlv_TEXCOORD0;
in highp vec3 xlv_TEXCOORD1;
in highp vec3 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_EnvTex, xlv_TEXCOORD1);
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_2.xyz + (tmpvar_3 * dot (tmpvar_2, _EnvStrength)).xyz);
  c_1.xyz = tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (c_1.xyz * xlv_COLOR);
  c_1.xyz = tmpvar_5;
  _glesFragData[0] = c_1;
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
uniform highp vec4 _Time;
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
uniform highp vec4 _MainTex_ST;
uniform highp float _SHLightingScale;
uniform highp vec4 _ProjParams;
uniform highp vec4 _Params;
uniform highp vec4 _UVScrollSpeed;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_COLOR;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp mat4 projTM_2;
  highp vec4 tmpvar_3;
  highp vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  projTM_2 = glstate_matrix_projection;
  highp vec4 tmpvar_6;
  if ((_Params.x > 0.0)) {
    tmpvar_6 = _ProjParams;
  } else {
    tmpvar_6 = vec4(1.0, 1.0, 1.0, 0.0);
  };
  projTM_2[0] = glstate_matrix_projection[0]; projTM_2[0].x = (glstate_matrix_projection[0].x * tmpvar_6.x);
  projTM_2[1] = projTM_2[1]; projTM_2[1].y = (projTM_2[1].y * tmpvar_6.y);
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = tmpvar_5;
  highp vec4 tmpvar_8;
  tmpvar_8 = (projTM_2 * tmpvar_7);
  tmpvar_3.xyw = tmpvar_8.xyw;
  tmpvar_3.z = (tmpvar_8.z * tmpvar_6.z);
  tmpvar_3.z = (tmpvar_3.z + (tmpvar_6.w * tmpvar_8.w));
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 * tmpvar_1);
  highp vec3 tmpvar_11;
  highp vec3 i_12;
  i_12 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_11 = (i_12 - (2.0 * (dot (tmpvar_10, i_12) * tmpvar_10)));
  tmpvar_4.yz = tmpvar_11.yz;
  tmpvar_4.x = -(tmpvar_11.x);
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = tmpvar_10;
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
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((_UVScrollSpeed.xy * _Time.xy)));
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_COLOR = (tmpvar_14 * _SHLightingScale);
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform lowp samplerCube _EnvTex;
uniform highp vec4 _EnvStrength;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  lowp vec4 tmpvar_3;
  tmpvar_3 = textureCube (_EnvTex, xlv_TEXCOORD1);
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_2.xyz + (tmpvar_3 * dot (tmpvar_2, _EnvStrength)).xyz);
  c_1.xyz = tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (c_1.xyz * xlv_COLOR);
  c_1.xyz = tmpvar_5;
  gl_FragData[0] = c_1;
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
uniform highp vec4 _Time;
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
uniform highp vec4 _MainTex_ST;
uniform highp float _SHLightingScale;
uniform highp vec4 _ProjParams;
uniform highp vec4 _Params;
uniform highp vec4 _UVScrollSpeed;
out highp vec2 xlv_TEXCOORD0;
out highp vec3 xlv_TEXCOORD1;
out highp vec3 xlv_COLOR;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp mat4 projTM_2;
  highp vec4 tmpvar_3;
  highp vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  projTM_2 = glstate_matrix_projection;
  highp vec4 tmpvar_6;
  if ((_Params.x > 0.0)) {
    tmpvar_6 = _ProjParams;
  } else {
    tmpvar_6 = vec4(1.0, 1.0, 1.0, 0.0);
  };
  projTM_2[0] = glstate_matrix_projection[0]; projTM_2[0].x = (glstate_matrix_projection[0].x * tmpvar_6.x);
  projTM_2[1] = projTM_2[1]; projTM_2[1].y = (projTM_2[1].y * tmpvar_6.y);
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = tmpvar_5;
  highp vec4 tmpvar_8;
  tmpvar_8 = (projTM_2 * tmpvar_7);
  tmpvar_3.xyw = tmpvar_8.xyw;
  tmpvar_3.z = (tmpvar_8.z * tmpvar_6.z);
  tmpvar_3.z = (tmpvar_3.z + (tmpvar_6.w * tmpvar_8.w));
  mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 * tmpvar_1);
  highp vec3 tmpvar_11;
  highp vec3 i_12;
  i_12 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_11 = (i_12 - (2.0 * (dot (tmpvar_10, i_12) * tmpvar_10)));
  tmpvar_4.yz = tmpvar_11.yz;
  tmpvar_4.x = -(tmpvar_11.x);
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = tmpvar_10;
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
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((_UVScrollSpeed.xy * _Time.xy)));
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_COLOR = (tmpvar_14 * _SHLightingScale);
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform lowp samplerCube _EnvTex;
uniform highp vec4 _EnvStrength;
in highp vec2 xlv_TEXCOORD0;
in highp vec3 xlv_TEXCOORD1;
in highp vec3 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_EnvTex, xlv_TEXCOORD1);
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_2.xyz + (tmpvar_3 * dot (tmpvar_2, _EnvStrength)).xyz);
  c_1.xyz = tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (c_1.xyz * xlv_COLOR);
  c_1.xyz = tmpvar_5;
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