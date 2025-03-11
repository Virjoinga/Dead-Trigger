Â%Shader "MADFINGER/FX/Blood FX blended" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _Params ("Params", Vector) = (0,1,0,0)
 _ColorBooster ("Color booster", Color) = (0.5,0,0,0)
}
SubShader { 
 Tags { "QUEUE"="Overlay" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Overlay" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZTest Always
  ZWrite Off
  Cull Off
  Fog {
   Color (0,0,0,0)
  }
  Blend DstColor SrcColor
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp vec4 _Params;
uniform highp vec4 _ColorBooster;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  highp vec4 tmpvar_1;
  lowp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = normalize(-(_glesVertex.xy));
  highp float tmpvar_4;
  tmpvar_4 = ((_Time.y * _Params.y) / 1.25);
  highp float tmpvar_5;
  tmpvar_5 = (fract(abs(tmpvar_4)) * 1.25);
  highp float tmpvar_6;
  if ((tmpvar_4 >= 0.0)) {
    tmpvar_6 = tmpvar_5;
  } else {
    tmpvar_6 = -(tmpvar_5);
  };
  highp float tmpvar_7;
  if ((tmpvar_6 < 0.5)) {
    highp float tmpvar_8;
    tmpvar_8 = (tmpvar_6 / 0.5);
    highp float tmpvar_9;
    tmpvar_9 = (fract(abs(tmpvar_8)) * 0.5);
    highp float tmpvar_10;
    if ((tmpvar_8 >= 0.0)) {
      tmpvar_10 = tmpvar_9;
    } else {
      tmpvar_10 = -(tmpvar_9);
    };
    highp float tmpvar_11;
    tmpvar_11 = (20.0 * tmpvar_10);
    tmpvar_7 = (tmpvar_11 * exp((1.0 - tmpvar_11)));
  } else {
    tmpvar_7 = 0.0;
  };
  highp float tmpvar_12;
  if ((_Params.x > 0.0)) {
    tmpvar_12 = mix (0.1, 0.0, _Params.x);
  } else {
    tmpvar_12 = 0.0;
  };
  highp float tmpvar_13;
  if ((_Params.x > 0.0)) {
    tmpvar_13 = mix (1.0, 0.0, _Params.x);
  } else {
    tmpvar_13 = 0.0;
  };
  highp vec4 tmpvar_14;
  tmpvar_14.zw = vec2(0.0, 1.0);
  tmpvar_14.xy = (_glesVertex.xy * 2.0);
  tmpvar_1.zw = tmpvar_14.zw;
  highp vec4 tmpvar_15;
  tmpvar_15.xyz = (((_ColorBooster.xyz * tmpvar_13) * _glesColor.w) * tmpvar_7);
  tmpvar_15.w = _glesColor.w;
  tmpvar_2 = tmpvar_15;
  tmpvar_1.xy = (tmpvar_14.xy - ((tmpvar_3 * (_Params.x + (tmpvar_7 * tmpvar_12))) * _glesColor.w));
  gl_Position = tmpvar_1;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_COLOR = tmpvar_2;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD0);
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
uniform highp vec4 _Params;
uniform highp vec4 _ColorBooster;
out highp vec2 xlv_TEXCOORD0;
out lowp vec4 xlv_COLOR;
void main ()
{
  highp vec4 tmpvar_1;
  lowp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = normalize(-(_glesVertex.xy));
  highp float tmpvar_4;
  tmpvar_4 = ((_Time.y * _Params.y) / 1.25);
  highp float tmpvar_5;
  tmpvar_5 = (fract(abs(tmpvar_4)) * 1.25);
  highp float tmpvar_6;
  if ((tmpvar_4 >= 0.0)) {
    tmpvar_6 = tmpvar_5;
  } else {
    tmpvar_6 = -(tmpvar_5);
  };
  highp float tmpvar_7;
  if ((tmpvar_6 < 0.5)) {
    highp float tmpvar_8;
    tmpvar_8 = (tmpvar_6 / 0.5);
    highp float tmpvar_9;
    tmpvar_9 = (fract(abs(tmpvar_8)) * 0.5);
    highp float tmpvar_10;
    if ((tmpvar_8 >= 0.0)) {
      tmpvar_10 = tmpvar_9;
    } else {
      tmpvar_10 = -(tmpvar_9);
    };
    highp float tmpvar_11;
    tmpvar_11 = (20.0 * tmpvar_10);
    tmpvar_7 = (tmpvar_11 * exp((1.0 - tmpvar_11)));
  } else {
    tmpvar_7 = 0.0;
  };
  highp float tmpvar_12;
  if ((_Params.x > 0.0)) {
    tmpvar_12 = mix (0.1, 0.0, _Params.x);
  } else {
    tmpvar_12 = 0.0;
  };
  highp float tmpvar_13;
  if ((_Params.x > 0.0)) {
    tmpvar_13 = mix (1.0, 0.0, _Params.x);
  } else {
    tmpvar_13 = 0.0;
  };
  highp vec4 tmpvar_14;
  tmpvar_14.zw = vec2(0.0, 1.0);
  tmpvar_14.xy = (_glesVertex.xy * 2.0);
  tmpvar_1.zw = tmpvar_14.zw;
  highp vec4 tmpvar_15;
  tmpvar_15.xyz = (((_ColorBooster.xyz * tmpvar_13) * _glesColor.w) * tmpvar_7);
  tmpvar_15.w = _glesColor.w;
  tmpvar_2 = tmpvar_15;
  tmpvar_1.xy = (tmpvar_14.xy - ((tmpvar_3 * (_Params.x + (tmpvar_7 * tmpvar_12))) * _glesColor.w));
  gl_Position = tmpvar_1;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_COLOR = tmpvar_2;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
in highp vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture (_MainTex, xlv_TEXCOORD0);
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