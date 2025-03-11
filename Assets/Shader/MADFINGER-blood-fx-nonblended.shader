þShader "MADFINGER/FX/Blood FX nonblended" {
Properties {
 _Params ("Params", Vector) = (0,1,0,0)
 _Color ("Color", Color) = (0,0,0,0)
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
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
uniform highp vec4 _Time;
uniform highp vec4 _Params;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2 = normalize(-(_glesVertex.xy));
  highp float tmpvar_3;
  tmpvar_3 = ((_Time.y * _Params.y) / 1.25);
  highp float tmpvar_4;
  tmpvar_4 = (fract(abs(tmpvar_3)) * 1.25);
  highp float tmpvar_5;
  if ((tmpvar_3 >= 0.0)) {
    tmpvar_5 = tmpvar_4;
  } else {
    tmpvar_5 = -(tmpvar_4);
  };
  highp float tmpvar_6;
  if ((tmpvar_5 < 0.5)) {
    highp float tmpvar_7;
    tmpvar_7 = (tmpvar_5 / 0.5);
    highp float tmpvar_8;
    tmpvar_8 = (fract(abs(tmpvar_7)) * 0.5);
    highp float tmpvar_9;
    if ((tmpvar_7 >= 0.0)) {
      tmpvar_9 = tmpvar_8;
    } else {
      tmpvar_9 = -(tmpvar_8);
    };
    highp float tmpvar_10;
    tmpvar_10 = (20.0 * tmpvar_9);
    tmpvar_6 = (tmpvar_10 * exp((1.0 - tmpvar_10)));
  } else {
    tmpvar_6 = 0.0;
  };
  highp float tmpvar_11;
  if ((_Params.x > 0.0)) {
    tmpvar_11 = mix (0.1, 0.0, _Params.x);
  } else {
    tmpvar_11 = 0.0;
  };
  highp vec4 tmpvar_12;
  tmpvar_12.zw = vec2(0.0, 1.0);
  tmpvar_12.xy = (_glesVertex.xy * 2.0);
  tmpvar_1.zw = tmpvar_12.zw;
  tmpvar_1.xy = (tmpvar_12.xy - ((tmpvar_2 * (_Params.x + (tmpvar_6 * tmpvar_11))) * _glesColor.w));
  gl_Position = tmpvar_1;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Color;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _Color;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesColor;
uniform highp vec4 _Time;
uniform highp vec4 _Params;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2 = normalize(-(_glesVertex.xy));
  highp float tmpvar_3;
  tmpvar_3 = ((_Time.y * _Params.y) / 1.25);
  highp float tmpvar_4;
  tmpvar_4 = (fract(abs(tmpvar_3)) * 1.25);
  highp float tmpvar_5;
  if ((tmpvar_3 >= 0.0)) {
    tmpvar_5 = tmpvar_4;
  } else {
    tmpvar_5 = -(tmpvar_4);
  };
  highp float tmpvar_6;
  if ((tmpvar_5 < 0.5)) {
    highp float tmpvar_7;
    tmpvar_7 = (tmpvar_5 / 0.5);
    highp float tmpvar_8;
    tmpvar_8 = (fract(abs(tmpvar_7)) * 0.5);
    highp float tmpvar_9;
    if ((tmpvar_7 >= 0.0)) {
      tmpvar_9 = tmpvar_8;
    } else {
      tmpvar_9 = -(tmpvar_8);
    };
    highp float tmpvar_10;
    tmpvar_10 = (20.0 * tmpvar_9);
    tmpvar_6 = (tmpvar_10 * exp((1.0 - tmpvar_10)));
  } else {
    tmpvar_6 = 0.0;
  };
  highp float tmpvar_11;
  if ((_Params.x > 0.0)) {
    tmpvar_11 = mix (0.1, 0.0, _Params.x);
  } else {
    tmpvar_11 = 0.0;
  };
  highp vec4 tmpvar_12;
  tmpvar_12.zw = vec2(0.0, 1.0);
  tmpvar_12.xy = (_glesVertex.xy * 2.0);
  tmpvar_1.zw = tmpvar_12.zw;
  tmpvar_1.xy = (tmpvar_12.xy - ((tmpvar_2 * (_Params.x + (tmpvar_6 * tmpvar_11))) * _glesColor.w));
  gl_Position = tmpvar_1;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform highp vec4 _Color;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _Color;
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