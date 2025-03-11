ÏiShader "MADFINGER/Environment/Lightprobes with Gloss Per-Vertex Additive" {
Properties {
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
 _SpecDir ("Specular Direction", Vector) = (1,1,0,0)
 _SpecColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec3 _SpecDir;
uniform highp vec3 _SpecColor;
uniform highp float _Shininess;
uniform highp float _SHLightingScale;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec3 tmpvar_1;
  lowp vec3 tmpvar_2;
  mat3 tmpvar_3;
  tmpvar_3[0] = _Object2World[0].xyz;
  tmpvar_3[1] = _Object2World[1].xyz;
  tmpvar_3[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * normalize(_glesNormal));
  highp vec3 tmpvar_5;
  tmpvar_5 = normalize(((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos));
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_5 - (2.0 * (dot (tmpvar_4, tmpvar_5) * tmpvar_4)));
  highp vec3 tmpvar_7;
  tmpvar_7 = ((_SpecColor * pow (clamp (dot (normalize(_SpecDir), tmpvar_6), 0.0, 1.0), (_Shininess * 128.0))) * 2.0);
  tmpvar_1 = tmpvar_7;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = tmpvar_4;
  mediump vec3 tmpvar_9;
  mediump vec4 normal_10;
  normal_10 = tmpvar_8;
  highp float vC_11;
  mediump vec3 x3_12;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAr, normal_10);
  x1_14.x = tmpvar_15;
  highp float tmpvar_16;
  tmpvar_16 = dot (unity_SHAg, normal_10);
  x1_14.y = tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHAb, normal_10);
  x1_14.z = tmpvar_17;
  mediump vec4 tmpvar_18;
  tmpvar_18 = (normal_10.xyzz * normal_10.yzzx);
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBr, tmpvar_18);
  x2_13.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHBg, tmpvar_18);
  x2_13.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHBb, tmpvar_18);
  x2_13.z = tmpvar_21;
  mediump float tmpvar_22;
  tmpvar_22 = ((normal_10.x * normal_10.x) - (normal_10.y * normal_10.y));
  vC_11 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = (unity_SHC.xyz * vC_11);
  x3_12 = tmpvar_23;
  tmpvar_9 = ((x1_14 + x2_13) + x3_12);
  highp vec3 tmpvar_24;
  tmpvar_24 = (tmpvar_9 * _SHLightingScale);
  tmpvar_2 = tmpvar_24;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD3 = tmpvar_1;
  xlv_TEXCOORD4 = tmpvar_2;
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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec3 _SpecDir;
uniform highp vec3 _SpecColor;
uniform highp float _Shininess;
uniform highp float _SHLightingScale;
out highp vec2 xlv_TEXCOORD0;
out highp vec3 xlv_TEXCOORD1;
out lowp vec3 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec3 tmpvar_1;
  lowp vec3 tmpvar_2;
  mat3 tmpvar_3;
  tmpvar_3[0] = _Object2World[0].xyz;
  tmpvar_3[1] = _Object2World[1].xyz;
  tmpvar_3[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * normalize(_glesNormal));
  highp vec3 tmpvar_5;
  tmpvar_5 = normalize(((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos));
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_5 - (2.0 * (dot (tmpvar_4, tmpvar_5) * tmpvar_4)));
  highp vec3 tmpvar_7;
  tmpvar_7 = ((_SpecColor * pow (clamp (dot (normalize(_SpecDir), tmpvar_6), 0.0, 1.0), (_Shininess * 128.0))) * 2.0);
  tmpvar_1 = tmpvar_7;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = tmpvar_4;
  mediump vec3 tmpvar_9;
  mediump vec4 normal_10;
  normal_10 = tmpvar_8;
  highp float vC_11;
  mediump vec3 x3_12;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAr, normal_10);
  x1_14.x = tmpvar_15;
  highp float tmpvar_16;
  tmpvar_16 = dot (unity_SHAg, normal_10);
  x1_14.y = tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHAb, normal_10);
  x1_14.z = tmpvar_17;
  mediump vec4 tmpvar_18;
  tmpvar_18 = (normal_10.xyzz * normal_10.yzzx);
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBr, tmpvar_18);
  x2_13.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHBg, tmpvar_18);
  x2_13.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHBb, tmpvar_18);
  x2_13.z = tmpvar_21;
  mediump float tmpvar_22;
  tmpvar_22 = ((normal_10.x * normal_10.x) - (normal_10.y * normal_10.y));
  vC_11 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = (unity_SHC.xyz * vC_11);
  x3_12 = tmpvar_23;
  tmpvar_9 = ((x1_14 + x2_13) + x3_12);
  highp vec3 tmpvar_24;
  tmpvar_24 = (tmpvar_9 * _SHLightingScale);
  tmpvar_2 = tmpvar_24;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD3 = tmpvar_1;
  xlv_TEXCOORD4 = tmpvar_2;
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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec3 _SpecDir;
uniform highp vec3 _SpecColor;
uniform highp float _Shininess;
uniform highp float _SHLightingScale;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec3 tmpvar_1;
  lowp vec3 tmpvar_2;
  mat3 tmpvar_3;
  tmpvar_3[0] = _Object2World[0].xyz;
  tmpvar_3[1] = _Object2World[1].xyz;
  tmpvar_3[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * normalize(_glesNormal));
  highp vec3 tmpvar_5;
  tmpvar_5 = normalize(((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos));
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_5 - (2.0 * (dot (tmpvar_4, tmpvar_5) * tmpvar_4)));
  highp vec3 tmpvar_7;
  tmpvar_7 = ((_SpecColor * pow (clamp (dot (normalize(_SpecDir), tmpvar_6), 0.0, 1.0), (_Shininess * 128.0))) * 2.0);
  tmpvar_1 = tmpvar_7;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = tmpvar_4;
  mediump vec3 tmpvar_9;
  mediump vec4 normal_10;
  normal_10 = tmpvar_8;
  highp float vC_11;
  mediump vec3 x3_12;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAr, normal_10);
  x1_14.x = tmpvar_15;
  highp float tmpvar_16;
  tmpvar_16 = dot (unity_SHAg, normal_10);
  x1_14.y = tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHAb, normal_10);
  x1_14.z = tmpvar_17;
  mediump vec4 tmpvar_18;
  tmpvar_18 = (normal_10.xyzz * normal_10.yzzx);
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBr, tmpvar_18);
  x2_13.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHBg, tmpvar_18);
  x2_13.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHBb, tmpvar_18);
  x2_13.z = tmpvar_21;
  mediump float tmpvar_22;
  tmpvar_22 = ((normal_10.x * normal_10.x) - (normal_10.y * normal_10.y));
  vC_11 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = (unity_SHC.xyz * vC_11);
  x3_12 = tmpvar_23;
  tmpvar_9 = ((x1_14 + x2_13) + x3_12);
  highp vec3 tmpvar_24;
  tmpvar_24 = (tmpvar_9 * _SHLightingScale);
  tmpvar_2 = tmpvar_24;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD3 = tmpvar_1;
  xlv_TEXCOORD4 = tmpvar_2;
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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec3 _SpecDir;
uniform highp vec3 _SpecColor;
uniform highp float _Shininess;
uniform highp float _SHLightingScale;
out highp vec2 xlv_TEXCOORD0;
out highp vec3 xlv_TEXCOORD1;
out lowp vec3 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec3 tmpvar_1;
  lowp vec3 tmpvar_2;
  mat3 tmpvar_3;
  tmpvar_3[0] = _Object2World[0].xyz;
  tmpvar_3[1] = _Object2World[1].xyz;
  tmpvar_3[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * normalize(_glesNormal));
  highp vec3 tmpvar_5;
  tmpvar_5 = normalize(((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos));
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_5 - (2.0 * (dot (tmpvar_4, tmpvar_5) * tmpvar_4)));
  highp vec3 tmpvar_7;
  tmpvar_7 = ((_SpecColor * pow (clamp (dot (normalize(_SpecDir), tmpvar_6), 0.0, 1.0), (_Shininess * 128.0))) * 2.0);
  tmpvar_1 = tmpvar_7;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = tmpvar_4;
  mediump vec3 tmpvar_9;
  mediump vec4 normal_10;
  normal_10 = tmpvar_8;
  highp float vC_11;
  mediump vec3 x3_12;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAr, normal_10);
  x1_14.x = tmpvar_15;
  highp float tmpvar_16;
  tmpvar_16 = dot (unity_SHAg, normal_10);
  x1_14.y = tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHAb, normal_10);
  x1_14.z = tmpvar_17;
  mediump vec4 tmpvar_18;
  tmpvar_18 = (normal_10.xyzz * normal_10.yzzx);
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBr, tmpvar_18);
  x2_13.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHBg, tmpvar_18);
  x2_13.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHBb, tmpvar_18);
  x2_13.z = tmpvar_21;
  mediump float tmpvar_22;
  tmpvar_22 = ((normal_10.x * normal_10.x) - (normal_10.y * normal_10.y));
  vC_11 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = (unity_SHC.xyz * vC_11);
  x3_12 = tmpvar_23;
  tmpvar_9 = ((x1_14 + x2_13) + x3_12);
  highp vec3 tmpvar_24;
  tmpvar_24 = (tmpvar_9 * _SHLightingScale);
  tmpvar_2 = tmpvar_24;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD3 = tmpvar_1;
  xlv_TEXCOORD4 = tmpvar_2;
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