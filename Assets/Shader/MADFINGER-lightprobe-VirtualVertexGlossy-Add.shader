ôrShader "MADFINGER/Environment/Lightprobes with VirtualGloss Per-Vertex Additive" {
Properties {
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
 _SpecOffset ("Specular Offset from Camera", Vector) = (1,10,2,0)
 _SpecRange ("Specular Range", Float) = 20
 _SpecColor ("Specular Color", Color) = (1,1,1,1)
 _Shininess ("Shininess", Range(0.01,1)) = 0.078125
 _SHLightingScale ("LightProbe influence scale", Float) = 1
}
SubShader { 
 LOD 100
 Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
Program "vp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp vec3 _SpecOffset;
uniform highp float _SpecRange;
uniform highp vec3 _SpecColor;
uniform highp float _Shininess;
uniform highp float _SHLightingScale;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  mat3 tmpvar_6;
  tmpvar_6[0] = glstate_matrix_modelview0[0].xyz;
  tmpvar_6[1] = glstate_matrix_modelview0[1].xyz;
  tmpvar_6[2] = glstate_matrix_modelview0[2].xyz;
  highp vec3 tmpvar_7;
  tmpvar_7 = ((glstate_matrix_modelview0 * _glesVertex).xyz - (_SpecOffset * vec3(1.0, 1.0, -1.0)));
  highp vec3 tmpvar_8;
  tmpvar_8 = (((_SpecColor * pow (clamp (dot ((tmpvar_6 * tmpvar_1), normalize(((vec3(0.0, 0.0, 1.0) + normalize(-(tmpvar_7))) * 0.5))), 0.0, 1.0), (_Shininess * 128.0))) * 2.0) * (1.0 - clamp ((sqrt(dot (tmpvar_7, tmpvar_7)) / _SpecRange), 0.0, 1.0)));
  tmpvar_3 = tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = (tmpvar_5 * tmpvar_1);
  mediump vec3 tmpvar_10;
  mediump vec4 normal_11;
  normal_11 = tmpvar_9;
  highp float vC_12;
  mediump vec3 x3_13;
  mediump vec3 x2_14;
  mediump vec3 x1_15;
  highp float tmpvar_16;
  tmpvar_16 = dot (unity_SHAr, normal_11);
  x1_15.x = tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHAg, normal_11);
  x1_15.y = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHAb, normal_11);
  x1_15.z = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (normal_11.xyzz * normal_11.yzzx);
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHBr, tmpvar_19);
  x2_14.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHBg, tmpvar_19);
  x2_14.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHBb, tmpvar_19);
  x2_14.z = tmpvar_22;
  mediump float tmpvar_23;
  tmpvar_23 = ((normal_11.x * normal_11.x) - (normal_11.y * normal_11.y));
  vC_12 = tmpvar_23;
  highp vec3 tmpvar_24;
  tmpvar_24 = (unity_SHC.xyz * vC_12);
  x3_13 = tmpvar_24;
  tmpvar_10 = ((x1_15 + x2_14) + x3_13);
  highp vec3 tmpvar_25;
  tmpvar_25 = (tmpvar_10 * _SHLightingScale);
  tmpvar_4 = tmpvar_25;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD3 = tmpvar_3;
  xlv_TEXCOORD4 = tmpvar_4;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz * xlv_TEXCOORD4);
  c_1.xyz = (c_1.xyz + (xlv_TEXCOORD3 * tmpvar_2.w));
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp vec3 _SpecOffset;
uniform highp float _SpecRange;
uniform highp vec3 _SpecColor;
uniform highp float _Shininess;
uniform highp float _SHLightingScale;
out highp vec2 xlv_TEXCOORD0;
out highp vec3 xlv_TEXCOORD1;
out lowp vec3 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  mat3 tmpvar_6;
  tmpvar_6[0] = glstate_matrix_modelview0[0].xyz;
  tmpvar_6[1] = glstate_matrix_modelview0[1].xyz;
  tmpvar_6[2] = glstate_matrix_modelview0[2].xyz;
  highp vec3 tmpvar_7;
  tmpvar_7 = ((glstate_matrix_modelview0 * _glesVertex).xyz - (_SpecOffset * vec3(1.0, 1.0, -1.0)));
  highp vec3 tmpvar_8;
  tmpvar_8 = (((_SpecColor * pow (clamp (dot ((tmpvar_6 * tmpvar_1), normalize(((vec3(0.0, 0.0, 1.0) + normalize(-(tmpvar_7))) * 0.5))), 0.0, 1.0), (_Shininess * 128.0))) * 2.0) * (1.0 - clamp ((sqrt(dot (tmpvar_7, tmpvar_7)) / _SpecRange), 0.0, 1.0)));
  tmpvar_3 = tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = (tmpvar_5 * tmpvar_1);
  mediump vec3 tmpvar_10;
  mediump vec4 normal_11;
  normal_11 = tmpvar_9;
  highp float vC_12;
  mediump vec3 x3_13;
  mediump vec3 x2_14;
  mediump vec3 x1_15;
  highp float tmpvar_16;
  tmpvar_16 = dot (unity_SHAr, normal_11);
  x1_15.x = tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHAg, normal_11);
  x1_15.y = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHAb, normal_11);
  x1_15.z = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (normal_11.xyzz * normal_11.yzzx);
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHBr, tmpvar_19);
  x2_14.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHBg, tmpvar_19);
  x2_14.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHBb, tmpvar_19);
  x2_14.z = tmpvar_22;
  mediump float tmpvar_23;
  tmpvar_23 = ((normal_11.x * normal_11.x) - (normal_11.y * normal_11.y));
  vC_12 = tmpvar_23;
  highp vec3 tmpvar_24;
  tmpvar_24 = (unity_SHC.xyz * vC_12);
  x3_13 = tmpvar_24;
  tmpvar_10 = ((x1_15 + x2_14) + x3_13);
  highp vec3 tmpvar_25;
  tmpvar_25 = (tmpvar_10 * _SHLightingScale);
  tmpvar_4 = tmpvar_25;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD3 = tmpvar_3;
  xlv_TEXCOORD4 = tmpvar_4;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
