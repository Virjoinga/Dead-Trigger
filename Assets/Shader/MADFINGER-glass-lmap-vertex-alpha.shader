È!Shader "MADFINGER/Glass/Lightmap + cube env + per vertex alpha" {
Properties {
 _MainTex ("Diffuse", 2D) = "white" {}
 _EnvMap ("EnvMap", CUBE) = "black" {}
 _ReflectivityMaskWeights ("Reflectivity Mask Weights (RGB)", Vector) = (0.3,0.59,0.11,0)
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
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying lowp vec4 xlv_COLOR;
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
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = (i_3 - (2.0 * (dot (tmpvar_2, i_3) * tmpvar_2)));
  xlv_COLOR = _glesColor;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform lowp samplerCube _EnvMap;
uniform sampler2D unity_Lightmap;
uniform highp vec4 _ReflectivityMaskWeights;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  lowp vec4 tmpvar_3;
  tmpvar_3 = textureCube (_EnvMap, xlv_TEXCOORD2);
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_2.xyz + (tmpvar_3 * dot (tmpvar_2.xyz, _ReflectivityMaskWeights.xyz)).xyz);
  c_1.xyz = tmpvar_4;
  c_1.xyz = (c_1.xyz * (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz));
  c_1.w = xlv_COLOR.w;
  gl_FragData[0] = c_1;
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
in vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
out lowp vec4 xlv_COLOR;
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
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = (i_3 - (2.0 * (dot (tmpvar_2, i_3) * tmpvar_2)));
  xlv_COLOR = _glesColor;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform lowp samplerCube _EnvMap;
uniform sampler2D unity_Lightmap;
uniform highp vec4 _ReflectivityMaskWeights;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
in lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_EnvMap, xlv_TEXCOORD2);
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_2.xyz + (tmpvar_3 * dot (tmpvar_2.xyz, _ReflectivityMaskWeights.xyz)).xyz);
  c_1.xyz = tmpvar_4;
  c_1.xyz = (c_1.xyz * (2.0 * texture (unity_Lightmap, xlv_TEXCOORD1).xyz));
  c_1.w = xlv_COLOR.w;
  _glesFragData[0] = c_1;
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