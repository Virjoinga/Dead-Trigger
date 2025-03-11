‘±Shader "MADFINGER/FX/Water" {
Properties {
 _EnvTex ("Cube env tex", CUBE) = "black" {}
 _Normals ("Normals", 2D) = "bump" {}
 _Params ("Fresnel bias - x, Fresnel pow - y, Transparency bias - z", Vector) = (0,10,0.2,0)
 _Params2 ("Bumps tiling - x, Bumps scroll speed", Vector) = (0.25,0.1,0,0)
 _DeepColor ("Deep color", Color) = (0,1,0,1)
 _ShallowColor ("Shallow color", Color) = (1,0,0,1)
}
SubShader { 
 LOD 100
 Tags { "QUEUE"="Transparent-10" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent-10" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
Program "vp" {
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_LOW" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _Params;
uniform highp vec4 _DeepColor;
uniform highp vec4 _ShallowColor;
varying highp vec3 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
void main ()
{
  highp vec3 tmpvar_1;
  lowp vec4 tmpvar_2;
  lowp vec4 tmpvar_3;
  mat3 tmpvar_4;
  tmpvar_4[0] = _Object2World[0].xyz;
  tmpvar_4[1] = _Object2World[1].xyz;
  tmpvar_4[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_5;
  tmpvar_5 = normalize((tmpvar_4 * normalize(_glesNormal)));
  highp vec3 tmpvar_6;
  tmpvar_6 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  highp vec3 tmpvar_7;
  highp vec3 i_8;
  i_8 = -(tmpvar_6);
  tmpvar_7 = (i_8 - (2.0 * (dot (tmpvar_5, i_8) * tmpvar_5)));
  highp float tmpvar_9;
  tmpvar_9 = (1.0 - clamp (dot (tmpvar_6, tmpvar_5), 0.0, 1.0));
  highp float tmpvar_10;
  tmpvar_10 = clamp ((_Params.x + ((1.0 - _Params.x) * pow (tmpvar_9, _Params.y))), 0.0, 1.0);
  tmpvar_1.yz = tmpvar_7.yz;
  tmpvar_1.x = -(tmpvar_7.x);
  highp vec4 tmpvar_11;
  tmpvar_11.xyz = mix (_DeepColor, _ShallowColor, vec4(tmpvar_9)).xyz;
  tmpvar_11.w = tmpvar_10;
  tmpvar_2 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = vec4((tmpvar_10 + _Params.z));
  tmpvar_3 = tmpvar_12;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_3;
}



#endif
#ifdef FRAGMENT

uniform lowp samplerCube _EnvTex;
varying highp vec3 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
void main ()
{
  mediump vec3 refl_1;
  lowp vec4 res_2;
  refl_1 = xlv_TEXCOORD0;
  res_2.w = xlv_COLOR1.w;
  res_2.xyz = mix (xlv_COLOR.xyz, textureCube (_EnvTex, refl_1).xyz, xlv_COLOR.www);
  gl_FragData[0] = res_2;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_LOW" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _Params;
uniform highp vec4 _DeepColor;
uniform highp vec4 _ShallowColor;
out highp vec3 xlv_TEXCOORD0;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
void main ()
{
  highp vec3 tmpvar_1;
  lowp vec4 tmpvar_2;
  lowp vec4 tmpvar_3;
  mat3 tmpvar_4;
  tmpvar_4[0] = _Object2World[0].xyz;
  tmpvar_4[1] = _Object2World[1].xyz;
  tmpvar_4[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_5;
  tmpvar_5 = normalize((tmpvar_4 * normalize(_glesNormal)));
  highp vec3 tmpvar_6;
  tmpvar_6 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  highp vec3 tmpvar_7;
  highp vec3 i_8;
  i_8 = -(tmpvar_6);
  tmpvar_7 = (i_8 - (2.0 * (dot (tmpvar_5, i_8) * tmpvar_5)));
  highp float tmpvar_9;
  tmpvar_9 = (1.0 - clamp (dot (tmpvar_6, tmpvar_5), 0.0, 1.0));
  highp float tmpvar_10;
  tmpvar_10 = clamp ((_Params.x + ((1.0 - _Params.x) * pow (tmpvar_9, _Params.y))), 0.0, 1.0);
  tmpvar_1.yz = tmpvar_7.yz;
  tmpvar_1.x = -(tmpvar_7.x);
  highp vec4 tmpvar_11;
  tmpvar_11.xyz = mix (_DeepColor, _ShallowColor, vec4(tmpvar_9)).xyz;
  tmpvar_11.w = tmpvar_10;
  tmpvar_2 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = vec4((tmpvar_10 + _Params.z));
  tmpvar_3 = tmpvar_12;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_3;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform lowp samplerCube _EnvTex;
in highp vec3 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
void main ()
{
  mediump vec3 refl_1;
  lowp vec4 res_2;
  refl_1 = xlv_TEXCOORD0;
  res_2.w = xlv_COLOR1.w;
  res_2.xyz = mix (xlv_COLOR.xyz, texture (_EnvTex, refl_1).xyz, xlv_COLOR.www);
  _glesFragData[0] = res_2;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_MEDIUM" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _Params;
uniform highp vec4 _Params2;
uniform highp vec4 _DeepColor;
uniform highp vec4 _ShallowColor;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
void main ()
{
  highp vec3 tmpvar_1;
  highp vec4 tmpvar_2;
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_6;
  tmpvar_6 = normalize((tmpvar_5 * normalize(_glesNormal)));
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  highp vec3 tmpvar_8;
  highp vec3 i_9;
  i_9 = -(tmpvar_7);
  tmpvar_8 = (i_9 - (2.0 * (dot (tmpvar_6, i_9) * tmpvar_6)));
  highp float tmpvar_10;
  tmpvar_10 = (1.0 - clamp (dot (tmpvar_7, tmpvar_6), 0.0, 1.0));
  highp float tmpvar_11;
  tmpvar_11 = clamp ((_Params.x + ((1.0 - _Params.x) * pow (tmpvar_10, _Params.y))), 0.0, 1.0);
  tmpvar_1.yz = tmpvar_8.yz;
  tmpvar_1.x = -(tmpvar_8.x);
  highp vec4 tmpvar_12;
  tmpvar_12.xyz = mix (_DeepColor, _ShallowColor, vec4(tmpvar_10)).xyz;
  tmpvar_12.w = tmpvar_11;
  tmpvar_3 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = vec4((tmpvar_11 + _Params.z));
  tmpvar_4 = tmpvar_13;
  highp vec3 tmpvar_14;
  tmpvar_14 = (_Object2World * _glesVertex).xyz;
  tmpvar_2.xy = ((tmpvar_14.xz * _Params2.x) + fract(((_Time.yy * _Params2.y) * vec2(-0.393919, 0.919145))));
  tmpvar_2.zw = ((tmpvar_14.xz * _Params2.x) + fract((vec2(0.464238, 0.185695) * (_Time.yy * _Params2.y))));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_6;
  xlv_TEXCOORD3 = (_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz);
  xlv_COLOR = tmpvar_3;
  xlv_COLOR1 = tmpvar_4;
}



#endif
#ifdef FRAGMENT

uniform lowp samplerCube _EnvTex;
uniform sampler2D _Normals;
uniform highp vec4 _Params;
uniform highp vec4 _DeepColor;
uniform highp vec4 _ShallowColor;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec3 color_1;
  mediump float fresnel_2;
  mediump vec3 viewDir_3;
  lowp vec4 res_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_Normals, xlv_TEXCOORD1.xy);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_Normals, xlv_TEXCOORD1.zw);
  mediump vec3 n1_7;
  n1_7 = ((tmpvar_5.xyz * 2.0) - 1.0);
  mediump vec3 n2_8;
  n2_8 = ((tmpvar_6.xyz * 2.0) - 1.0);
  mediump vec3 tmpvar_9;
  tmpvar_9.xy = (n1_7.xy + n2_8.xy);
  tmpvar_9.z = (n1_7.z * n2_8.z);
  mediump vec3 tmpvar_10;
  tmpvar_10 = normalize(tmpvar_9);
  mediump vec3 n2_11;
  n2_11 = xlv_TEXCOORD2;
  mediump vec3 tmpvar_12;
  tmpvar_12.xy = (tmpvar_10.xz + n2_11.xy);
  tmpvar_12.z = (tmpvar_10.y * n2_11.z);
  mediump vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_12);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(xlv_TEXCOORD3);
  viewDir_3 = tmpvar_14;
  mediump vec3 tmpvar_15;
  mediump vec3 i_16;
  i_16 = -(viewDir_3);
  tmpvar_15 = (i_16 - (2.0 * (dot (tmpvar_13, i_16) * tmpvar_13)));
  mediump float tmpvar_17;
  tmpvar_17 = (1.0 - clamp (dot (viewDir_3, tmpvar_13), 0.0, 1.0));
  highp float facing_18;
  facing_18 = tmpvar_17;
  highp float tmpvar_19;
  tmpvar_19 = clamp ((_Params.x + ((1.0 - _Params.x) * pow (facing_18, _Params.y))), 0.0, 1.0);
  fresnel_2 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = mix (_DeepColor, _ShallowColor, vec4(tmpvar_17)).xyz;
  color_1 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (fresnel_2 + _Params.z);
  res_4.w = tmpvar_21;
  lowp vec4 tmpvar_22;
  tmpvar_22 = textureCube (_EnvTex, tmpvar_15);
  mediump vec3 tmpvar_23;
  tmpvar_23 = mix (color_1, tmpvar_22.xyz, vec3(fresnel_2));
  res_4.xyz = tmpvar_23;
  gl_FragData[0] = res_4;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_MEDIUM" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _Params;
uniform highp vec4 _Params2;
uniform highp vec4 _DeepColor;
uniform highp vec4 _ShallowColor;
out highp vec3 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
void main ()
{
  highp vec3 tmpvar_1;
  highp vec4 tmpvar_2;
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_6;
  tmpvar_6 = normalize((tmpvar_5 * normalize(_glesNormal)));
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  highp vec3 tmpvar_8;
  highp vec3 i_9;
  i_9 = -(tmpvar_7);
  tmpvar_8 = (i_9 - (2.0 * (dot (tmpvar_6, i_9) * tmpvar_6)));
  highp float tmpvar_10;
  tmpvar_10 = (1.0 - clamp (dot (tmpvar_7, tmpvar_6), 0.0, 1.0));
  highp float tmpvar_11;
  tmpvar_11 = clamp ((_Params.x + ((1.0 - _Params.x) * pow (tmpvar_10, _Params.y))), 0.0, 1.0);
  tmpvar_1.yz = tmpvar_8.yz;
  tmpvar_1.x = -(tmpvar_8.x);
  highp vec4 tmpvar_12;
  tmpvar_12.xyz = mix (_DeepColor, _ShallowColor, vec4(tmpvar_10)).xyz;
  tmpvar_12.w = tmpvar_11;
  tmpvar_3 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = vec4((tmpvar_11 + _Params.z));
  tmpvar_4 = tmpvar_13;
  highp vec3 tmpvar_14;
  tmpvar_14 = (_Object2World * _glesVertex).xyz;
  tmpvar_2.xy = ((tmpvar_14.xz * _Params2.x) + fract(((_Time.yy * _Params2.y) * vec2(-0.393919, 0.919145))));
  tmpvar_2.zw = ((tmpvar_14.xz * _Params2.x) + fract((vec2(0.464238, 0.185695) * (_Time.yy * _Params2.y))));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_6;
  xlv_TEXCOORD3 = (_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz);
  xlv_COLOR = tmpvar_3;
  xlv_COLOR1 = tmpvar_4;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform lowp samplerCube _EnvTex;
uniform sampler2D _Normals;
uniform highp vec4 _Params;
uniform highp vec4 _DeepColor;
uniform highp vec4 _ShallowColor;
in highp vec4 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
in highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec3 color_1;
  mediump float fresnel_2;
  mediump vec3 viewDir_3;
  lowp vec4 res_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture (_Normals, xlv_TEXCOORD1.xy);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_Normals, xlv_TEXCOORD1.zw);
  mediump vec3 n1_7;
  n1_7 = ((tmpvar_5.xyz * 2.0) - 1.0);
  mediump vec3 n2_8;
  n2_8 = ((tmpvar_6.xyz * 2.0) - 1.0);
  mediump vec3 tmpvar_9;
  tmpvar_9.xy = (n1_7.xy + n2_8.xy);
  tmpvar_9.z = (n1_7.z * n2_8.z);
  mediump vec3 tmpvar_10;
  tmpvar_10 = normalize(tmpvar_9);
  mediump vec3 n2_11;
  n2_11 = xlv_TEXCOORD2;
  mediump vec3 tmpvar_12;
  tmpvar_12.xy = (tmpvar_10.xz + n2_11.xy);
  tmpvar_12.z = (tmpvar_10.y * n2_11.z);
  mediump vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_12);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(xlv_TEXCOORD3);
  viewDir_3 = tmpvar_14;
  mediump vec3 tmpvar_15;
  mediump vec3 i_16;
  i_16 = -(viewDir_3);
  tmpvar_15 = (i_16 - (2.0 * (dot (tmpvar_13, i_16) * tmpvar_13)));
  mediump float tmpvar_17;
  tmpvar_17 = (1.0 - clamp (dot (viewDir_3, tmpvar_13), 0.0, 1.0));
  highp float facing_18;
  facing_18 = tmpvar_17;
  highp float tmpvar_19;
  tmpvar_19 = clamp ((_Params.x + ((1.0 - _Params.x) * pow (facing_18, _Params.y))), 0.0, 1.0);
  fresnel_2 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = mix (_DeepColor, _ShallowColor, vec4(tmpvar_17)).xyz;
  color_1 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (fresnel_2 + _Params.z);
  res_4.w = tmpvar_21;
  lowp vec4 tmpvar_22;
  tmpvar_22 = texture (_EnvTex, tmpvar_15);
  mediump vec3 tmpvar_23;
  tmpvar_23 = mix (color_1, tmpvar_22.xyz, vec3(fresnel_2));
  res_4.xyz = tmpvar_23;
  _glesFragData[0] = res_4;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_HIGH" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _Params;
uniform highp vec4 _Params2;
uniform highp vec4 _DeepColor;
uniform highp vec4 _ShallowColor;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
void main ()
{
  highp vec3 tmpvar_1;
  highp vec4 tmpvar_2;
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_6;
  tmpvar_6 = normalize((tmpvar_5 * normalize(_glesNormal)));
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  highp vec3 tmpvar_8;
  highp vec3 i_9;
  i_9 = -(tmpvar_7);
  tmpvar_8 = (i_9 - (2.0 * (dot (tmpvar_6, i_9) * tmpvar_6)));
  highp float tmpvar_10;
  tmpvar_10 = (1.0 - clamp (dot (tmpvar_7, tmpvar_6), 0.0, 1.0));
  highp float tmpvar_11;
  tmpvar_11 = clamp ((_Params.x + ((1.0 - _Params.x) * pow (tmpvar_10, _Params.y))), 0.0, 1.0);
  tmpvar_1.yz = tmpvar_8.yz;
  tmpvar_1.x = -(tmpvar_8.x);
  highp vec4 tmpvar_12;
  tmpvar_12.xyz = mix (_DeepColor, _ShallowColor, vec4(tmpvar_10)).xyz;
  tmpvar_12.w = tmpvar_11;
  tmpvar_3 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = vec4((tmpvar_11 + _Params.z));
  tmpvar_4 = tmpvar_13;
  highp vec3 tmpvar_14;
  tmpvar_14 = (_Object2World * _glesVertex).xyz;
  tmpvar_2.xy = ((tmpvar_14.xz * _Params2.x) + fract(((_Time.yy * _Params2.y) * vec2(-0.393919, 0.919145))));
  tmpvar_2.zw = ((tmpvar_14.xz * _Params2.x) + fract((vec2(0.464238, 0.185695) * (_Time.yy * _Params2.y))));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_6;
  xlv_TEXCOORD3 = (_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz);
  xlv_COLOR = tmpvar_3;
  xlv_COLOR1 = tmpvar_4;
}



#endif
#ifdef FRAGMENT

uniform lowp samplerCube _EnvTex;
uniform sampler2D _Normals;
uniform highp vec4 _Params;
uniform highp vec4 _DeepColor;
uniform highp vec4 _ShallowColor;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec3 color_1;
  mediump float fresnel_2;
  mediump vec3 viewDir_3;
  lowp vec4 res_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_Normals, xlv_TEXCOORD1.xy);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_Normals, xlv_TEXCOORD1.zw);
  mediump vec3 n1_7;
  n1_7 = ((tmpvar_5.xyz * 2.0) - 1.0);
  mediump vec3 n2_8;
  n2_8 = ((tmpvar_6.xyz * 2.0) - 1.0);
  mediump vec3 tmpvar_9;
  tmpvar_9.xy = (n1_7.xy + n2_8.xy);
  tmpvar_9.z = (n1_7.z * n2_8.z);
  mediump vec3 tmpvar_10;
  tmpvar_10 = normalize(tmpvar_9);
  mediump vec3 n2_11;
  n2_11 = xlv_TEXCOORD2;
  mediump vec3 tmpvar_12;
  tmpvar_12.xy = (tmpvar_10.xz + n2_11.xy);
  tmpvar_12.z = (tmpvar_10.y * n2_11.z);
  mediump vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_12);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(xlv_TEXCOORD3);
  viewDir_3 = tmpvar_14;
  mediump vec3 tmpvar_15;
  mediump vec3 i_16;
  i_16 = -(viewDir_3);
  tmpvar_15 = (i_16 - (2.0 * (dot (tmpvar_13, i_16) * tmpvar_13)));
  mediump float tmpvar_17;
  tmpvar_17 = (1.0 - clamp (dot (viewDir_3, tmpvar_13), 0.0, 1.0));
  highp float facing_18;
  facing_18 = tmpvar_17;
  highp float tmpvar_19;
  tmpvar_19 = clamp ((_Params.x + ((1.0 - _Params.x) * pow (facing_18, _Params.y))), 0.0, 1.0);
  fresnel_2 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = mix (_DeepColor, _ShallowColor, vec4(tmpvar_17)).xyz;
  color_1 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (fresnel_2 + _Params.z);
  res_4.w = tmpvar_21;
  lowp vec4 tmpvar_22;
  tmpvar_22 = textureCube (_EnvTex, tmpvar_15);
  mediump vec3 tmpvar_23;
  tmpvar_23 = mix (color_1, tmpvar_22.xyz, vec3(fresnel_2));
  res_4.xyz = tmpvar_23;
  gl_FragData[0] = res_4;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_HIGH" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _Params;
uniform highp vec4 _Params2;
uniform highp vec4 _DeepColor;
uniform highp vec4 _ShallowColor;
out highp vec3 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
void main ()
{
  highp vec3 tmpvar_1;
  highp vec4 tmpvar_2;
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_6;
  tmpvar_6 = normalize((tmpvar_5 * normalize(_glesNormal)));
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  highp vec3 tmpvar_8;
  highp vec3 i_9;
  i_9 = -(tmpvar_7);
  tmpvar_8 = (i_9 - (2.0 * (dot (tmpvar_6, i_9) * tmpvar_6)));
  highp float tmpvar_10;
  tmpvar_10 = (1.0 - clamp (dot (tmpvar_7, tmpvar_6), 0.0, 1.0));
  highp float tmpvar_11;
  tmpvar_11 = clamp ((_Params.x + ((1.0 - _Params.x) * pow (tmpvar_10, _Params.y))), 0.0, 1.0);
  tmpvar_1.yz = tmpvar_8.yz;
  tmpvar_1.x = -(tmpvar_8.x);
  highp vec4 tmpvar_12;
  tmpvar_12.xyz = mix (_DeepColor, _ShallowColor, vec4(tmpvar_10)).xyz;
  tmpvar_12.w = tmpvar_11;
  tmpvar_3 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = vec4((tmpvar_11 + _Params.z));
  tmpvar_4 = tmpvar_13;
  highp vec3 tmpvar_14;
  tmpvar_14 = (_Object2World * _glesVertex).xyz;
  tmpvar_2.xy = ((tmpvar_14.xz * _Params2.x) + fract(((_Time.yy * _Params2.y) * vec2(-0.393919, 0.919145))));
  tmpvar_2.zw = ((tmpvar_14.xz * _Params2.x) + fract((vec2(0.464238, 0.185695) * (_Time.yy * _Params2.y))));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_6;
  xlv_TEXCOORD3 = (_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz);
  xlv_COLOR = tmpvar_3;
  xlv_COLOR1 = tmpvar_4;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform lowp samplerCube _EnvTex;
uniform sampler2D _Normals;
uniform highp vec4 _Params;
uniform highp vec4 _DeepColor;
uniform highp vec4 _ShallowColor;
in highp vec4 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
in highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec3 color_1;
  mediump float fresnel_2;
  mediump vec3 viewDir_3;
  lowp vec4 res_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture (_Normals, xlv_TEXCOORD1.xy);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_Normals, xlv_TEXCOORD1.zw);
  mediump vec3 n1_7;
  n1_7 = ((tmpvar_5.xyz * 2.0) - 1.0);
  mediump vec3 n2_8;
  n2_8 = ((tmpvar_6.xyz * 2.0) - 1.0);
  mediump vec3 tmpvar_9;
  tmpvar_9.xy = (n1_7.xy + n2_8.xy);
  tmpvar_9.z = (n1_7.z * n2_8.z);
  mediump vec3 tmpvar_10;
  tmpvar_10 = normalize(tmpvar_9);
  mediump vec3 n2_11;
  n2_11 = xlv_TEXCOORD2;
  mediump vec3 tmpvar_12;
  tmpvar_12.xy = (tmpvar_10.xz + n2_11.xy);
  tmpvar_12.z = (tmpvar_10.y * n2_11.z);
  mediump vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_12);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(xlv_TEXCOORD3);
  viewDir_3 = tmpvar_14;
  mediump vec3 tmpvar_15;
  mediump vec3 i_16;
  i_16 = -(viewDir_3);
  tmpvar_15 = (i_16 - (2.0 * (dot (tmpvar_13, i_16) * tmpvar_13)));
  mediump float tmpvar_17;
  tmpvar_17 = (1.0 - clamp (dot (viewDir_3, tmpvar_13), 0.0, 1.0));
  highp float facing_18;
  facing_18 = tmpvar_17;
  highp float tmpvar_19;
  tmpvar_19 = clamp ((_Params.x + ((1.0 - _Params.x) * pow (facing_18, _Params.y))), 0.0, 1.0);
  fresnel_2 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = mix (_DeepColor, _ShallowColor, vec4(tmpvar_17)).xyz;
  color_1 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (fresnel_2 + _Params.z);
  res_4.w = tmpvar_21;
  lowp vec4 tmpvar_22;
  tmpvar_22 = texture (_EnvTex, tmpvar_15);
  mediump vec3 tmpvar_23;
  tmpvar_23 = mix (color_1, tmpvar_22.xyz, vec3(fresnel_2));
  res_4.xyz = tmpvar_23;
  _glesFragData[0] = res_4;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_VERY_HIGH" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _Params;
uniform highp vec4 _Params2;
uniform highp vec4 _DeepColor;
uniform highp vec4 _ShallowColor;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
void main ()
{
  highp vec3 tmpvar_1;
  highp vec4 tmpvar_2;
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_6;
  tmpvar_6 = normalize((tmpvar_5 * normalize(_glesNormal)));
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  highp vec3 tmpvar_8;
  highp vec3 i_9;
  i_9 = -(tmpvar_7);
  tmpvar_8 = (i_9 - (2.0 * (dot (tmpvar_6, i_9) * tmpvar_6)));
  highp float tmpvar_10;
  tmpvar_10 = (1.0 - clamp (dot (tmpvar_7, tmpvar_6), 0.0, 1.0));
  highp float tmpvar_11;
  tmpvar_11 = clamp ((_Params.x + ((1.0 - _Params.x) * pow (tmpvar_10, _Params.y))), 0.0, 1.0);
  tmpvar_1.yz = tmpvar_8.yz;
  tmpvar_1.x = -(tmpvar_8.x);
  highp vec4 tmpvar_12;
  tmpvar_12.xyz = mix (_DeepColor, _ShallowColor, vec4(tmpvar_10)).xyz;
  tmpvar_12.w = tmpvar_11;
  tmpvar_3 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = vec4((tmpvar_11 + _Params.z));
  tmpvar_4 = tmpvar_13;
  highp vec3 tmpvar_14;
  tmpvar_14 = (_Object2World * _glesVertex).xyz;
  tmpvar_2.xy = ((tmpvar_14.xz * _Params2.x) + fract(((_Time.yy * _Params2.y) * vec2(-0.393919, 0.919145))));
  tmpvar_2.zw = ((tmpvar_14.xz * _Params2.x) + fract((vec2(0.464238, 0.185695) * (_Time.yy * _Params2.y))));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_6;
  xlv_TEXCOORD3 = (_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz);
  xlv_COLOR = tmpvar_3;
  xlv_COLOR1 = tmpvar_4;
}



#endif
#ifdef FRAGMENT

uniform lowp samplerCube _EnvTex;
uniform sampler2D _Normals;
uniform highp vec4 _Params;
uniform highp vec4 _DeepColor;
uniform highp vec4 _ShallowColor;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec3 color_1;
  mediump float fresnel_2;
  mediump vec3 viewDir_3;
  lowp vec4 res_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_Normals, xlv_TEXCOORD1.xy);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_Normals, xlv_TEXCOORD1.zw);
  mediump vec3 n1_7;
  n1_7 = ((tmpvar_5.xyz * 2.0) - 1.0);
  mediump vec3 n2_8;
  n2_8 = ((tmpvar_6.xyz * 2.0) - 1.0);
  mediump vec3 tmpvar_9;
  tmpvar_9.xy = (n1_7.xy + n2_8.xy);
  tmpvar_9.z = (n1_7.z * n2_8.z);
  mediump vec3 tmpvar_10;
  tmpvar_10 = normalize(tmpvar_9);
  mediump vec3 n2_11;
  n2_11 = xlv_TEXCOORD2;
  mediump vec3 tmpvar_12;
  tmpvar_12.xy = (tmpvar_10.xz + n2_11.xy);
  tmpvar_12.z = (tmpvar_10.y * n2_11.z);
  mediump vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_12);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(xlv_TEXCOORD3);
  viewDir_3 = tmpvar_14;
  mediump vec3 tmpvar_15;
  mediump vec3 i_16;
  i_16 = -(viewDir_3);
  tmpvar_15 = (i_16 - (2.0 * (dot (tmpvar_13, i_16) * tmpvar_13)));
  mediump float tmpvar_17;
  tmpvar_17 = (1.0 - clamp (dot (viewDir_3, tmpvar_13), 0.0, 1.0));
  highp float facing_18;
  facing_18 = tmpvar_17;
  highp float tmpvar_19;
  tmpvar_19 = clamp ((_Params.x + ((1.0 - _Params.x) * pow (facing_18, _Params.y))), 0.0, 1.0);
  fresnel_2 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = mix (_DeepColor, _ShallowColor, vec4(tmpvar_17)).xyz;
  color_1 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (fresnel_2 + _Params.z);
  res_4.w = tmpvar_21;
  lowp vec4 tmpvar_22;
  tmpvar_22 = textureCube (_EnvTex, tmpvar_15);
  mediump vec3 tmpvar_23;
  tmpvar_23 = mix (color_1, tmpvar_22.xyz, vec3(fresnel_2));
  res_4.xyz = tmpvar_23;
  gl_FragData[0] = res_4;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_VERY_HIGH" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _Params;
