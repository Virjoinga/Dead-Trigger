µShader "MADFINGER/FX/Blood FX - alpha blended" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _FadeInSpeed ("Fade in speed", Float) = 5
 _DrippingSpeed ("Dripping speed", Float) = 0.1
 _UsrTime ("Time", Float) = 0
}
SubShader { 
 Tags { "QUEUE"="Overlay" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Overlay" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Fog {
   Color (0,0,0,0)
  }
  Blend SrcAlpha OneMinusSrcAlpha
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp float _DrippingSpeed;
uniform highp float _UsrTime;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  highp vec4 tmpvar_1;
  lowp vec4 tmpvar_2;
  highp float tmpvar_3;
  tmpvar_3 = (_UsrTime + _glesVertex.z);
  highp vec4 tmpvar_4;
  tmpvar_4.zw = vec2(0.0, 1.0);
  tmpvar_4.xy = _glesVertex.xy;
  tmpvar_1.xzw = tmpvar_4.xzw;
  highp vec4 tmpvar_5;
  tmpvar_5.xyz = vec3(1.0, 1.0, 1.0);
  tmpvar_5.w = (1.0 - max ((tmpvar_3 - (0.25 * _glesMultiTexCoord1.x)), 0.0));
  tmpvar_2 = tmpvar_5;
  tmpvar_1.y = (_glesVertex.y - ((tmpvar_3 * _DrippingSpeed) * _glesMultiTexCoord1.y));
  gl_Position = tmpvar_1;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_COLOR = tmpvar_2;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = (texture2D (_MainTex, xlv_TEXCOORD0) * xlv_COLOR);
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp float _DrippingSpeed;
uniform highp float _UsrTime;
out highp vec2 xlv_TEXCOORD0;
out lowp vec4 xlv_COLOR;
void main ()
{
  highp vec4 tmpvar_1;
  lowp vec4 tmpvar_2;
  highp float tmpvar_3;
  tmpvar_3 = (_UsrTime + _glesVertex.z);
  highp vec4 tmpvar_4;
  tmpvar_4.zw = vec2(0.0, 1.0);
  tmpvar_4.xy = _glesVertex.xy;
  tmpvar_1.xzw = tmpvar_4.xzw;
  highp vec4 tmpvar_5;
  tmpvar_5.xyz = vec3(1.0, 1.0, 1.0);
  tmpvar_5.w = (1.0 - max ((tmpvar_3 - (0.25 * _glesMultiTexCoord1.x)), 0.0));
  tmpvar_2 = tmpvar_5;
  tmpvar_1.y = (_glesVertex.y - ((tmpvar_3 * _DrippingSpeed) * _glesMultiTexCoord1.y));
  gl_Position = tmpvar_1;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_COLOR = tmpvar_2;
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
  tmpvar_1 = (texture (_MainTex, xlv_TEXCOORD0) * xlv_COLOR);
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