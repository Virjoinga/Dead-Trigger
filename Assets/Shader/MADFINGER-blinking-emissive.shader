Ø Shader "MADFINGER/Environment/Blinking emissive" {
Properties {
 _MainTex ("Base texture", 2D) = "white" {}
 _IntensityScaleBias ("Intensity scale X / bias Y", Vector) = (1,0.1,0,0)
 _SwitchOnOffDuration ("Switch ON (X) / OFF (Y) duration", Vector) = (1,3,0,0)
 _BlinkingRate ("Blinking rate", Float) = 10
 _RndGridSize ("Randomization grid size", Float) = 5
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
uniform highp vec2 _IntensityScaleBias;
uniform highp vec2 _SwitchOnOffDuration;
uniform highp float _BlinkingRate;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
void main ()
{
  highp float seed_1;
  lowp vec4 tmpvar_2;
  lowp float tmpvar_3;
  tmpvar_3 = (dot (_glesColor, _glesColor) * 40.0);
  seed_1 = tmpvar_3;
  highp float time_4;
  time_4 = (_Time.y * _BlinkingRate);
  highp float tmpvar_5;
  tmpvar_5 = float((abs(cos(((17.0 * sin((time_4 * 5.0))) + (10.0 * sin(((seed_1 + (time_4 * 3.0)) + 7.993)))))) > 0.5));
  highp vec2 tmpvar_6;
  tmpvar_6 = (_SwitchOnOffDuration * (0.8 + (0.4 * fract(seed_1))));
  highp float y_7;
  y_7 = (tmpvar_6.x + tmpvar_6.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_Time.y + seed_1) / y_7);
  highp float tmpvar_9;
  tmpvar_9 = (fract(abs(tmpvar_8)) * y_7);
  highp float tmpvar_10;
  if ((tmpvar_8 >= 0.0)) {
    tmpvar_10 = tmpvar_9;
  } else {
    tmpvar_10 = -(tmpvar_9);
  };
  highp vec4 tmpvar_11;
  tmpvar_11 = (((_glesColor * (tmpvar_5 * float((tmpvar_10 < tmpvar_6.x)))) * _IntensityScaleBias.x) + _IntensityScaleBias.y);
  tmpvar_2 = tmpvar_11;
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
in vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec2 _IntensityScaleBias;
uniform highp vec2 _SwitchOnOffDuration;
uniform highp float _BlinkingRate;
out highp vec2 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
void main ()
{
  highp float seed_1;
  lowp vec4 tmpvar_2;
  lowp float tmpvar_3;
  tmpvar_3 = (dot (_glesColor, _glesColor) * 40.0);
  seed_1 = tmpvar_3;
  highp float time_4;
  time_4 = (_Time.y * _BlinkingRate);
  highp float tmpvar_5;
  tmpvar_5 = float((abs(cos(((17.0 * sin((time_4 * 5.0))) + (10.0 * sin(((seed_1 + (time_4 * 3.0)) + 7.993)))))) > 0.5));
  highp vec2 tmpvar_6;
  tmpvar_6 = (_SwitchOnOffDuration * (0.8 + (0.4 * fract(seed_1))));
  highp float y_7;
  y_7 = (tmpvar_6.x + tmpvar_6.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_Time.y + seed_1) / y_7);
  highp float tmpvar_9;
  tmpvar_9 = (fract(abs(tmpvar_8)) * y_7);
  highp float tmpvar_10;
  if ((tmpvar_8 >= 0.0)) {
    tmpvar_10 = tmpvar_9;
  } else {
    tmpvar_10 = -(tmpvar_9);
  };
  highp vec4 tmpvar_11;
  tmpvar_11 = (((_glesColor * (tmpvar_5 * float((tmpvar_10 < tmpvar_6.x)))) * _IntensityScaleBias.x) + _IntensityScaleBias.y);
  tmpvar_2 = tmpvar_11;
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