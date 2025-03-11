иоShader "MADFINGER/Environment/Cube env map (Supports LightProbes)" {
Properties {
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
 _EnvTex ("Cube env tex", CUBE) = "black" {}
 _SHLightingScale ("LightProbe influence scale", Float) = 1
 _EnvStrength ("Env strength weights", Vector) = (0,0,0,2)
 _UVScrollSpeed ("UV scroll speed XY", Vector) = (0,0,0,0)
}
SubShader { 
 LOD 100
 Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
Program "vp" {
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_LOW" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
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
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _MainTex_ST;
uniform highp float _SHLightingScale;
uniform highp vec4 _UVScrollSpeed;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_COLOR;
void main ()
{
  highp vec3 tmpvar_1;
  mat3 tmpvar_2;
  tmpvar_2[0] = _Object2World[0].xyz;
  tmpvar_2[1] = _Object2World[1].xyz;
  tmpvar_2[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = (tmpvar_2 * normalize(_glesNormal));
  highp vec3 tmpvar_4;
  highp vec3 i_5;
  i_5 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_4 = (i_5 - (2.0 * (dot (tmpvar_3, i_5) * tmpvar_3)));
  tmpvar_1.yz = tmpvar_4.yz;
  tmpvar_1.x = -(tmpvar_4.x);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_3;
  mediump vec3 tmpvar_7;
  mediump vec4 normal_8;
  normal_8 = tmpvar_6;
  highp float vC_9;
  mediump vec3 x3_10;
  mediump vec3 x2_11;
  mediump vec3 x1_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (unity_SHAr, normal_8);
  x1_12.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (unity_SHAg, normal_8);
  x1_12.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAb, normal_8);
  x1_12.z = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
  x2_11.x = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
  x2_11.y = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
  x2_11.z = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
  vC_9 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (unity_SHC.xyz * vC_9);
  x3_10 = tmpvar_21;
  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((_UVScrollSpeed.xy * _Time.xy)));
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_COLOR = ((tmpvar_7 * _SHLightingScale) * _glesColor.xyz);
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
in vec4 _glesColor;
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
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _MainTex_ST;
uniform highp float _SHLightingScale;
uniform highp vec4 _UVScrollSpeed;
out highp vec2 xlv_TEXCOORD0;
out highp vec3 xlv_TEXCOORD1;
out highp vec3 xlv_COLOR;
void main ()
{
  highp vec3 tmpvar_1;
  mat3 tmpvar_2;
  tmpvar_2[0] = _Object2World[0].xyz;
  tmpvar_2[1] = _Object2World[1].xyz;
  tmpvar_2[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = (tmpvar_2 * normalize(_glesNormal));
  highp vec3 tmpvar_4;
  highp vec3 i_5;
  i_5 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_4 = (i_5 - (2.0 * (dot (tmpvar_3, i_5) * tmpvar_3)));
  tmpvar_1.yz = tmpvar_4.yz;
  tmpvar_1.x = -(tmpvar_4.x);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_3;
  mediump vec3 tmpvar_7;
  mediump vec4 normal_8;
  normal_8 = tmpvar_6;
  highp float vC_9;
  mediump vec3 x3_10;
  mediump vec3 x2_11;
  mediump vec3 x1_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (unity_SHAr, normal_8);
  x1_12.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (unity_SHAg, normal_8);
  x1_12.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAb, normal_8);
  x1_12.z = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
  x2_11.x = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
  x2_11.y = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
  x2_11.z = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
  vC_9 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (unity_SHC.xyz * vC_9);
  x3_10 = tmpvar_21;
  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((_UVScrollSpeed.xy * _Time.xy)));
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_COLOR = ((tmpvar_7 * _SHLightingScale) * _glesColor.xyz);
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
attribute vec4 _glesColor;
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
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _MainTex_ST;
uniform highp float _SHLightingScale;
uniform highp vec4 _UVScrollSpeed;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_COLOR;
void main ()
{
  highp vec3 tmpvar_1;
  mat3 tmpvar_2;
  tmpvar_2[0] = _Object2World[0].xyz;
  tmpvar_2[1] = _Object2World[1].xyz;
  tmpvar_2[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = (tmpvar_2 * normalize(_glesNormal));
  highp vec3 tmpvar_4;
  highp vec3 i_5;
  i_5 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_4 = (i_5 - (2.0 * (dot (tmpvar_3, i_5) * tmpvar_3)));
  tmpvar_1.yz = tmpvar_4.yz;
  tmpvar_1.x = -(tmpvar_4.x);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_3;
  mediump vec3 tmpvar_7;
  mediump vec4 normal_8;
  normal_8 = tmpvar_6;
  highp float vC_9;
  mediump vec3 x3_10;
  mediump vec3 x2_11;
  mediump vec3 x1_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (unity_SHAr, normal_8);
  x1_12.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (unity_SHAg, normal_8);
  x1_12.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAb, normal_8);
  x1_12.z = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
  x2_11.x = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
  x2_11.y = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
  x2_11.z = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
  vC_9 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (unity_SHC.xyz * vC_9);
  x3_10 = tmpvar_21;
  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((_UVScrollSpeed.xy * _Time.xy)));
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_COLOR = ((tmpvar_7 * _SHLightingScale) * _glesColor.xyz);
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
in vec4 _glesColor;
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
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _MainTex_ST;
uniform highp float _SHLightingScale;
uniform highp vec4 _UVScrollSpeed;
out highp vec2 xlv_TEXCOORD0;
out highp vec3 xlv_TEXCOORD1;
out highp vec3 xlv_COLOR;
void main ()
{
  highp vec3 tmpvar_1;
  mat3 tmpvar_2;
  tmpvar_2[0] = _Object2World[0].xyz;
  tmpvar_2[1] = _Object2World[1].xyz;
  tmpvar_2[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = (tmpvar_2 * normalize(_glesNormal));
  highp vec3 tmpvar_4;
  highp vec3 i_5;
  i_5 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_4 = (i_5 - (2.0 * (dot (tmpvar_3, i_5) * tmpvar_3)));
  tmpvar_1.yz = tmpvar_4.yz;
  tmpvar_1.x = -(tmpvar_4.x);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_3;
  mediump vec3 tmpvar_7;
  mediump vec4 normal_8;
  normal_8 = tmpvar_6;
  highp float vC_9;
  mediump vec3 x3_10;
  mediump vec3 x2_11;
  mediump vec3 x1_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (unity_SHAr, normal_8);
  x1_12.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (unity_SHAg, normal_8);
  x1_12.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAb, normal_8);
  x1_12.z = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
  x2_11.x = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
  x2_11.y = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
  x2_11.z = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
  vC_9 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (unity_SHC.xyz * vC_9);
  x3_10 = tmpvar_21;
  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((_UVScrollSpeed.xy * _Time.xy)));
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_COLOR = ((tmpvar_7 * _SHLightingScale) * _glesColor.xyz);
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
attribute vec4 _glesColor;
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
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _MainTex_ST;
uniform highp float _SHLightingScale;
uniform highp vec4 _UVScrollSpeed;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_COLOR;
void main ()
{
  highp vec3 tmpvar_1;
  mat3 tmpvar_2;
  tmpvar_2[0] = _Object2World[0].xyz;
  tmpvar_2[1] = _Object2World[1].xyz;
  tmpvar_2[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = (tmpvar_2 * normalize(_glesNormal));
  highp vec3 tmpvar_4;
  highp vec3 i_5;
  i_5 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_4 = (i_5 - (2.0 * (dot (tmpvar_3, i_5) * tmpvar_3)));
  tmpvar_1.yz = tmpvar_4.yz;
  tmpvar_1.x = -(tmpvar_4.x);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_3;
  mediump vec3 tmpvar_7;
  mediump vec4 normal_8;
  normal_8 = tmpvar_6;
  highp float vC_9;
  mediump vec3 x3_10;
  mediump vec3 x2_11;
  mediump vec3 x1_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (unity_SHAr, normal_8);
  x1_12.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (unity_SHAg, normal_8);
  x1_12.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAb, normal_8);
  x1_12.z = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
  x2_11.x = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
  x2_11.y = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
  x2_11.z = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
  vC_9 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (unity_SHC.xyz * vC_9);
  x3_10 = tmpvar_21;
  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((_UVScrollSpeed.xy * _Time.xy)));
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_COLOR = ((tmpvar_7 * _SHLightingScale) * _glesColor.xyz);
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
in vec4 _glesColor;
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
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _MainTex_ST;
uniform highp float _SHLightingScale;
uniform highp vec4 _UVScrollSpeed;
out highp vec2 xlv_TEXCOORD0;
out highp vec3 xlv_TEXCOORD1;
out highp vec3 xlv_COLOR;
void main ()
{
  highp vec3 tmpvar_1;
  mat3 tmpvar_2;
  tmpvar_2[0] = _Object2World[0].xyz;
  tmpvar_2[1] = _Object2World[1].xyz;
  tmpvar_2[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = (tmpvar_2 * normalize(_glesNormal));
  highp vec3 tmpvar_4;
  highp vec3 i_5;
  i_5 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_4 = (i_5 - (2.0 * (dot (tmpvar_3, i_5) * tmpvar_3)));
  tmpvar_1.yz = tmpvar_4.yz;
  tmpvar_1.x = -(tmpvar_4.x);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_3;
  mediump vec3 tmpvar_7;
  mediump vec4 normal_8;
  normal_8 = tmpvar_6;
  highp float vC_9;
  mediump vec3 x3_10;
  mediump vec3 x2_11;
  mediump vec3 x1_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (unity_SHAr, normal_8);
  x1_12.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (unity_SHAg, normal_8);
  x1_12.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAb, normal_8);
  x1_12.z = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
  x2_11.x = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
  x2_11.y = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
  x2_11.z = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
  vC_9 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (unity_SHC.xyz * vC_9);
  x3_10 = tmpvar_21;
  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((_UVScrollSpeed.xy * _Time.xy)));
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_COLOR = ((tmpvar_7 * _SHLightingScale) * _glesColor.xyz);
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
attribute vec4 _glesColor;
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
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _MainTex_ST;
uniform highp float _SHLightingScale;
uniform highp vec4 _UVScrollSpeed;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_COLOR;
void main ()
{
  highp vec3 tmpvar_1;
  mat3 tmpvar_2;
  tmpvar_2[0] = _Object2World[0].xyz;
  tmpvar_2[1] = _Object2World[1].xyz;
  tmpvar_2[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = (tmpvar_2 * normalize(_glesNormal));
  highp vec3 tmpvar_4;
  highp vec3 i_5;
  i_5 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_4 = (i_5 - (2.0 * (dot (tmpvar_3, i_5) * tmpvar_3)));
  tmpvar_1.yz = tmpvar_4.yz;
  tmpvar_1.x = -(tmpvar_4.x);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_3;
  mediump vec3 tmpvar_7;
  mediump vec4 normal_8;
  normal_8 = tmpvar_6;
  highp float vC_9;
  mediump vec3 x3_10;
  mediump vec3 x2_11;
  mediump vec3 x1_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (unity_SHAr, normal_8);
  x1_12.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (unity_SHAg, normal_8);
  x1_12.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAb, normal_8);
  x1_12.z = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
  x2_11.x = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
  x2_11.y = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
  x2_11.z = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
  vC_9 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (unity_SHC.xyz * vC_9);
  x3_10 = tmpvar_21;
  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((_UVScrollSpeed.xy * _Time.xy)));
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_COLOR = ((tmpvar_7 * _SHLightingScale) * _glesColor.xyz);
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
in vec4 _glesColor;
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
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _MainTex_ST;
uniform highp float _SHLightingScale;
uniform highp vec4 _UVScrollSpeed;
out highp vec2 xlv_TEXCOORD0;
out highp vec3 xlv_TEXCOORD1;
out highp vec3 xlv_COLOR;
void main ()
{
  highp vec3 tmpvar_1;
  mat3 tmpvar_2;
  tmpvar_2[0] = _Object2World[0].xyz;
  tmpvar_2[1] = _Object2World[1].xyz;
  tmpvar_2[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = (tmpvar_2 * normalize(_glesNormal));
  highp vec3 tmpvar_4;
  highp vec3 i_5;
  i_5 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  tmpvar_4 = (i_5 - (2.0 * (dot (tmpvar_3, i_5) * tmpvar_3)));
  tmpvar_1.yz = tmpvar_4.yz;
  tmpvar_1.x = -(tmpvar_4.x);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_3;
  mediump vec3 tmpvar_7;
  mediump vec4 normal_8;
  normal_8 = tmpvar_6;
  highp float vC_9;
  mediump vec3 x3_10;
  mediump vec3 x2_11;
  mediump vec3 x1_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (unity_SHAr, normal_8);
  x1_12.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (unity_SHAg, normal_8);
  x1_12.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAb, normal_8);
  x1_12.z = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
  x2_11.x = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
  x2_11.y = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
  x2_11.z = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
  vC_9 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (unity_SHC.xyz * vC_9);
  x3_10 = tmpvar_21;
  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((_UVScrollSpeed.xy * _Time.xy)));
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_COLOR = ((tmpvar_7 * _SHLightingScale) * _glesColor.xyz);
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