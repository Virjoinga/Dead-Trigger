Å|Shader "MADFINGER/Environment/Cubemap specular + Lightmap - transparent" {
Properties {
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
 _TransparencyMaskTex ("Transparency Mask (RGB - multiply, A - blend control", 2D) = "white" {}
 _SpecCubeTex ("SpecCube", CUBE) = "black" {}
 _SpecularStrength ("Specular strength", Float) = 1
 _ScrollingSpeed ("Scrolling speed", Vector) = (0,0,0,0)
}
SubShader { 
 LOD 100
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
Program "vp" {
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_LOW" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _ScrollingSpeed;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _TransparencyMaskTex;
uniform sampler2D unity_Lightmap;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_TransparencyMaskTex, xlv_TEXCOORD0);
  c_1.xyz = (tmpvar_2.xyz * tmpvar_3.xyz);
  c_1.xyz = (c_1.xyz * (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz));
  c_1.w = tmpvar_3.w;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_LOW" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _ScrollingSpeed;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _TransparencyMaskTex;
uniform sampler2D unity_Lightmap;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_TransparencyMaskTex, xlv_TEXCOORD0);
  c_1.xyz = (tmpvar_2.xyz * tmpvar_3.xyz);
  c_1.xyz = (c_1.xyz * (2.0 * texture (unity_Lightmap, xlv_TEXCOORD1).xyz));
  c_1.w = tmpvar_3.w;
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
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _ScrollingSpeed;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
void main ()
{
  mat3 tmpvar_1;
  tmpvar_1[0] = _Object2World[0].xyz;
  tmpvar_1[1] = _Object2World[1].xyz;
  tmpvar_1[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize((tmpvar_1 * normalize(_glesNormal)));
  highp vec3 i_3;
  i_3 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = (i_3 - (2.0 * (dot (tmpvar_2, i_3) * tmpvar_2)));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _TransparencyMaskTex;
uniform lowp samplerCube _SpecCubeTex;
uniform sampler2D unity_Lightmap;
uniform highp float _SpecularStrength;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_TransparencyMaskTex, xlv_TEXCOORD0);
  c_1.xyz = (tmpvar_2.xyz * tmpvar_3.xyz);
  lowp vec4 tmpvar_4;
  tmpvar_4 = textureCube (_SpecCubeTex, xlv_TEXCOORD2);
  highp vec3 tmpvar_5;
  tmpvar_5 = (c_1.xyz + ((tmpvar_4 * _SpecularStrength) * tmpvar_2.w).xyz);
  c_1.xyz = tmpvar_5;
  c_1.xyz = (c_1.xyz * (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz));
  c_1.w = tmpvar_3.w;
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
in vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _ScrollingSpeed;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
void main ()
{
  mat3 tmpvar_1;
  tmpvar_1[0] = _Object2World[0].xyz;
  tmpvar_1[1] = _Object2World[1].xyz;
  tmpvar_1[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize((tmpvar_1 * normalize(_glesNormal)));
  highp vec3 i_3;
  i_3 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = (i_3 - (2.0 * (dot (tmpvar_2, i_3) * tmpvar_2)));
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _TransparencyMaskTex;
uniform lowp samplerCube _SpecCubeTex;
uniform sampler2D unity_Lightmap;
uniform highp float _SpecularStrength;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_TransparencyMaskTex, xlv_TEXCOORD0);
  c_1.xyz = (tmpvar_2.xyz * tmpvar_3.xyz);
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_SpecCubeTex, xlv_TEXCOORD2);
  highp vec3 tmpvar_5;
  tmpvar_5 = (c_1.xyz + ((tmpvar_4 * _SpecularStrength) * tmpvar_2.w).xyz);
  c_1.xyz = tmpvar_5;
  c_1.xyz = (c_1.xyz * (2.0 * texture (unity_Lightmap, xlv_TEXCOORD1).xyz));
  c_1.w = tmpvar_3.w;
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
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _ScrollingSpeed;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
void main ()
{
  mat3 tmpvar_1;
  tmpvar_1[0] = _Object2World[0].xyz;
  tmpvar_1[1] = _Object2World[1].xyz;
  tmpvar_1[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize((tmpvar_1 * normalize(_glesNormal)));
  highp vec3 i_3;
  i_3 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = (i_3 - (2.0 * (dot (tmpvar_2, i_3) * tmpvar_2)));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _TransparencyMaskTex;
uniform lowp samplerCube _SpecCubeTex;
uniform sampler2D unity_Lightmap;
uniform highp float _SpecularStrength;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_TransparencyMaskTex, xlv_TEXCOORD0);
  c_1.xyz = (tmpvar_2.xyz * tmpvar_3.xyz);
  lowp vec4 tmpvar_4;
  tmpvar_4 = textureCube (_SpecCubeTex, xlv_TEXCOORD2);
  highp vec3 tmpvar_5;
  tmpvar_5 = (c_1.xyz + ((tmpvar_4 * _SpecularStrength) * tmpvar_2.w).xyz);
  c_1.xyz = tmpvar_5;
  c_1.xyz = (c_1.xyz * (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz));
  c_1.w = tmpvar_3.w;
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
in vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _ScrollingSpeed;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
void main ()
{
  mat3 tmpvar_1;
  tmpvar_1[0] = _Object2World[0].xyz;
  tmpvar_1[1] = _Object2World[1].xyz;
  tmpvar_1[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize((tmpvar_1 * normalize(_glesNormal)));
  highp vec3 i_3;
  i_3 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = (i_3 - (2.0 * (dot (tmpvar_2, i_3) * tmpvar_2)));
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _TransparencyMaskTex;
uniform lowp samplerCube _SpecCubeTex;
uniform sampler2D unity_Lightmap;
uniform highp float _SpecularStrength;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_TransparencyMaskTex, xlv_TEXCOORD0);
  c_1.xyz = (tmpvar_2.xyz * tmpvar_3.xyz);
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_SpecCubeTex, xlv_TEXCOORD2);
  highp vec3 tmpvar_5;
  tmpvar_5 = (c_1.xyz + ((tmpvar_4 * _SpecularStrength) * tmpvar_2.w).xyz);
  c_1.xyz = tmpvar_5;
  c_1.xyz = (c_1.xyz * (2.0 * texture (unity_Lightmap, xlv_TEXCOORD1).xyz));
  c_1.w = tmpvar_3.w;
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
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _ScrollingSpeed;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
void main ()
{
  mat3 tmpvar_1;
  tmpvar_1[0] = _Object2World[0].xyz;
  tmpvar_1[1] = _Object2World[1].xyz;
  tmpvar_1[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize((tmpvar_1 * normalize(_glesNormal)));
  highp vec3 i_3;
  i_3 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = (i_3 - (2.0 * (dot (tmpvar_2, i_3) * tmpvar_2)));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _TransparencyMaskTex;
uniform lowp samplerCube _SpecCubeTex;
uniform sampler2D unity_Lightmap;
uniform highp float _SpecularStrength;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_TransparencyMaskTex, xlv_TEXCOORD0);
  c_1.xyz = (tmpvar_2.xyz * tmpvar_3.xyz);
  lowp vec4 tmpvar_4;
  tmpvar_4 = textureCube (_SpecCubeTex, xlv_TEXCOORD2);
  highp vec3 tmpvar_5;
  tmpvar_5 = (c_1.xyz + ((tmpvar_4 * _SpecularStrength) * tmpvar_2.w).xyz);
  c_1.xyz = tmpvar_5;
  c_1.xyz = (c_1.xyz * (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz));
  c_1.w = tmpvar_3.w;
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
in vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _ScrollingSpeed;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
void main ()
{
  mat3 tmpvar_1;
  tmpvar_1[0] = _Object2World[0].xyz;
  tmpvar_1[1] = _Object2World[1].xyz;
  tmpvar_1[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize((tmpvar_1 * normalize(_glesNormal)));
  highp vec3 i_3;
  i_3 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = (i_3 - (2.0 * (dot (tmpvar_2, i_3) * tmpvar_2)));
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _TransparencyMaskTex;
uniform lowp samplerCube _SpecCubeTex;
uniform sampler2D unity_Lightmap;
uniform highp float _SpecularStrength;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_TransparencyMaskTex, xlv_TEXCOORD0);
  c_1.xyz = (tmpvar_2.xyz * tmpvar_3.xyz);
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_SpecCubeTex, xlv_TEXCOORD2);
  highp vec3 tmpvar_5;
  tmpvar_5 = (c_1.xyz + ((tmpvar_4 * _SpecularStrength) * tmpvar_2.w).xyz);
  c_1.xyz = tmpvar_5;
  c_1.xyz = (c_1.xyz * (2.0 * texture (unity_Lightmap, xlv_TEXCOORD1).xyz));
  c_1.w = tmpvar_3.w;
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