uniform highp vec4 _Params2;
uniform highp vec4 _DeepColor;
uniform highp vec4 _ShallowColor;
out highp vec3 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
void main ()
{
  highp vec3 tmpvar_1;
  highp vec4 tmpvar_2;
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_6;
  tmpvar_6 = normalize((tmpvar_5 * normalize(_glesNormal)));
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  highp vec3 tmpvar_8;
  highp vec3 i_9;
  i_9 = -(tmpvar_7);
  tmpvar_8 = (i_9 - (2.0 * (dot (tmpvar_6, i_9) * tmpvar_6)));
  highp float tmpvar_10;
  tmpvar_10 = (1.0 - clamp (dot (tmpvar_7, tmpvar_6), 0.0, 1.0));
  highp float tmpvar_11;
  tmpvar_11 = clamp ((_Params.x + ((1.0 - _Params.x) * pow (tmpvar_10, _Params.y))), 0.0, 1.0);
  tmpvar_1.yz = tmpvar_8.yz;
  tmpvar_1.x = -(tmpvar_8.x);
  highp vec4 tmpvar_12;
  tmpvar_12.xyz = mix (_DeepColor, _ShallowColor, vec4(tmpvar_10)).xyz;
  tmpvar_12.w = tmpvar_11;
  tmpvar_3 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = vec4((tmpvar_11 + _Params.z));
  tmpvar_4 = tmpvar_13;
  highp vec3 tmpvar_14;
  tmpvar_14 = (_Object2World * _glesVertex).xyz;
  tmpvar_2.xy = ((tmpvar_14.xz * _Params2.x) + fract(((_Time.yy * _Params2.y) * vec2(-0.393919, 0.919145))));
  tmpvar_2.zw = ((tmpvar_14.xz * _Params2.x) + fract((vec2(0.464238, 0.185695) * (_Time.yy * _Params2.y))));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_6;
  xlv_TEXCOORD3 = (_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz);
  xlv_COLOR = tmpvar_3;
  xlv_COLOR1 = tmpvar_4;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform lowp samplerCube _EnvTex;
uniform sampler2D _Normals;
uniform highp vec4 _Params;
uniform highp vec4 _DeepColor;
uniform highp vec4 _ShallowColor;
in highp vec4 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
in highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec3 color_1;
  mediump float fresnel_2;
  mediump vec3 viewDir_3;
  lowp vec4 res_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture (_Normals, xlv_TEXCOORD1.xy);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_Normals, xlv_TEXCOORD1.zw);
  mediump vec3 n1_7;
  n1_7 = ((tmpvar_5.xyz * 2.0) - 1.0);
  mediump vec3 n2_8;
  n2_8 = ((tmpvar_6.xyz * 2.0) - 1.0);
  mediump vec3 tmpvar_9;
  tmpvar_9.xy = (n1_7.xy + n2_8.xy);
  tmpvar_9.z = (n1_7.z * n2_8.z);
  mediump vec3 tmpvar_10;
  tmpvar_10 = normalize(tmpvar_9);
  mediump vec3 n2_11;
  n2_11 = xlv_TEXCOORD2;
  mediump vec3 tmpvar_12;
  tmpvar_12.xy = (tmpvar_10.xz + n2_11.xy);
  tmpvar_12.z = (tmpvar_10.y * n2_11.z);
  mediump vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_12);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(xlv_TEXCOORD3);
  viewDir_3 = tmpvar_14;
  mediump vec3 tmpvar_15;
  mediump vec3 i_16;
  i_16 = -(viewDir_3);
  tmpvar_15 = (i_16 - (2.0 * (dot (tmpvar_13, i_16) * tmpvar_13)));
  mediump float tmpvar_17;
  tmpvar_17 = (1.0 - clamp (dot (viewDir_3, tmpvar_13), 0.0, 1.0));
  highp float facing_18;
  facing_18 = tmpvar_17;
  highp float tmpvar_19;
  tmpvar_19 = clamp ((_Params.x + ((1.0 - _Params.x) * pow (facing_18, _Params.y))), 0.0, 1.0);
  fresnel_2 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = mix (_DeepColor, _ShallowColor, vec4(tmpvar_17)).xyz;
  color_1 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (fresnel_2 + _Params.z);
  res_4.w = tmpvar_21;
  lowp vec4 tmpvar_22;
  tmpvar_22 = texture (_EnvTex, tmpvar_15);
  mediump vec3 tmpvar_23;
  tmpvar_23 = mix (color_1, tmpvar_22.xyz, vec3(fresnel_2));
  res_4.xyz = tmpvar_23;
  _glesFragData[0] = res_4;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_ULTRA" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _Params;
