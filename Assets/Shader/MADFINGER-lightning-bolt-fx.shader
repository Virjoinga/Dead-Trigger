ßYShader "MADFINGER/FX/Lightning bolt FX" {
Properties {
 _MainTex ("Main tex", 2D) = "white" {}
 _Duration ("Duration (x - ON, y - OFF)", Vector) = (0.5,2,0,0)
 _Amplitude ("Amplitude (x - horizontal, y - vertical)", Vector) = (0.2,0.01,0,0)
 _NoiseFreqs ("NoiseFreqs", Vector) = (4,8,16,32)
 _NoiseSpeeds ("NoiseSpeeds", Vector) = (3.2,2.3,0.5,1)
 _NoiseAmps ("NoiseAmps", Vector) = (2,1,0.5,0.125)
 _OtherParams ("x - line width, y - inv wave speed, z - aspect ratio", Vector) = (0.2,0.025,0,0)
 _Color ("Color", Color) = (0.7,0.8,1.3,1)
}
SubShader { 
 LOD 100
 Tags { "QUEUE"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" }
  ZWrite Off
  Cull Off
  Fog { Mode Off }
  Blend One One
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _Duration;
uniform highp vec4 _Amplitude;
uniform highp vec4 _NoiseFreqs;
uniform highp vec4 _NoiseSpeeds;
uniform highp vec4 _NoiseAmps;
uniform highp vec4 _Color;
uniform highp vec4 _OtherParams;
varying highp vec2 xlv_TEXCOORD;
varying lowp vec4 xlv_TEXCOORD1;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec4 tmpvar_2;
  tmpvar_2 = _glesVertex;
  highp vec2 offsDir2D_3;
  highp vec4 sdir_4;
  highp vec4 tmpvar_5;
  lowp vec4 tmpvar_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize(tmpvar_1);
  highp float tmpvar_8;
  tmpvar_8 = abs(tmpvar_7.y);
  vec3 tmpvar_9;
  if ((tmpvar_8 > 0.999)) {
    tmpvar_9 = vec3(0.0, 0.0, 1.0);
  } else {
    tmpvar_9 = vec3(0.0, 1.0, 0.0);
  };
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize(((tmpvar_9.yzx * tmpvar_7.zxy) - (tmpvar_9.zxy * tmpvar_7.yzx)));
  highp vec3 tmpvar_11;
  tmpvar_11 = ((tmpvar_7.yzx * tmpvar_10.zxy) - (tmpvar_7.zxy * tmpvar_10.yzx));
  highp float tmpvar_12;
  tmpvar_12 = (_glesMultiTexCoord0.x * 0.2);
  highp float tmpvar_13;
  tmpvar_13 = ((_Time.y * 0.5) + (_glesMultiTexCoord1.x * 536.375));
  highp vec4 tmpvar_14;
  tmpvar_14 = ((fract(((12.4512 * vec4((_glesMultiTexCoord1.x * 63.2771))) + 0.5)) * 2.0) - 1.0);
  highp vec4 tmpvar_15;
  tmpvar_15 = (vec4(0.7, 0.7, 0.7, 0.7) + (fract((175034.0 * (tmpvar_14 - (tmpvar_14 * abs(tmpvar_14))))) * 0.3));
  highp float tmpvar_16;
  tmpvar_16 = (_Duration.x * tmpvar_15.x);
  highp float y_17;
  y_17 = (tmpvar_16 + (_Duration.y * tmpvar_15.y));
  highp float tmpvar_18;
  tmpvar_18 = ((tmpvar_13 - (tmpvar_12 * _OtherParams.y)) / y_17);
  highp float tmpvar_19;
  tmpvar_19 = (fract(abs(tmpvar_18)) * y_17);
  highp float tmpvar_20;
  if ((tmpvar_18 >= 0.0)) {
    tmpvar_20 = tmpvar_19;
  } else {
    tmpvar_20 = -(tmpvar_19);
  };
  highp float t_21;
  t_21 = max (min ((tmpvar_20 / (tmpvar_16 * 0.1)), 1.0), 0.0);
  highp float edge0_22;
  edge0_22 = (tmpvar_16 * 0.9);
  highp float t_23;
  t_23 = max (min (((tmpvar_20 - edge0_22) / (tmpvar_16 - edge0_22)), 1.0), 0.0);
  highp float tmpvar_24;
  tmpvar_24 = ((t_21 * (t_21 * (3.0 - (2.0 * t_21)))) * (1.0 - (t_23 * (t_23 * (3.0 - (2.0 * t_23))))));
  highp vec2 offs_25;
  highp vec4 tmpvar_26;
  tmpvar_26 = (((vec4(tmpvar_12) + vec4(183.818, 1041.63, 3860.17, 5575.81)) + (tmpvar_13 * _NoiseSpeeds)) * _NoiseFreqs);
  highp vec4 tmpvar_27;
  tmpvar_27 = (((vec4(tmpvar_12) + vec4(5575.81, 183.818, 1041.63, 3860.17)) + (tmpvar_13 * _NoiseSpeeds)) * _NoiseFreqs);
  highp vec4 tmpvar_28;
  tmpvar_28 = fract(tmpvar_26);
  highp vec4 tmpvar_29;
  tmpvar_29 = ((fract(((12.4512 * (tmpvar_26 - tmpvar_28)) + 0.5)) * 2.0) - 1.0);
  highp vec4 tmpvar_30;
  tmpvar_30 = ((fract(((12.4512 * ((tmpvar_26 - tmpvar_28) + 1.0)) + 0.5)) * 2.0) - 1.0);
  highp vec4 tmpvar_31;
  tmpvar_31 = fract(tmpvar_27);
  highp vec4 tmpvar_32;
  tmpvar_32 = ((fract(((12.4512 * (tmpvar_27 - tmpvar_31)) + 0.5)) * 2.0) - 1.0);
  highp vec4 tmpvar_33;
  tmpvar_33 = ((fract(((12.4512 * ((tmpvar_27 - tmpvar_31) + 1.0)) + 0.5)) * 2.0) - 1.0);
  offs_25.x = dot (((mix (fract((175034.0 * (tmpvar_29 - (tmpvar_29 * abs(tmpvar_29))))), fract((175034.0 * (tmpvar_30 - (tmpvar_30 * abs(tmpvar_30))))), ((tmpvar_28 * tmpvar_28) * (vec4(3.0, 3.0, 3.0, 3.0) - (2.0 * tmpvar_28)))) * 2.0) - 1.0), _NoiseAmps);
  offs_25.y = dot (((mix (fract((175034.0 * (tmpvar_32 - (tmpvar_32 * abs(tmpvar_32))))), fract((175034.0 * (tmpvar_33 - (tmpvar_33 * abs(tmpvar_33))))), ((tmpvar_31 * tmpvar_31) * (vec4(3.0, 3.0, 3.0, 3.0) - (2.0 * tmpvar_31)))) * 2.0) - 1.0), _NoiseAmps);
  highp vec2 tmpvar_34;
  tmpvar_34 = ((offs_25 * _Amplitude.xy) * tmpvar_24);
  tmpvar_2.xyz = ((_glesVertex.xyz + (tmpvar_10 * tmpvar_34.x)) + (tmpvar_11 * tmpvar_34.y));
  highp vec4 tmpvar_35;
  tmpvar_35.w = 1.0;
  tmpvar_35.xyz = tmpvar_2.xyz;
  highp vec4 tmpvar_36;
  tmpvar_36 = (glstate_matrix_mvp * tmpvar_35);
  highp vec4 tmpvar_37;
  tmpvar_37.w = 1.0;
  tmpvar_37.xyz = (tmpvar_2.xyz + tmpvar_1);
  highp vec4 tmpvar_38;
  tmpvar_38 = (glstate_matrix_mvp * tmpvar_37);
  sdir_4.xy = normalize(((tmpvar_38.xy / tmpvar_38.w) - (tmpvar_36.xy / tmpvar_36.w)));
  highp vec4 tmpvar_39;
  tmpvar_39 = (glstate_matrix_mvp * tmpvar_2);
  tmpvar_5.zw = tmpvar_39.zw;
  offsDir2D_3.x = 1.0;
  offsDir2D_3.y = (-1.0 * _OtherParams.z);
  tmpvar_5.xy = (tmpvar_39.xy + ((((sdir_4.yx * offsDir2D_3) * _OtherParams.x) * _glesMultiTexCoord0.y) * tmpvar_24));
  highp vec4 tmpvar_40;
  tmpvar_40.xyz = (((_Color.xyz * tmpvar_24) * clamp ((tmpvar_39.w - 1.0), 0.0, 1.0)) * 2.0);
  tmpvar_40.w = clamp ((1.0 - pow ((2.0 * abs((0.5 - _glesMultiTexCoord1.y))), 8.0)), 0.0, 1.0);
  tmpvar_6 = tmpvar_40;
  gl_Position = tmpvar_5;
  xlv_TEXCOORD = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_6;
}



#endif
#ifdef FRAGMENT

varying highp vec2 xlv_TEXCOORD;
varying lowp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp float c_1;
  highp float tmpvar_2;
  tmpvar_2 = ((1.0 - abs(xlv_TEXCOORD.y)) * xlv_TEXCOORD1.w);
  c_1 = tmpvar_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = ((c_1 * c_1) * xlv_TEXCOORD1);
  gl_FragData[0] = tmpvar_3;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _Duration;
uniform highp vec4 _Amplitude;
uniform highp vec4 _NoiseFreqs;
uniform highp vec4 _NoiseSpeeds;
uniform highp vec4 _NoiseAmps;
uniform highp vec4 _Color;
uniform highp vec4 _OtherParams;
out highp vec2 xlv_TEXCOORD;
out lowp vec4 xlv_TEXCOORD1;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec4 tmpvar_2;
  tmpvar_2 = _glesVertex;
  highp vec2 offsDir2D_3;
  highp vec4 sdir_4;
  highp vec4 tmpvar_5;
  lowp vec4 tmpvar_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize(tmpvar_1);
  highp float tmpvar_8;
  tmpvar_8 = abs(tmpvar_7.y);
  vec3 tmpvar_9;
  if ((tmpvar_8 > 0.999)) {
    tmpvar_9 = vec3(0.0, 0.0, 1.0);
  } else {
    tmpvar_9 = vec3(0.0, 1.0, 0.0);
  };
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize(((tmpvar_9.yzx * tmpvar_7.zxy) - (tmpvar_9.zxy * tmpvar_7.yzx)));
  highp vec3 tmpvar_11;
  tmpvar_11 = ((tmpvar_7.yzx * tmpvar_10.zxy) - (tmpvar_7.zxy * tmpvar_10.yzx));
  highp float tmpvar_12;
  tmpvar_12 = (_glesMultiTexCoord0.x * 0.2);
  highp float tmpvar_13;
  tmpvar_13 = ((_Time.y * 0.5) + (_glesMultiTexCoord1.x * 536.375));
  highp vec4 tmpvar_14;
  tmpvar_14 = ((fract(((12.4512 * vec4((_glesMultiTexCoord1.x * 63.2771))) + 0.5)) * 2.0) - 1.0);
  highp vec4 tmpvar_15;
  tmpvar_15 = (vec4(0.7, 0.7, 0.7, 0.7) + (fract((175034.0 * (tmpvar_14 - (tmpvar_14 * abs(tmpvar_14))))) * 0.3));
  highp float tmpvar_16;
  tmpvar_16 = (_Duration.x * tmpvar_15.x);
  highp float y_17;
  y_17 = (tmpvar_16 + (_Duration.y * tmpvar_15.y));
  highp float tmpvar_18;
  tmpvar_18 = ((tmpvar_13 - (tmpvar_12 * _OtherParams.y)) / y_17);
  highp float tmpvar_19;
  tmpvar_19 = (fract(abs(tmpvar_18)) * y_17);
  highp float tmpvar_20;
  if ((tmpvar_18 >= 0.0)) {
    tmpvar_20 = tmpvar_19;
  } else {
    tmpvar_20 = -(tmpvar_19);
  };
  highp float t_21;
  t_21 = max (min ((tmpvar_20 / (tmpvar_16 * 0.1)), 1.0), 0.0);
  highp float edge0_22;
  edge0_22 = (tmpvar_16 * 0.9);
  highp float t_23;
  t_23 = max (min (((tmpvar_20 - edge0_22) / (tmpvar_16 - edge0_22)), 1.0), 0.0);
  highp float tmpvar_24;
  tmpvar_24 = ((t_21 * (t_21 * (3.0 - (2.0 * t_21)))) * (1.0 - (t_23 * (t_23 * (3.0 - (2.0 * t_23))))));
  highp vec2 offs_25;
  highp vec4 tmpvar_26;
  tmpvar_26 = (((vec4(tmpvar_12) + vec4(183.818, 1041.63, 3860.17, 5575.81)) + (tmpvar_13 * _NoiseSpeeds)) * _NoiseFreqs);
  highp vec4 tmpvar_27;
  tmpvar_27 = (((vec4(tmpvar_12) + vec4(5575.81, 183.818, 1041.63, 3860.17)) + (tmpvar_13 * _NoiseSpeeds)) * _NoiseFreqs);
  highp vec4 tmpvar_28;
  tmpvar_28 = fract(tmpvar_26);
  highp vec4 tmpvar_29;
  tmpvar_29 = ((fract(((12.4512 * (tmpvar_26 - tmpvar_28)) + 0.5)) * 2.0) - 1.0);
  highp vec4 tmpvar_30;
  tmpvar_30 = ((fract(((12.4512 * ((tmpvar_26 - tmpvar_28) + 1.0)) + 0.5)) * 2.0) - 1.0);
  highp vec4 tmpvar_31;
  tmpvar_31 = fract(tmpvar_27);
  highp vec4 tmpvar_32;
  tmpvar_32 = ((fract(((12.4512 * (tmpvar_27 - tmpvar_31)) + 0.5)) * 2.0) - 1.0);
  highp vec4 tmpvar_33;
  tmpvar_33 = ((fract(((12.4512 * ((tmpvar_27 - tmpvar_31) + 1.0)) + 0.5)) * 2.0) - 1.0);
  offs_25.x = dot (((mix (fract((175034.0 * (tmpvar_29 - (tmpvar_29 * abs(tmpvar_29))))), fract((175034.0 * (tmpvar_30 - (tmpvar_30 * abs(tmpvar_30))))), ((tmpvar_28 * tmpvar_28) * (vec4(3.0, 3.0, 3.0, 3.0) - (2.0 * tmpvar_28)))) * 2.0) - 1.0), _NoiseAmps);
  offs_25.y = dot (((mix (fract((175034.0 * (tmpvar_32 - (tmpvar_32 * abs(tmpvar_32))))), fract((175034.0 * (tmpvar_33 - (tmpvar_33 * abs(tmpvar_33))))), ((tmpvar_31 * tmpvar_31) * (vec4(3.0, 3.0, 3.0, 3.0) - (2.0 * tmpvar_31)))) * 2.0) - 1.0), _NoiseAmps);
  highp vec2 tmpvar_34;
  tmpvar_34 = ((offs_25 * _Amplitude.xy) * tmpvar_24);
  tmpvar_2.xyz = ((_glesVertex.xyz + (tmpvar_10 * tmpvar_34.x)) + (tmpvar_11 * tmpvar_34.y));
  highp vec4 tmpvar_35;
  tmpvar_35.w = 1.0;
  tmpvar_35.xyz = tmpvar_2.xyz;
  highp vec4 tmpvar_36;
  tmpvar_36 = (glstate_matrix_mvp * tmpvar_35);
  highp vec4 tmpvar_37;
  tmpvar_37.w = 1.0;
  tmpvar_37.xyz = (tmpvar_2.xyz + tmpvar_1);
  highp vec4 tmpvar_38;
  tmpvar_38 = (glstate_matrix_mvp * tmpvar_37);
  sdir_4.xy = normalize(((tmpvar_38.xy / tmpvar_38.w) - (tmpvar_36.xy / tmpvar_36.w)));
  highp vec4 tmpvar_39;
  tmpvar_39 = (glstate_matrix_mvp * tmpvar_2);
  tmpvar_5.zw = tmpvar_39.zw;
  offsDir2D_3.x = 1.0;
  offsDir2D_3.y = (-1.0 * _OtherParams.z);
  tmpvar_5.xy = (tmpvar_39.xy + ((((sdir_4.yx * offsDir2D_3) * _OtherParams.x) * _glesMultiTexCoord0.y) * tmpvar_24));
  highp vec4 tmpvar_40;
  tmpvar_40.xyz = (((_Color.xyz * tmpvar_24) * clamp ((tmpvar_39.w - 1.0), 0.0, 1.0)) * 2.0);
  tmpvar_40.w = clamp ((1.0 - pow ((2.0 * abs((0.5 - _glesMultiTexCoord1.y))), 8.0)), 0.0, 1.0);
  tmpvar_6 = tmpvar_40;
  gl_Position = tmpvar_5;
  xlv_TEXCOORD = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_6;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
in highp vec2 xlv_TEXCOORD;
in lowp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp float c_1;
  highp float tmpvar_2;
  tmpvar_2 = ((1.0 - abs(xlv_TEXCOORD.y)) * xlv_TEXCOORD1.w);
  c_1 = tmpvar_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = ((c_1 * c_1) * xlv_TEXCOORD1);
  _glesFragData[0] = tmpvar_3;
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