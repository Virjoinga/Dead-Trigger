ù*Shader "MADFINGER/FX/Geom wave" {
Properties {
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
 _WaveSpeed ("Waves speed", Vector) = (0.25,0.25,0.25,0.25)
 _WaveSize ("Waves size", Vector) = (0.25,0.25,0.25,0.25)
 _WaveAmplitude ("Waves amp", Vector) = (1,0,0,0)
 _WaveBias ("Waves bias", Vector) = (0,0,0,0)
 _WaveTimeOffs ("Wave time offs", Vector) = (0,0,0,0)
}
SubShader { 
 LOD 100
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
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _WaveSpeed;
uniform highp vec4 _WaveSize;
uniform highp vec4 _WaveAmplitude;
uniform highp vec4 _WaveBias;
uniform highp vec4 _WaveTimeOffs;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec4 tmpvar_2;
  tmpvar_2 = _glesVertex;
  highp vec4 tmpvar_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = abs((fract((_glesColor.zzzz - (-(_WaveSize) + ((_Time.y + _WaveTimeOffs) * _WaveSpeed)))) - _WaveSize));
  highp vec4 tmpvar_5;
  tmpvar_5 = (tmpvar_4 / _WaveSize);
  bvec4 tmpvar_6;
  tmpvar_6 = greaterThan (tmpvar_4, _WaveSize);
  highp vec4 c_7;
  c_7 = (vec4(1.0, 1.0, 1.0, 1.0) - ((tmpvar_5 * tmpvar_5) * (vec4(3.0, 3.0, 3.0, 3.0) - (2.0 * tmpvar_5))));
  highp float tmpvar_8;
  if (tmpvar_6.x) {
    tmpvar_8 = 0.0;
  } else {
    tmpvar_8 = c_7.x;
  };
  highp float tmpvar_9;
  if (tmpvar_6.y) {
    tmpvar_9 = 0.0;
  } else {
    tmpvar_9 = c_7.y;
  };
  highp float tmpvar_10;
  if (tmpvar_6.z) {
    tmpvar_10 = 0.0;
  } else {
    tmpvar_10 = c_7.z;
  };
  highp float tmpvar_11;
  if (tmpvar_6.w) {
    tmpvar_11 = 0.0;
  } else {
    tmpvar_11 = c_7.w;
  };
  highp vec4 tmpvar_12;
  tmpvar_12.x = tmpvar_8;
  tmpvar_12.y = tmpvar_9;
  tmpvar_12.z = tmpvar_10;
  tmpvar_12.w = tmpvar_11;
  tmpvar_2.xyz = (_glesVertex.xyz + (((tmpvar_1 * dot ((tmpvar_12 + _WaveBias), _WaveAmplitude)) * unity_Scale.w) * _glesColor.w));
  tmpvar_3.xy = _glesMultiTexCoord0.xy;
  gl_Position = (glstate_matrix_mvp * tmpvar_2);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D unity_Lightmap;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz * (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz));
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
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _WaveSpeed;
uniform highp vec4 _WaveSize;
uniform highp vec4 _WaveAmplitude;
uniform highp vec4 _WaveBias;
uniform highp vec4 _WaveTimeOffs;
out highp vec4 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec4 tmpvar_2;
  tmpvar_2 = _glesVertex;
  highp vec4 tmpvar_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = abs((fract((_glesColor.zzzz - (-(_WaveSize) + ((_Time.y + _WaveTimeOffs) * _WaveSpeed)))) - _WaveSize));
  highp vec4 tmpvar_5;
  tmpvar_5 = (tmpvar_4 / _WaveSize);
  bvec4 tmpvar_6;
  tmpvar_6 = greaterThan (tmpvar_4, _WaveSize);
  highp vec4 c_7;
  c_7 = (vec4(1.0, 1.0, 1.0, 1.0) - ((tmpvar_5 * tmpvar_5) * (vec4(3.0, 3.0, 3.0, 3.0) - (2.0 * tmpvar_5))));
  highp float tmpvar_8;
  if (tmpvar_6.x) {
    tmpvar_8 = 0.0;
  } else {
    tmpvar_8 = c_7.x;
  };
  highp float tmpvar_9;
  if (tmpvar_6.y) {
    tmpvar_9 = 0.0;
  } else {
    tmpvar_9 = c_7.y;
  };
  highp float tmpvar_10;
  if (tmpvar_6.z) {
    tmpvar_10 = 0.0;
  } else {
    tmpvar_10 = c_7.z;
  };
  highp float tmpvar_11;
  if (tmpvar_6.w) {
    tmpvar_11 = 0.0;
  } else {
    tmpvar_11 = c_7.w;
  };
  highp vec4 tmpvar_12;
  tmpvar_12.x = tmpvar_8;
  tmpvar_12.y = tmpvar_9;
  tmpvar_12.z = tmpvar_10;
  tmpvar_12.w = tmpvar_11;
  tmpvar_2.xyz = (_glesVertex.xyz + (((tmpvar_1 * dot ((tmpvar_12 + _WaveBias), _WaveAmplitude)) * unity_Scale.w) * _glesColor.w));
  tmpvar_3.xy = _glesMultiTexCoord0.xy;
  gl_Position = (glstate_matrix_mvp * tmpvar_2);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D unity_Lightmap;
in highp vec4 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0.xy);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz * (2.0 * texture (unity_Lightmap, xlv_TEXCOORD1).xyz));
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