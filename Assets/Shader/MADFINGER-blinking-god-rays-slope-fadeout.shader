þ;Shader "MADFINGER/Transparent/Blinking GodRays - slope fadeout" {
Properties {
 _MainTex ("Base texture", 2D) = "white" {}
 _FadeOutDistNear ("Near fadeout dist", Float) = 10
 _FadeOutDistFar ("Far fadeout dist", Float) = 10000
 _Multiplier ("Color multiplier", Float) = 1
 _Bias ("Bias", Float) = 0
 _TimeOnDuration ("ON duration", Float) = 0.5
 _TimeOffDuration ("OFF duration", Float) = 0.5
 _BlinkingTimeOffsScale ("Blinking time offset scale (seconds)", Float) = 5
 _SizeGrowStartDist ("Size grow start dist", Float) = 5
 _SizeGrowEndDist ("Size grow end dist", Float) = 50
 _MaxGrowSize ("Max grow size", Float) = 2.5
 _NoiseAmount ("Noise amount (when zero, pulse wave is used)", Range(0,0.5)) = 0
 _Color ("Color", Color) = (1,1,1,1)
}
SubShader { 
 LOD 100
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Fog {
   Color (0,0,0,0)
  }
  Blend One One
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp float _FadeOutDistNear;
uniform highp float _FadeOutDistFar;
uniform highp float _Multiplier;
uniform highp float _Bias;
uniform highp float _TimeOnDuration;
uniform highp float _TimeOffDuration;
uniform highp float _BlinkingTimeOffsScale;
uniform highp float _NoiseAmount;
uniform highp vec4 _Color;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  highp float tmpvar_3;
  tmpvar_3 = (_Time.y + (_BlinkingTimeOffsScale * _glesColor.z));
  highp vec3 tmpvar_4;
  tmpvar_4 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp float tmpvar_5;
  tmpvar_5 = sqrt(dot (tmpvar_4, tmpvar_4));
  highp float tmpvar_6;
  tmpvar_6 = clamp ((tmpvar_5 / _FadeOutDistNear), 0.0, 1.0);
  highp float tmpvar_7;
  tmpvar_7 = (1.0 - clamp ((max ((tmpvar_5 - _FadeOutDistFar), 0.0) * 0.2), 0.0, 1.0));
  highp float y_8;
  y_8 = (_TimeOnDuration + _TimeOffDuration);
  highp float tmpvar_9;
  tmpvar_9 = (tmpvar_3 / y_8);
  highp float tmpvar_10;
  tmpvar_10 = (fract(abs(tmpvar_9)) * y_8);
  highp float tmpvar_11;
  if ((tmpvar_9 >= 0.0)) {
    tmpvar_11 = tmpvar_10;
  } else {
    tmpvar_11 = -(tmpvar_10);
  };
  highp float t_12;
  t_12 = max (min ((tmpvar_11 / (_TimeOnDuration * 0.25)), 1.0), 0.0);
  highp float edge0_13;
  edge0_13 = (_TimeOnDuration * 0.75);
  highp float t_14;
  t_14 = max (min (((tmpvar_11 - edge0_13) / (_TimeOnDuration - edge0_13)), 1.0), 0.0);
  highp float tmpvar_15;
  tmpvar_15 = ((t_12 * (t_12 * (3.0 - (2.0 * t_12)))) * (1.0 - (t_14 * (t_14 * (3.0 - (2.0 * t_14))))));
  highp float tmpvar_16;
  tmpvar_16 = (tmpvar_3 * (6.28319 / _TimeOnDuration));
  highp float tmpvar_17;
  tmpvar_17 = ((_NoiseAmount * (sin(tmpvar_16) * ((0.5 * cos(((tmpvar_16 * 0.6366) + 56.7272))) + 0.5))) + (1.0 - _NoiseAmount));
  mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp float tmpvar_19;
  tmpvar_19 = clamp (abs(dot (normalize((tmpvar_18 * tmpvar_1)), normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz)))), 0.0, 1.0);
  highp float tmpvar_20;
  if ((_NoiseAmount < 0.01)) {
    tmpvar_20 = tmpvar_15;
  } else {
    tmpvar_20 = tmpvar_17;
  };
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_6 * tmpvar_6);
  highp vec4 tmpvar_22;
  tmpvar_22 = ((((((tmpvar_21 * tmpvar_21) * (tmpvar_7 * tmpvar_7)) * _Color) * _Multiplier) * (tmpvar_20 + _Bias)) * tmpvar_19);
  tmpvar_2 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_2;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = (texture2D (_MainTex, xlv_TEXCOORD0) * xlv_TEXCOORD1);
  gl_FragData[0] = tmpvar_1;
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
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp float _FadeOutDistNear;
uniform highp float _FadeOutDistFar;
uniform highp float _Multiplier;
uniform highp float _Bias;
uniform highp float _TimeOnDuration;
uniform highp float _TimeOffDuration;
uniform highp float _BlinkingTimeOffsScale;
uniform highp float _NoiseAmount;
uniform highp vec4 _Color;
out highp vec2 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  highp float tmpvar_3;
  tmpvar_3 = (_Time.y + (_BlinkingTimeOffsScale * _glesColor.z));
  highp vec3 tmpvar_4;
  tmpvar_4 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp float tmpvar_5;
  tmpvar_5 = sqrt(dot (tmpvar_4, tmpvar_4));
  highp float tmpvar_6;
  tmpvar_6 = clamp ((tmpvar_5 / _FadeOutDistNear), 0.0, 1.0);
  highp float tmpvar_7;
  tmpvar_7 = (1.0 - clamp ((max ((tmpvar_5 - _FadeOutDistFar), 0.0) * 0.2), 0.0, 1.0));
  highp float y_8;
  y_8 = (_TimeOnDuration + _TimeOffDuration);
  highp float tmpvar_9;
  tmpvar_9 = (tmpvar_3 / y_8);
  highp float tmpvar_10;
  tmpvar_10 = (fract(abs(tmpvar_9)) * y_8);
  highp float tmpvar_11;
  if ((tmpvar_9 >= 0.0)) {
    tmpvar_11 = tmpvar_10;
  } else {
    tmpvar_11 = -(tmpvar_10);
  };
  highp float t_12;
  t_12 = max (min ((tmpvar_11 / (_TimeOnDuration * 0.25)), 1.0), 0.0);
  highp float edge0_13;
  edge0_13 = (_TimeOnDuration * 0.75);
  highp float t_14;
  t_14 = max (min (((tmpvar_11 - edge0_13) / (_TimeOnDuration - edge0_13)), 1.0), 0.0);
  highp float tmpvar_15;
  tmpvar_15 = ((t_12 * (t_12 * (3.0 - (2.0 * t_12)))) * (1.0 - (t_14 * (t_14 * (3.0 - (2.0 * t_14))))));
  highp float tmpvar_16;
  tmpvar_16 = (tmpvar_3 * (6.28319 / _TimeOnDuration));
  highp float tmpvar_17;
  tmpvar_17 = ((_NoiseAmount * (sin(tmpvar_16) * ((0.5 * cos(((tmpvar_16 * 0.6366) + 56.7272))) + 0.5))) + (1.0 - _NoiseAmount));
  mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp float tmpvar_19;
  tmpvar_19 = clamp (abs(dot (normalize((tmpvar_18 * tmpvar_1)), normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz)))), 0.0, 1.0);
  highp float tmpvar_20;
  if ((_NoiseAmount < 0.01)) {
    tmpvar_20 = tmpvar_15;
  } else {
    tmpvar_20 = tmpvar_17;
  };
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_6 * tmpvar_6);
  highp vec4 tmpvar_22;
  tmpvar_22 = ((((((tmpvar_21 * tmpvar_21) * (tmpvar_7 * tmpvar_7)) * _Color) * _Multiplier) * (tmpvar_20 + _Bias)) * tmpvar_19);
  tmpvar_2 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_2;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
in highp vec2 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = (texture (_MainTex, xlv_TEXCOORD0) * xlv_TEXCOORD1);
  _glesFragData[0] = tmpvar_1;
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