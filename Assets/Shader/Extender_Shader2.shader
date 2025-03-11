˜.Shader "__CAPA__/Shader_Extender2" {
Properties {
 _MainTex ("Base texture", 2D) = "white" {}
 _Multiplier ("Color multiplier", Float) = 1
 _Bias ("Bias", Float) = 0
 _TimeOnDuration ("ON duration", Float) = 0.5
 _TimeOffDuration ("OFF duration", Float) = 0.5
 _BlinkingTimeOffsScale ("Blinking time offset scale (seconds)", Float) = 5
 _NoiseAmount ("Noise amount (when zero, pulse wave is used)", Range(0,0.5)) = 0
 _Color ("Color", Color) = (1,1,1,1)
 _Scaling ("XYZ = direction, Z = MaxDist", Vector) = (0,0,1,1)
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
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex_ST;
uniform highp float _Multiplier;
uniform highp float _Bias;
uniform highp float _TimeOnDuration;
uniform highp float _TimeOffDuration;
uniform highp float _BlinkingTimeOffsScale;
uniform highp float _NoiseAmount;
uniform highp vec4 _Color;
uniform highp vec4 _Scaling;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
void main ()
{
  highp vec4 pos_1;
  lowp vec4 tmpvar_2;
  highp float tmpvar_3;
  tmpvar_3 = (_Time.y + (_BlinkingTimeOffsScale * _glesColor.z));
  highp float y_4;
  y_4 = (_TimeOnDuration + _TimeOffDuration);
  highp float tmpvar_5;
  tmpvar_5 = (tmpvar_3 / y_4);
  highp float tmpvar_6;
  tmpvar_6 = (fract(abs(tmpvar_5)) * y_4);
  highp float tmpvar_7;
  if ((tmpvar_5 >= 0.0)) {
    tmpvar_7 = tmpvar_6;
  } else {
    tmpvar_7 = -(tmpvar_6);
  };
  highp float t_8;
  t_8 = max (min ((tmpvar_7 / (_TimeOnDuration * 0.25)), 1.0), 0.0);
  highp float edge0_9;
  edge0_9 = (_TimeOnDuration * 0.75);
  highp float t_10;
  t_10 = max (min (((tmpvar_7 - edge0_9) / (_TimeOnDuration - edge0_9)), 1.0), 0.0);
  highp float tmpvar_11;
  tmpvar_11 = ((t_8 * (t_8 * (3.0 - (2.0 * t_8)))) * (1.0 - (t_10 * (t_10 * (3.0 - (2.0 * t_10))))));
  highp float tmpvar_12;
  tmpvar_12 = (tmpvar_3 * (6.28319 / _TimeOnDuration));
  highp float tmpvar_13;
  tmpvar_13 = ((_NoiseAmount * (sin(tmpvar_12) * ((0.5 * cos(((tmpvar_12 * 0.6366) + 56.7272))) + 0.5))) + (1.0 - _NoiseAmount));
  highp float tmpvar_14;
  if ((_NoiseAmount < 0.01)) {
    tmpvar_14 = tmpvar_11;
  } else {
    tmpvar_14 = tmpvar_13;
  };
  highp float tmpvar_15;
  tmpvar_15 = (tmpvar_14 + _Bias);
  pos_1 = _glesVertex;
  if ((_glesColor.x > 0.0)) {
    pos_1.xyz = (_glesVertex.xyz + (_Scaling.xyz * ((_Scaling.w - 1.0) * _glesColor.x)));
  };
  highp vec4 tmpvar_16;
  tmpvar_16 = ((_Color * _Multiplier) * tmpvar_15);
  tmpvar_2 = tmpvar_16;
  gl_Position = (glstate_matrix_mvp * pos_1);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
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
in vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex_ST;
uniform highp float _Multiplier;
uniform highp float _Bias;
uniform highp float _TimeOnDuration;
uniform highp float _TimeOffDuration;
uniform highp float _BlinkingTimeOffsScale;
uniform highp float _NoiseAmount;
uniform highp vec4 _Color;
uniform highp vec4 _Scaling;
out highp vec2 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
void main ()
{
  highp vec4 pos_1;
  lowp vec4 tmpvar_2;
  highp float tmpvar_3;
  tmpvar_3 = (_Time.y + (_BlinkingTimeOffsScale * _glesColor.z));
  highp float y_4;
  y_4 = (_TimeOnDuration + _TimeOffDuration);
  highp float tmpvar_5;
  tmpvar_5 = (tmpvar_3 / y_4);
  highp float tmpvar_6;
  tmpvar_6 = (fract(abs(tmpvar_5)) * y_4);
  highp float tmpvar_7;
  if ((tmpvar_5 >= 0.0)) {
    tmpvar_7 = tmpvar_6;
  } else {
    tmpvar_7 = -(tmpvar_6);
  };
  highp float t_8;
  t_8 = max (min ((tmpvar_7 / (_TimeOnDuration * 0.25)), 1.0), 0.0);
  highp float edge0_9;
  edge0_9 = (_TimeOnDuration * 0.75);
  highp float t_10;
  t_10 = max (min (((tmpvar_7 - edge0_9) / (_TimeOnDuration - edge0_9)), 1.0), 0.0);
  highp float tmpvar_11;
  tmpvar_11 = ((t_8 * (t_8 * (3.0 - (2.0 * t_8)))) * (1.0 - (t_10 * (t_10 * (3.0 - (2.0 * t_10))))));
  highp float tmpvar_12;
  tmpvar_12 = (tmpvar_3 * (6.28319 / _TimeOnDuration));
  highp float tmpvar_13;
  tmpvar_13 = ((_NoiseAmount * (sin(tmpvar_12) * ((0.5 * cos(((tmpvar_12 * 0.6366) + 56.7272))) + 0.5))) + (1.0 - _NoiseAmount));
  highp float tmpvar_14;
  if ((_NoiseAmount < 0.01)) {
    tmpvar_14 = tmpvar_11;
  } else {
    tmpvar_14 = tmpvar_13;
  };
  highp float tmpvar_15;
  tmpvar_15 = (tmpvar_14 + _Bias);
  pos_1 = _glesVertex;
  if ((_glesColor.x > 0.0)) {
    pos_1.xyz = (_glesVertex.xyz + (_Scaling.xyz * ((_Scaling.w - 1.0) * _glesColor.x)));
  };
  highp vec4 tmpvar_16;
  tmpvar_16 = ((_Color * _Multiplier) * tmpvar_15);
  tmpvar_2 = tmpvar_16;
  gl_Position = (glstate_matrix_mvp * pos_1);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
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