uniform highp vec4 _Params2;
uniform highp vec4 _DeepColor;
uniform highp vec4 _ShallowColor;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
void main ()
{
  highp vec3 tmpvar_1;
  highp vec4 tmpvar_2;
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_6;
  tmpvar_6 = normalize((tmpvar_5 * normalize(_glesNormal)));
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  highp vec3 tmpvar_8;
  highp vec3 i_9;
  i_9 = -(tmpvar_7);
  tmpvar_8 = (i_9 - (2.0 * (dot (tmpvar_6, i_9) * tmpvar_6)));
  highp float tmpvar_10;
  tmpvar_10 = (1.0 - clamp (dot (tmpvar_7, tmpvar_6), 0.0, 1.0));
  highp float tmpvar_11;
  tmpvar_11 = clamp ((_Params.x + ((1.0 - _Params.x) * pow (tmpvar_10, _Params.y))), 0.0, 1.0);
  tmpvar_1.yz = tmpvar_8.yz;
  tmpvar_1.x = -(tmpvar_8.x);
  highp vec4 tmpvar_12;
  tmpvar_12.xyz = mix (_DeepColor, _ShallowColor, vec4(tmpvar_10)).xyz;
  tmpvar_12.w = tmpvar_11;
  tmpvar_3 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = vec4((tmpvar_11 + _Params.z));
  tmpvar_4 = tmpvar_13;
  highp vec3 tmpvar_14;
  tmpvar_14 = (_Object2World * _glesVertex).xyz;
  tmpvar_2.xy = ((tmpvar_14.xz * _Params2.x) + fract(((_Time.yy * _Params2.y) * vec2(-0.393919, 0.919145))));
  tmpvar_2.zw = ((tmpvar_14.xz * _Params2.x) + fract((vec2(0.464238, 0.185695) * (_Time.yy * _Params2.y))));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_6;
  xlv_TEXCOORD3 = (_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz);
  xlv_COLOR = tmpvar_3;
  xlv_COLOR1 = tmpvar_4;
}



