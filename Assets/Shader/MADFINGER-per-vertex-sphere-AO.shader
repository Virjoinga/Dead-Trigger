›#Shader "MADFINGER/Characters/Character shadow plane - sphere AO" {
Properties {
 _Sphere0 ("S0", Vector) = (0,0,0,0)
 _Sphere1 ("S0", Vector) = (0,0,0,0)
 _Sphere2 ("S0", Vector) = (0,0,0,0)
 _Intensity ("Intensity", Float) = 0.9
}
SubShader { 
 Tags { "QUEUE"="Transparent-15" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent-15" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Fog {
   Color (0,0,0,0)
  }
  Blend DstColor Zero
  ColorMask RGB
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _Sphere0;
uniform highp vec4 _Sphere1;
uniform highp vec4 _Sphere2;
uniform highp float _Intensity;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = (_Object2World * _glesVertex).xyz;
  mat3 tmpvar_3;
  tmpvar_3[0] = _Object2World[0].xyz;
  tmpvar_3[1] = _Object2World[1].xyz;
  tmpvar_3[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * normalize(_glesNormal));
  highp vec3 tmpvar_5;
  tmpvar_5 = (_Sphere0.xyz - tmpvar_2);
  highp float tmpvar_6;
  tmpvar_6 = sqrt(dot (tmpvar_5, tmpvar_5));
  highp float tmpvar_7;
  tmpvar_7 = (_Sphere0.w / tmpvar_6);
  highp vec3 tmpvar_8;
  tmpvar_8 = (_Sphere1.xyz - tmpvar_2);
  highp float tmpvar_9;
  tmpvar_9 = sqrt(dot (tmpvar_8, tmpvar_8));
  highp float tmpvar_10;
  tmpvar_10 = (_Sphere1.w / tmpvar_9);
  highp vec3 tmpvar_11;
  tmpvar_11 = (_Sphere2.xyz - tmpvar_2);
  highp float tmpvar_12;
  tmpvar_12 = sqrt(dot (tmpvar_11, tmpvar_11));
  highp float tmpvar_13;
  tmpvar_13 = (_Sphere2.w / tmpvar_12);
  highp float tmpvar_14;
  tmpvar_14 = (max ((1.0 - clamp (((((dot (tmpvar_4, (tmpvar_5 / tmpvar_6)) * tmpvar_7) * tmpvar_7) + ((dot (tmpvar_4, (tmpvar_8 / tmpvar_9)) * tmpvar_10) * tmpvar_10)) + ((dot (tmpvar_4, (tmpvar_11 / tmpvar_12)) * tmpvar_13) * tmpvar_13)), 0.0, 1.0)), (1.0 - _Intensity)) + (1.0 - _glesColor.x));
  highp vec4 tmpvar_15;
  tmpvar_15.x = tmpvar_14;
  tmpvar_15.y = tmpvar_14;
  tmpvar_15.z = tmpvar_14;
  tmpvar_15.w = tmpvar_14;
  tmpvar_1 = tmpvar_15;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_COLOR = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying lowp vec4 xlv_COLOR;
void main ()
{
  gl_FragData[0] = xlv_COLOR;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _Sphere0;
uniform highp vec4 _Sphere1;
uniform highp vec4 _Sphere2;
uniform highp float _Intensity;
out lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = (_Object2World * _glesVertex).xyz;
  mat3 tmpvar_3;
  tmpvar_3[0] = _Object2World[0].xyz;
  tmpvar_3[1] = _Object2World[1].xyz;
  tmpvar_3[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * normalize(_glesNormal));
  highp vec3 tmpvar_5;
  tmpvar_5 = (_Sphere0.xyz - tmpvar_2);
  highp float tmpvar_6;
  tmpvar_6 = sqrt(dot (tmpvar_5, tmpvar_5));
  highp float tmpvar_7;
  tmpvar_7 = (_Sphere0.w / tmpvar_6);
  highp vec3 tmpvar_8;
  tmpvar_8 = (_Sphere1.xyz - tmpvar_2);
  highp float tmpvar_9;
  tmpvar_9 = sqrt(dot (tmpvar_8, tmpvar_8));
  highp float tmpvar_10;
  tmpvar_10 = (_Sphere1.w / tmpvar_9);
  highp vec3 tmpvar_11;
  tmpvar_11 = (_Sphere2.xyz - tmpvar_2);
  highp float tmpvar_12;
  tmpvar_12 = sqrt(dot (tmpvar_11, tmpvar_11));
  highp float tmpvar_13;
  tmpvar_13 = (_Sphere2.w / tmpvar_12);
  highp float tmpvar_14;
  tmpvar_14 = (max ((1.0 - clamp (((((dot (tmpvar_4, (tmpvar_5 / tmpvar_6)) * tmpvar_7) * tmpvar_7) + ((dot (tmpvar_4, (tmpvar_8 / tmpvar_9)) * tmpvar_10) * tmpvar_10)) + ((dot (tmpvar_4, (tmpvar_11 / tmpvar_12)) * tmpvar_13) * tmpvar_13)), 0.0, 1.0)), (1.0 - _Intensity)) + (1.0 - _glesColor.x));
  highp vec4 tmpvar_15;
  tmpvar_15.x = tmpvar_14;
  tmpvar_15.y = tmpvar_14;
  tmpvar_15.z = tmpvar_14;
  tmpvar_15.w = tmpvar_14;
  tmpvar_1 = tmpvar_15;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_COLOR = tmpvar_1;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
in lowp vec4 xlv_COLOR;
void main ()
{
  _glesFragData[0] = xlv_COLOR;
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