in highp vec2 xlv_TEXCOORD0;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz * xlv_TEXCOORD4);
  c_1.xyz = (c_1.xyz + (xlv_TEXCOORD3 * tmpvar_2.w));
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp vec3 _SpecOffset;
uniform highp float _SpecRange;
uniform highp vec3 _SpecColor;
uniform highp float _Shininess;
uniform highp float _SHLightingScale;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  mat3 tmpvar_6;
  tmpvar_6[0] = glstate_matrix_modelview0[0].xyz;
  tmpvar_6[1] = glstate_matrix_modelview0[1].xyz;
  tmpvar_6[2] = glstate_matrix_modelview0[2].xyz;
  highp vec3 tmpvar_7;
  tmpvar_7 = ((glstate_matrix_modelview0 * _glesVertex).xyz - (_SpecOffset * vec3(1.0, 1.0, -1.0)));
  highp vec3 tmpvar_8;
  tmpvar_8 = (((_SpecColor * pow (clamp (dot ((tmpvar_6 * tmpvar_1), normalize(((vec3(0.0, 0.0, 1.0) + normalize(-(tmpvar_7))) * 0.5))), 0.0, 1.0), (_Shininess * 128.0))) * 2.0) * (1.0 - clamp ((sqrt(dot (tmpvar_7, tmpvar_7)) / _SpecRange), 0.0, 1.0)));
  tmpvar_3 = tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = (tmpvar_5 * tmpvar_1);
  mediump vec3 tmpvar_10;
  mediump vec4 normal_11;
  normal_11 = tmpvar_9;
  highp float vC_12;
  mediump vec3 x3_13;
  mediump vec3 x2_14;
  mediump vec3 x1_15;
  highp float tmpvar_16;
  tmpvar_16 = dot (unity_SHAr, normal_11);
  x1_15.x = tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHAg, normal_11);
  x1_15.y = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHAb, normal_11);
  x1_15.z = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (normal_11.xyzz * normal_11.yzzx);
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHBr, tmpvar_19);
  x2_14.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHBg, tmpvar_19);
  x2_14.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHBb, tmpvar_19);
  x2_14.z = tmpvar_22;
  mediump float tmpvar_23;
  tmpvar_23 = ((normal_11.x * normal_11.x) - (normal_11.y * normal_11.y));
  vC_12 = tmpvar_23;
  highp vec3 tmpvar_24;
  tmpvar_24 = (unity_SHC.xyz * vC_12);
  x3_13 = tmpvar_24;
  tmpvar_10 = ((x1_15 + x2_14) + x3_13);
  highp vec3 tmpvar_25;
  tmpvar_25 = (tmpvar_10 * _SHLightingScale);
  tmpvar_4 = tmpvar_25;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD3 = tmpvar_3;
  xlv_TEXCOORD4 = tmpvar_4;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz * xlv_TEXCOORD4);
  c_1.xyz = (c_1.xyz + (xlv_TEXCOORD3 * tmpvar_2.w));
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp vec3 _SpecOffset;
uniform highp float _SpecRange;
uniform highp vec3 _SpecColor;
uniform highp float _Shininess;
uniform highp float _SHLightingScale;
out highp vec2 xlv_TEXCOORD0;
out highp vec3 xlv_TEXCOORD1;
out lowp vec3 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  mat3 tmpvar_6;
  tmpvar_6[0] = glstate_matrix_modelview0[0].xyz;
  tmpvar_6[1] = glstate_matrix_modelview0[1].xyz;
  tmpvar_6[2] = glstate_matrix_modelview0[2].xyz;
  highp vec3 tmpvar_7;
  tmpvar_7 = ((glstate_matrix_modelview0 * _glesVertex).xyz - (_SpecOffset * vec3(1.0, 1.0, -1.0)));
  highp vec3 tmpvar_8;
  tmpvar_8 = (((_SpecColor * pow (clamp (dot ((tmpvar_6 * tmpvar_1), normalize(((vec3(0.0, 0.0, 1.0) + normalize(-(tmpvar_7))) * 0.5))), 0.0, 1.0), (_Shininess * 128.0))) * 2.0) * (1.0 - clamp ((sqrt(dot (tmpvar_7, tmpvar_7)) / _SpecRange), 0.0, 1.0)));
  tmpvar_3 = tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = (tmpvar_5 * tmpvar_1);
  mediump vec3 tmpvar_10;
  mediump vec4 normal_11;
  normal_11 = tmpvar_9;
  highp float vC_12;
  mediump vec3 x3_13;
  mediump vec3 x2_14;
  mediump vec3 x1_15;
  highp float tmpvar_16;
  tmpvar_16 = dot (unity_SHAr, normal_11);
  x1_15.x = tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHAg, normal_11);
  x1_15.y = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHAb, normal_11);
  x1_15.z = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (normal_11.xyzz * normal_11.yzzx);
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHBr, tmpvar_19);
  x2_14.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHBg, tmpvar_19);
  x2_14.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHBb, tmpvar_19);
  x2_14.z = tmpvar_22;
  mediump float tmpvar_23;
  tmpvar_23 = ((normal_11.x * normal_11.x) - (normal_11.y * normal_11.y));
  vC_12 = tmpvar_23;
  highp vec3 tmpvar_24;
  tmpvar_24 = (unity_SHC.xyz * vC_12);
  x3_13 = tmpvar_24;
  tmpvar_10 = ((x1_15 + x2_14) + x3_13);
  highp vec3 tmpvar_25;
  tmpvar_25 = (tmpvar_10 * _SHLightingScale);
  tmpvar_4 = tmpvar_25;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD3 = tmpvar_3;
  xlv_TEXCOORD4 = tmpvar_4;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
in highp vec2 xlv_TEXCOORD0;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz * xlv_TEXCOORD4);
  c_1.xyz = (c_1.xyz + (xlv_TEXCOORD3 * tmpvar_2.w));
  _glesFragData[0] = c_1;
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