#endif
#ifdef FRAGMENT

uniform lowp samplerCube _EnvTex;
uniform sampler2D _Normals;
uniform highp vec4 _Params;
uniform highp vec4 _DeepColor;
uniform highp vec4 _ShallowColor;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec3 color_1;
  mediump float fresnel_2;
  mediump vec3 viewDir_3;
  lowp vec4 res_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_Normals, xlv_TEXCOORD1.xy);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_Normals, xlv_TEXCOORD1.zw);
  mediump vec3 n1_7;
  n1_7 = ((tmpvar_5.xyz * 2.0) - 1.0);
  mediump vec3 n2_8;
  n2_8 = ((tmpvar_6.xyz * 2.0) - 1.0);
  mediump vec3 tmpvar_9;
  tmpvar_9.xy = (n1_7.xy + n2_8.xy);
  tmpvar_9.z = (n1_7.z * n2_8.z);
  mediump vec3 tmpvar_10;
  tmpvar_10 = normalize(tmpvar_9);
  mediump vec3 n2_11;
  n2_11 = xlv_TEXCOORD2;
  mediump vec3 tmpvar_12;
  tmpvar_12.xy = (tmpvar_10.xz + n2_11.xy);
  tmpvar_12.z = (tmpvar_10.y * n2_11.z);
  mediump vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_12);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(xlv_TEXCOORD3);
  viewDir_3 = tmpvar_14;
  mediump vec3 tmpvar_15;
  mediump vec3 i_16;
  i_16 = -(viewDir_3);
  tmpvar_15 = (i_16 - (2.0 * (dot (tmpvar_13, i_16) * tmpvar_13)));
  mediump float tmpvar_17;
  tmpvar_17 = (1.0 - clamp (dot (viewDir_3, tmpvar_13), 0.0, 1.0));
  highp float facing_18;
  facing_18 = tmpvar_17;
  highp float tmpvar_19;
  tmpvar_19 = clamp ((_Params.x + ((1.0 - _Params.x) * pow (facing_18, _Params.y))), 0.0, 1.0);
  fresnel_2 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = mix (_DeepColor, _ShallowColor, vec4(tmpvar_17)).xyz;
  color_1 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (fresnel_2 + _Params.z);
  res_4.w = tmpvar_21;
  lowp vec4 tmpvar_22;
  tmpvar_22 = textureCube (_EnvTex, tmpvar_15);
  mediump vec3 tmpvar_23;
  tmpvar_23 = mix (color_1, tmpvar_22.xyz, vec3(fresnel_2));
  res_4.xyz = tmpvar_23;
  gl_FragData[0] = res_4;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_ULTRA" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _Params;
