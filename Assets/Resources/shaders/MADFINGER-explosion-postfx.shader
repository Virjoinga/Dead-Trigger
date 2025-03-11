…WShader "MADFINGER/PostFX/ExplosionFX" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "" {}
 _UVOffsAndAspectScale ("UVOffsAndAspectScale", Vector) = (0,0,0,0)
 _Wave0ParamSet0 ("Wave0ParamSet0", Vector) = (0,0,0,0)
 _Wave0ParamSet1 ("Wave0ParamSet1", Vector) = (0,0,0,0)
 _Wave1ParamSet0 ("Wave1ParamSet0", Vector) = (0,0,0,0)
 _Wave1ParamSet1 ("Wave1ParamSet1", Vector) = (0,0,0,0)
 _Wave2ParamSet0 ("Wave2ParamSet0", Vector) = (0,0,0,0)
 _Wave2ParamSet1 ("Wave2ParamSet1", Vector) = (0,0,0,0)
 _Wave3ParamSet0 ("Wave3ParamSet0", Vector) = (0,0,0,0)
 _Wave3ParamSet1 ("Wave3ParamSet1", Vector) = (0,0,0,0)
 _Color0 ("Color0", Color) = (1,1,1,0)
 _Color1 ("Color1", Color) = (0.5,0.3,0,1)
 _Params ("Params", Vector) = (0,0,0,0)
}
SubShader { 
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
uniform highp vec4 _UVOffsAndAspectScale;
uniform highp vec4 _Wave0ParamSet0;
uniform highp vec4 _Wave0ParamSet1;
uniform highp vec4 _Wave1ParamSet0;
uniform highp vec4 _Wave1ParamSet1;
uniform highp vec4 _Wave2ParamSet0;
uniform highp vec4 _Wave2ParamSet1;
uniform highp vec4 _Color0;
uniform highp vec4 _Color1;
uniform highp vec4 _Params;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec2 tmpvar_2;
  lowp vec4 tmpvar_3;
  highp vec2 tmpvar_4;
  highp vec2 tmpvar_5;
  tmpvar_5 = ((_glesVertex.xy - _Wave0ParamSet0.xy) * _UVOffsAndAspectScale.zw);
  highp float tmpvar_6;
  tmpvar_6 = sqrt(dot (tmpvar_5, tmpvar_5));
  highp float time_7;
  time_7 = (max ((_Wave0ParamSet1.z - (tmpvar_6 / _Wave0ParamSet1.y)), 0.0) * _Wave0ParamSet0.w);
  highp float tmpvar_8;
  tmpvar_8 = ((sin(time_7) * (1.0/((1.0 + (time_7 * time_7))))) * _Wave0ParamSet0.z);
  highp float tmpvar_9;
  tmpvar_9 = clamp ((tmpvar_6 * _Wave0ParamSet1.x), 0.0, 1.0);
  highp float tmpvar_10;
  tmpvar_10 = (max (_Wave0ParamSet1.z, 0.0) * 16.25);
  highp float tmpvar_11;
  tmpvar_11 = ((tmpvar_10 * exp((1.0 - tmpvar_10))) + 0.0001);
  highp float tmpvar_12;
  tmpvar_12 = (1.0/(_Wave0ParamSet1.x));
  highp float tmpvar_13;
  if ((tmpvar_12 > 0.65)) {
    tmpvar_13 = (tmpvar_12 * 2.0);
  } else {
    tmpvar_13 = tmpvar_12;
  };
  highp float tmpvar_14;
  tmpvar_14 = (1.0 - clamp ((tmpvar_6 / (tmpvar_13 * tmpvar_11)), 0.0, 1.0));
  highp vec4 tmpvar_15;
  tmpvar_15 = ((((((tmpvar_14 * tmpvar_14) * _Wave0ParamSet1.x) * mix (_Color1, _Color0, vec4(tmpvar_11))) * 1.5) * tmpvar_11) * _Params.x);
  tmpvar_4 = ((tmpvar_8 * (tmpvar_5 / tmpvar_6)) * (1.0 - (tmpvar_9 * tmpvar_9)));
  highp vec2 tmpvar_16;
  highp vec2 tmpvar_17;
  tmpvar_17 = ((_glesVertex.xy - _Wave1ParamSet0.xy) * _UVOffsAndAspectScale.zw);
  highp float tmpvar_18;
  tmpvar_18 = sqrt(dot (tmpvar_17, tmpvar_17));
  highp float time_19;
  time_19 = (max ((_Wave1ParamSet1.z - (tmpvar_18 / _Wave1ParamSet1.y)), 0.0) * _Wave1ParamSet0.w);
  highp float tmpvar_20;
  tmpvar_20 = ((sin(time_19) * (1.0/((1.0 + (time_19 * time_19))))) * _Wave1ParamSet0.z);
  highp float tmpvar_21;
  tmpvar_21 = clamp ((tmpvar_18 * _Wave1ParamSet1.x), 0.0, 1.0);
  highp float tmpvar_22;
  tmpvar_22 = (max (_Wave1ParamSet1.z, 0.0) * 16.25);
  highp float tmpvar_23;
  tmpvar_23 = ((tmpvar_22 * exp((1.0 - tmpvar_22))) + 0.0001);
  highp float tmpvar_24;
  tmpvar_24 = (1.0/(_Wave1ParamSet1.x));
  highp float tmpvar_25;
  if ((tmpvar_24 > 0.65)) {
    tmpvar_25 = (tmpvar_24 * 2.0);
  } else {
    tmpvar_25 = tmpvar_24;
  };
  highp float tmpvar_26;
  tmpvar_26 = (1.0 - clamp ((tmpvar_18 / (tmpvar_25 * tmpvar_23)), 0.0, 1.0));
  highp vec4 tmpvar_27;
  tmpvar_27 = ((((((tmpvar_26 * tmpvar_26) * _Wave1ParamSet1.x) * mix (_Color1, _Color0, vec4(tmpvar_23))) * 1.5) * tmpvar_23) * _Params.x);
  tmpvar_16 = ((tmpvar_20 * (tmpvar_17 / tmpvar_18)) * (1.0 - (tmpvar_21 * tmpvar_21)));
  highp vec2 tmpvar_28;
  tmpvar_28 = ((_glesVertex.xy - _Wave2ParamSet0.xy) * _UVOffsAndAspectScale.zw);
  highp float tmpvar_29;
  tmpvar_29 = sqrt(dot (tmpvar_28, tmpvar_28));
  highp float time_30;
  time_30 = (max ((_Wave2ParamSet1.z - (tmpvar_29 / _Wave2ParamSet1.y)), 0.0) * _Wave2ParamSet0.w);
  highp float tmpvar_31;
  tmpvar_31 = ((sin(time_30) * (1.0/((1.0 + (time_30 * time_30))))) * _Wave2ParamSet0.z);
  highp float tmpvar_32;
  tmpvar_32 = clamp ((tmpvar_29 * _Wave2ParamSet1.x), 0.0, 1.0);
  highp float tmpvar_33;
  tmpvar_33 = (max (_Wave2ParamSet1.z, 0.0) * 16.25);
  highp float tmpvar_34;
  tmpvar_34 = ((tmpvar_33 * exp((1.0 - tmpvar_33))) + 0.0001);
  highp float tmpvar_35;
  tmpvar_35 = (1.0/(_Wave2ParamSet1.x));
  highp float tmpvar_36;
  if ((tmpvar_35 > 0.65)) {
    tmpvar_36 = (tmpvar_35 * 2.0);
  } else {
    tmpvar_36 = tmpvar_35;
  };
  highp float tmpvar_37;
  tmpvar_37 = (1.0 - clamp ((tmpvar_29 / (tmpvar_36 * tmpvar_34)), 0.0, 1.0));
  highp vec4 tmpvar_38;
  tmpvar_38 = ((((((tmpvar_37 * tmpvar_37) * _Wave2ParamSet1.x) * mix (_Color1, _Color0, vec4(tmpvar_34))) * 1.5) * tmpvar_34) * _Params.x);
  highp vec4 tmpvar_39;
  tmpvar_39.zw = vec2(0.0, 1.0);
  tmpvar_39.xy = ((_glesVertex.xy * 2.0) - vec2(1.0, 1.0));
  tmpvar_1 = tmpvar_39;
  tmpvar_2 = ((_glesVertex.xy + _UVOffsAndAspectScale.xy) + ((tmpvar_4 + tmpvar_16) + ((tmpvar_31 * (tmpvar_28 / tmpvar_29)) * (1.0 - (tmpvar_32 * tmpvar_32)))));
  int tmpvar_40;
  if ((_UVOffsAndAspectScale.y < 0.0)) {
    tmpvar_40 = -1;
  } else {
    tmpvar_40 = 1;
  };
  tmpvar_1.y = (tmpvar_39.y * float(tmpvar_40));
  highp vec4 tmpvar_41;
  tmpvar_41 = ((tmpvar_15 + tmpvar_27) + tmpvar_38);
  tmpvar_3 = tmpvar_41;
  gl_Position = tmpvar_1;
  xlv_TEXCOORD0 = tmpvar_2;
  xlv_COLOR = tmpvar_3;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = (texture2D (_MainTex, xlv_TEXCOORD0) + xlv_COLOR);
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
uniform highp vec4 _UVOffsAndAspectScale;
uniform highp vec4 _Wave0ParamSet0;
uniform highp vec4 _Wave0ParamSet1;
uniform highp vec4 _Wave1ParamSet0;
uniform highp vec4 _Wave1ParamSet1;
uniform highp vec4 _Wave2ParamSet0;
uniform highp vec4 _Wave2ParamSet1;
uniform highp vec4 _Color0;
uniform highp vec4 _Color1;
uniform highp vec4 _Params;
out highp vec2 xlv_TEXCOORD0;
out lowp vec4 xlv_COLOR;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec2 tmpvar_2;
  lowp vec4 tmpvar_3;
  highp vec2 tmpvar_4;
  highp vec2 tmpvar_5;
  tmpvar_5 = ((_glesVertex.xy - _Wave0ParamSet0.xy) * _UVOffsAndAspectScale.zw);
  highp float tmpvar_6;
  tmpvar_6 = sqrt(dot (tmpvar_5, tmpvar_5));
  highp float time_7;
  time_7 = (max ((_Wave0ParamSet1.z - (tmpvar_6 / _Wave0ParamSet1.y)), 0.0) * _Wave0ParamSet0.w);
  highp float tmpvar_8;
  tmpvar_8 = ((sin(time_7) * (1.0/((1.0 + (time_7 * time_7))))) * _Wave0ParamSet0.z);
  highp float tmpvar_9;
  tmpvar_9 = clamp ((tmpvar_6 * _Wave0ParamSet1.x), 0.0, 1.0);
  highp float tmpvar_10;
  tmpvar_10 = (max (_Wave0ParamSet1.z, 0.0) * 16.25);
  highp float tmpvar_11;
  tmpvar_11 = ((tmpvar_10 * exp((1.0 - tmpvar_10))) + 0.0001);
  highp float tmpvar_12;
  tmpvar_12 = (1.0/(_Wave0ParamSet1.x));
  highp float tmpvar_13;
  if ((tmpvar_12 > 0.65)) {
    tmpvar_13 = (tmpvar_12 * 2.0);
  } else {
    tmpvar_13 = tmpvar_12;
  };
  highp float tmpvar_14;
  tmpvar_14 = (1.0 - clamp ((tmpvar_6 / (tmpvar_13 * tmpvar_11)), 0.0, 1.0));
  highp vec4 tmpvar_15;
  tmpvar_15 = ((((((tmpvar_14 * tmpvar_14) * _Wave0ParamSet1.x) * mix (_Color1, _Color0, vec4(tmpvar_11))) * 1.5) * tmpvar_11) * _Params.x);
  tmpvar_4 = ((tmpvar_8 * (tmpvar_5 / tmpvar_6)) * (1.0 - (tmpvar_9 * tmpvar_9)));
  highp vec2 tmpvar_16;
  highp vec2 tmpvar_17;
  tmpvar_17 = ((_glesVertex.xy - _Wave1ParamSet0.xy) * _UVOffsAndAspectScale.zw);
  highp float tmpvar_18;
  tmpvar_18 = sqrt(dot (tmpvar_17, tmpvar_17));
  highp float time_19;
  time_19 = (max ((_Wave1ParamSet1.z - (tmpvar_18 / _Wave1ParamSet1.y)), 0.0) * _Wave1ParamSet0.w);
  highp float tmpvar_20;
  tmpvar_20 = ((sin(time_19) * (1.0/((1.0 + (time_19 * time_19))))) * _Wave1ParamSet0.z);
  highp float tmpvar_21;
  tmpvar_21 = clamp ((tmpvar_18 * _Wave1ParamSet1.x), 0.0, 1.0);
  highp float tmpvar_22;
  tmpvar_22 = (max (_Wave1ParamSet1.z, 0.0) * 16.25);
  highp float tmpvar_23;
  tmpvar_23 = ((tmpvar_22 * exp((1.0 - tmpvar_22))) + 0.0001);
  highp float tmpvar_24;
  tmpvar_24 = (1.0/(_Wave1ParamSet1.x));
  highp float tmpvar_25;
  if ((tmpvar_24 > 0.65)) {
    tmpvar_25 = (tmpvar_24 * 2.0);
  } else {
    tmpvar_25 = tmpvar_24;
  };
  highp float tmpvar_26;
  tmpvar_26 = (1.0 - clamp ((tmpvar_18 / (tmpvar_25 * tmpvar_23)), 0.0, 1.0));
  highp vec4 tmpvar_27;
  tmpvar_27 = ((((((tmpvar_26 * tmpvar_26) * _Wave1ParamSet1.x) * mix (_Color1, _Color0, vec4(tmpvar_23))) * 1.5) * tmpvar_23) * _Params.x);
  tmpvar_16 = ((tmpvar_20 * (tmpvar_17 / tmpvar_18)) * (1.0 - (tmpvar_21 * tmpvar_21)));
  highp vec2 tmpvar_28;
  tmpvar_28 = ((_glesVertex.xy - _Wave2ParamSet0.xy) * _UVOffsAndAspectScale.zw);
  highp float tmpvar_29;
  tmpvar_29 = sqrt(dot (tmpvar_28, tmpvar_28));
  highp float time_30;
  time_30 = (max ((_Wave2ParamSet1.z - (tmpvar_29 / _Wave2ParamSet1.y)), 0.0) * _Wave2ParamSet0.w);
  highp float tmpvar_31;
  tmpvar_31 = ((sin(time_30) * (1.0/((1.0 + (time_30 * time_30))))) * _Wave2ParamSet0.z);
  highp float tmpvar_32;
  tmpvar_32 = clamp ((tmpvar_29 * _Wave2ParamSet1.x), 0.0, 1.0);
  highp float tmpvar_33;
  tmpvar_33 = (max (_Wave2ParamSet1.z, 0.0) * 16.25);
  highp float tmpvar_34;
  tmpvar_34 = ((tmpvar_33 * exp((1.0 - tmpvar_33))) + 0.0001);
  highp float tmpvar_35;
  tmpvar_35 = (1.0/(_Wave2ParamSet1.x));
  highp float tmpvar_36;
  if ((tmpvar_35 > 0.65)) {
    tmpvar_36 = (tmpvar_35 * 2.0);
  } else {
    tmpvar_36 = tmpvar_35;
  };
  highp float tmpvar_37;
  tmpvar_37 = (1.0 - clamp ((tmpvar_29 / (tmpvar_36 * tmpvar_34)), 0.0, 1.0));
  highp vec4 tmpvar_38;
  tmpvar_38 = ((((((tmpvar_37 * tmpvar_37) * _Wave2ParamSet1.x) * mix (_Color1, _Color0, vec4(tmpvar_34))) * 1.5) * tmpvar_34) * _Params.x);
  highp vec4 tmpvar_39;
  tmpvar_39.zw = vec2(0.0, 1.0);
  tmpvar_39.xy = ((_glesVertex.xy * 2.0) - vec2(1.0, 1.0));
  tmpvar_1 = tmpvar_39;
  tmpvar_2 = ((_glesVertex.xy + _UVOffsAndAspectScale.xy) + ((tmpvar_4 + tmpvar_16) + ((tmpvar_31 * (tmpvar_28 / tmpvar_29)) * (1.0 - (tmpvar_32 * tmpvar_32)))));
  int tmpvar_40;
  if ((_UVOffsAndAspectScale.y < 0.0)) {
    tmpvar_40 = -1;
  } else {
    tmpvar_40 = 1;
  };
  tmpvar_1.y = (tmpvar_39.y * float(tmpvar_40));
  highp vec4 tmpvar_41;
  tmpvar_41 = ((tmpvar_15 + tmpvar_27) + tmpvar_38);
  tmpvar_3 = tmpvar_41;
  gl_Position = tmpvar_1;
  xlv_TEXCOORD0 = tmpvar_2;
  xlv_COLOR = tmpvar_3;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
in highp vec2 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = (texture (_MainTex, xlv_TEXCOORD0) + xlv_COLOR);
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
Fallback Off
}