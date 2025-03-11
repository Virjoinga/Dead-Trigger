åBShader "MADFINGER/Transparent/Blinking GodRays FPV" {
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
 _Params ("x - FPV proj", Vector) = (1,0,0,0)
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
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_projection;
uniform highp float _FadeOutDistNear;
uniform highp float _FadeOutDistFar;
uniform highp float _Multiplier;
uniform highp float _Bias;
uniform highp float _TimeOnDuration;
uniform highp float _TimeOffDuration;
uniform highp float _BlinkingTimeOffsScale;
uniform highp float _NoiseAmount;
uniform highp vec4 _Color;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _ProjParams;
uniform highp vec4 _Params;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
void main ()
{
  highp mat4 projTM_1;
  highp vec4 tmpvar_2;
  lowp vec4 tmpvar_3;
  highp float tmpvar_4;
  tmpvar_4 = (_Time.y + (_BlinkingTimeOffsScale * _glesColor.z));
  highp vec3 tmpvar_5;
  tmpvar_5 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp float tmpvar_6;
  tmpvar_6 = sqrt(dot (tmpvar_5, tmpvar_5));
  highp float tmpvar_7;
  tmpvar_7 = clamp ((tmpvar_6 / _FadeOutDistNear), 0.0, 1.0);
  highp float tmpvar_8;
  tmpvar_8 = (1.0 - clamp ((max ((tmpvar_6 - _FadeOutDistFar), 0.0) * 0.2), 0.0, 1.0));
  highp float y_9;
  y_9 = (_TimeOnDuration + _TimeOffDuration);
  highp float tmpvar_10;
  tmpvar_10 = (tmpvar_4 / y_9);
  highp float tmpvar_11;
  tmpvar_11 = (fract(abs(tmpvar_10)) * y_9);
  highp float tmpvar_12;
  if ((tmpvar_10 >= 0.0)) {
    tmpvar_12 = tmpvar_11;
  } else {
    tmpvar_12 = -(tmpvar_11);
  };
  highp float t_13;
  t_13 = max (min ((tmpvar_12 / (_TimeOnDuration * 0.25)), 1.0), 0.0);
  highp float edge0_14;
  edge0_14 = (_TimeOnDuration * 0.75);
  highp float t_15;
  t_15 = max (min (((tmpvar_12 - edge0_14) / (_TimeOnDuration - edge0_14)), 1.0), 0.0);
  highp float tmpvar_16;
  tmpvar_16 = ((t_13 * (t_13 * (3.0 - (2.0 * t_13)))) * (1.0 - (t_15 * (t_15 * (3.0 - (2.0 * t_15))))));
  highp float tmpvar_17;
  tmpvar_17 = (tmpvar_4 * (6.28319 / _TimeOnDuration));
  highp float tmpvar_18;
  tmpvar_18 = ((_NoiseAmount * (sin(tmpvar_17) * ((0.5 * cos(((tmpvar_17 * 0.6366) + 56.7272))) + 0.5))) + (1.0 - _NoiseAmount));
  highp float tmpvar_19;
  if ((_NoiseAmount < 0.01)) {
    tmpvar_19 = tmpvar_16;
  } else {
    tmpvar_19 = tmpvar_18;
  };
  highp float tmpvar_20;
  tmpvar_20 = (tmpvar_19 + _Bias);
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_7 * tmpvar_7);
  highp float tmpvar_22;
  tmpvar_22 = ((tmpvar_21 * tmpvar_21) * (tmpvar_8 * tmpvar_8));
  projTM_1 = glstate_matrix_projection;
  highp vec4 tmpvar_23;
  if ((_Params.x > 0.0)) {
    tmpvar_23 = _ProjParams;
  } else {
    tmpvar_23 = vec4(1.0, 1.0, 1.0, 0.0);
  };
  projTM_1[0] = glstate_matrix_projection[0]; projTM_1[0].x = (glstate_matrix_projection[0].x * tmpvar_23.x);
  projTM_1[1] = projTM_1[1]; projTM_1[1].y = (projTM_1[1].y * tmpvar_23.y);
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = tmpvar_5;
  highp vec4 tmpvar_25;
  tmpvar_25 = (projTM_1 * tmpvar_24);
  tmpvar_2.xyw = tmpvar_25.xyw;
  tmpvar_2.z = (tmpvar_25.z * tmpvar_23.z);
  tmpvar_2.z = (tmpvar_2.z + (tmpvar_23.w * tmpvar_25.w));
  highp vec4 tmpvar_26;
  tmpvar_26 = (((tmpvar_22 * _Color) * _Multiplier) * tmpvar_20);
  tmpvar_3 = tmpvar_26;
  gl_Position = tmpvar_2;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_3;
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
in vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_projection;
uniform highp float _FadeOutDistNear;
uniform highp float _FadeOutDistFar;
uniform highp float _Multiplier;
uniform highp float _Bias;
uniform highp float _TimeOnDuration;
uniform highp float _TimeOffDuration;
uniform highp float _BlinkingTimeOffsScale;
uniform highp float _NoiseAmount;
uniform highp vec4 _Color;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _ProjParams;
uniform highp vec4 _Params;
out highp vec2 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
void main ()
{
  highp mat4 projTM_1;
  highp vec4 tmpvar_2;
  lowp vec4 tmpvar_3;
  highp float tmpvar_4;
  tmpvar_4 = (_Time.y + (_BlinkingTimeOffsScale * _glesColor.z));
  highp vec3 tmpvar_5;
  tmpvar_5 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp float tmpvar_6;
  tmpvar_6 = sqrt(dot (tmpvar_5, tmpvar_5));
  highp float tmpvar_7;
  tmpvar_7 = clamp ((tmpvar_6 / _FadeOutDistNear), 0.0, 1.0);
  highp float tmpvar_8;
  tmpvar_8 = (1.0 - clamp ((max ((tmpvar_6 - _FadeOutDistFar), 0.0) * 0.2), 0.0, 1.0));
  highp float y_9;
  y_9 = (_TimeOnDuration + _TimeOffDuration);
  highp float tmpvar_10;
  tmpvar_10 = (tmpvar_4 / y_9);
  highp float tmpvar_11;
  tmpvar_11 = (fract(abs(tmpvar_10)) * y_9);
  highp float tmpvar_12;
  if ((tmpvar_10 >= 0.0)) {
    tmpvar_12 = tmpvar_11;
  } else {
    tmpvar_12 = -(tmpvar_11);
  };
  highp float t_13;
  t_13 = max (min ((tmpvar_12 / (_TimeOnDuration * 0.25)), 1.0), 0.0);
  highp float edge0_14;
  edge0_14 = (_TimeOnDuration * 0.75);
  highp float t_15;
  t_15 = max (min (((tmpvar_12 - edge0_14) / (_TimeOnDuration - edge0_14)), 1.0), 0.0);
  highp float tmpvar_16;
  tmpvar_16 = ((t_13 * (t_13 * (3.0 - (2.0 * t_13)))) * (1.0 - (t_15 * (t_15 * (3.0 - (2.0 * t_15))))));
  highp float tmpvar_17;
  tmpvar_17 = (tmpvar_4 * (6.28319 / _TimeOnDuration));
  highp float tmpvar_18;
  tmpvar_18 = ((_NoiseAmount * (sin(tmpvar_17) * ((0.5 * cos(((tmpvar_17 * 0.6366) + 56.7272))) + 0.5))) + (1.0 - _NoiseAmount));
  highp float tmpvar_19;
  if ((_NoiseAmount < 0.01)) {
    tmpvar_19 = tmpvar_16;
  } else {
    tmpvar_19 = tmpvar_18;
  };
  highp float tmpvar_20;
  tmpvar_20 = (tmpvar_19 + _Bias);
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_7 * tmpvar_7);
  highp float tmpvar_22;
  tmpvar_22 = ((tmpvar_21 * tmpvar_21) * (tmpvar_8 * tmpvar_8));
  projTM_1 = glstate_matrix_projection;
  highp vec4 tmpvar_23;
  if ((_Params.x > 0.0)) {
    tmpvar_23 = _ProjParams;
  } else {
    tmpvar_23 = vec4(1.0, 1.0, 1.0, 0.0);
  };
  projTM_1[0] = glstate_matrix_projection[0]; projTM_1[0].x = (glstate_matrix_projection[0].x * tmpvar_23.x);
  projTM_1[1] = projTM_1[1]; projTM_1[1].y = (projTM_1[1].y * tmpvar_23.y);
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = tmpvar_5;
  highp vec4 tmpvar_25;
  tmpvar_25 = (projTM_1 * tmpvar_24);
  tmpvar_2.xyw = tmpvar_25.xyw;
  tmpvar_2.z = (tmpvar_25.z * tmpvar_23.z);
  tmpvar_2.z = (tmpvar_2.z + (tmpvar_23.w * tmpvar_25.w));
  highp vec4 tmpvar_26;
  tmpvar_26 = (((tmpvar_22 * _Color) * _Multiplier) * tmpvar_20);
  tmpvar_3 = tmpvar_26;
  gl_Position = tmpvar_2;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_3;
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