uniform highp vec4 _Params2;
uniform highp vec4 _DeepColor;
uniform highp vec4 _ShallowColor;
out highp vec3 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
void main ()
{
  highp vec3 tmpvar_1;
  highp vec4 tmpvar_2;
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_6;
  tmpvar_6 = normalize((tmpvar_5 * normalize(_glesNormal)));
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  highp vec3 tmpvar_8;
  highp vec3 i_9;
  i_9 = -(tmpvar_7);
  tmpvar_8 = (i_9 - (2.0 * (dot (tmpvar_6, i_9) * tmpvar_6)));
  highp float tmpvar_10;
  tmpvar_10 = (1.0 - clamp (dot (tmpvar_7, tmpvar_6), 0.0, 1.0));
  highp float tmpvar_11;
  tmpvar_11 = clamp ((_Params.x + ((1.0 - _Params.x) * pow (tmpvar_10, _Params.y))), 0.0, 1.0);
  tmpvar_1.yz = tmpvar_8.yz;
  tmpvar_1.x = -(tmpvar_8.x);
  highp vec4 tmpvar_12;
  tmpvar_12.xyz = mix (_DeepColor, _ShallowColor, vec4(tmpvar_10)).xyz;
  tmpvar_12.w = tmpvar_11;
  tmpvar_3 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = vec4((tmpvar_11 + _Params.z));
  tmpvar_4 = tmpvar_13;
  highp vec3 tmpvar_14;
  tmpvar_14 = (_Object2World * _glesVertex).xyz;
  tmpvar_2.xy = ((tmpvar_14.xz * _Params2.x) + fract(((_Time.yy * _Params2.y) * vec2(-0.393919, 0.919145))));
  tmpvar_2.zw = ((tmpvar_14.xz * _Params2.x) + fract((vec2(0.464238, 0.185695) * (_Time.yy * _Params2.y))));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_6;
  xlv_TEXCOORD3 = (_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz);
  xlv_COLOR = tmpvar_3;
  xlv_COLOR1 = tmpvar_4;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform lowp samplerCube _EnvTex;
uniform sampler2D _Normals;
uniform highp vec4 _Params;
uniform highp vec4 _DeepColor;
uniform highp vec4 _ShallowColor;
in highp vec4 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
in highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec3 color_1;
  mediump float fresnel_2;
  mediump vec3 viewDir_3;
  lowp vec4 res_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture (_Normals, xlv_TEXCOORD1.xy);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_Normals, xlv_TEXCOORD1.zw);
  mediump vec3 n1_7;
  n1_7 = ((tmpvar_5.xyz * 2.0) - 1.0);
  mediump vec3 n2_8;
  n2_8 = ((tmpvar_6.xyz * 2.0) - 1.0);
  mediump vec3 tmpvar_9;
  tmpvar_9.xy = (n1_7.xy + n2_8.xy);
  tmpvar_9.z = (n1_7.z * n2_8.z);
  mediump vec3 tmpvar_10;
  tmpvar_10 = normalize(tmpvar_9);
  mediump vec3 n2_11;
  n2_11 = xlv_TEXCOORD2;
  mediump vec3 tmpvar_12;
  tmpvar_12.xy = (tmpvar_10.xz + n2_11.xy);
  tmpvar_12.z = (tmpvar_10.y * n2_11.z);
  mediump vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_12);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(xlv_TEXCOORD3);
  viewDir_3 = tmpvar_14;
  mediump vec3 tmpvar_15;
  mediump vec3 i_16;
  i_16 = -(viewDir_3);
  tmpvar_15 = (i_16 - (2.0 * (dot (tmpvar_13, i_16) * tmpvar_13)));
  mediump float tmpvar_17;
  tmpvar_17 = (1.0 - clamp (dot (viewDir_3, tmpvar_13), 0.0, 1.0));
  highp float facing_18;
  facing_18 = tmpvar_17;
  highp float tmpvar_19;
  tmpvar_19 = clamp ((_Params.x + ((1.0 - _Params.x) * pow (facing_18, _Params.y))), 0.0, 1.0);
  fresnel_2 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = mix (_DeepColor, _ShallowColor, vec4(tmpvar_17)).xyz;
  color_1 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (fresnel_2 + _Params.z);
  res_4.w = tmpvar_21;
  lowp vec4 tmpvar_22;
  tmpvar_22 = texture (_EnvTex, tmpvar_15);
  mediump vec3 tmpvar_23;
  tmpvar_23 = mix (color_1, tmpvar_22.xyz, vec3(fresnel_2));
  res_4.xyz = tmpvar_23;
  _glesFragData[0] = res_4;
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
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_ULTRA" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_ULTRA" }
"!!GLES3"
}
}
 }
}
}