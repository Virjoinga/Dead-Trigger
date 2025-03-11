×xShader "MADFINGER/Environment/Cubemap specular + Custom Lightmap + fake bump" {
Properties {
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
 _CustomLightmap ("Custom lightmap", 2D) = "white" {}
 _SpecCubeTex ("SpecCube", CUBE) = "black" {}
 _SpecularStrength ("Specular strength weights", Vector) = (0,0,0,2)
 _ScrollingSpeed ("Scrolling speed", Vector) = (0,0,0,0)
 _Params ("Bumpiness - x", Vector) = (2,0,0,0)
}
SubShader { 
 LOD 100
 Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
Program "vp" {
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_LOW" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _ScrollingSpeed;
uniform highp vec4 _Params;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  tmpvar_1.zw = (vec2(2.0, 1.0) * _Params.x);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = _glesMultiTexCoord1.xy;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _CustomLightmap;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz * texture2D (_CustomLightmap, xlv_TEXCOORD1).xyz);
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_LOW" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _ScrollingSpeed;
uniform highp vec4 _Params;
out highp vec4 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  tmpvar_1.zw = (vec2(2.0, 1.0) * _Params.x);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = _glesMultiTexCoord1.xy;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _CustomLightmap;
in highp vec4 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0.xy);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz * texture (_CustomLightmap, xlv_TEXCOORD1).xyz);
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_MEDIUM" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _ScrollingSpeed;
uniform highp vec4 _Params;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  tmpvar_1.zw = (vec2(2.0, 1.0) * _Params.x);
  mat3 tmpvar_2;
  tmpvar_2[0] = _Object2World[0].xyz;
  tmpvar_2[1] = _Object2World[1].xyz;
  tmpvar_2[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = normalize((tmpvar_2 * normalize(_glesNormal)));
  highp vec3 i_4;
  i_4 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = _glesMultiTexCoord1.xy;
  xlv_TEXCOORD2 = (i_4 - (2.0 * (dot (tmpvar_3, i_4) * tmpvar_3)));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _CustomLightmap;
uniform lowp samplerCube _SpecCubeTex;
uniform highp vec4 _SpecularStrength;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec3 spec_1;
  lowp vec4 c_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  c_2.w = tmpvar_3.w;
  lowp vec4 tmpvar_4;
  highp vec3 P_5;
  P_5 = (xlv_TEXCOORD2 + ((tmpvar_3.xyz * xlv_TEXCOORD0.z) - xlv_TEXCOORD0.w));
  tmpvar_4 = textureCube (_SpecCubeTex, P_5);
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_4 * dot (_SpecularStrength, tmpvar_3)).xyz;
  spec_1 = tmpvar_6;
  c_2.xyz = (tmpvar_3.xyz + spec_1);
  c_2.xyz = (c_2.xyz * texture2D (_CustomLightmap, xlv_TEXCOORD1).xyz);
  gl_FragData[0] = c_2;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_MEDIUM" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _ScrollingSpeed;
uniform highp vec4 _Params;
out highp vec4 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  tmpvar_1.zw = (vec2(2.0, 1.0) * _Params.x);
  mat3 tmpvar_2;
  tmpvar_2[0] = _Object2World[0].xyz;
  tmpvar_2[1] = _Object2World[1].xyz;
  tmpvar_2[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = normalize((tmpvar_2 * normalize(_glesNormal)));
  highp vec3 i_4;
  i_4 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = _glesMultiTexCoord1.xy;
  xlv_TEXCOORD2 = (i_4 - (2.0 * (dot (tmpvar_3, i_4) * tmpvar_3)));
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _CustomLightmap;
uniform lowp samplerCube _SpecCubeTex;
uniform highp vec4 _SpecularStrength;
in highp vec4 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec3 spec_1;
  lowp vec4 c_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD0.xy);
  c_2.w = tmpvar_3.w;
  lowp vec4 tmpvar_4;
  highp vec3 P_5;
  P_5 = (xlv_TEXCOORD2 + ((tmpvar_3.xyz * xlv_TEXCOORD0.z) - xlv_TEXCOORD0.w));
  tmpvar_4 = texture (_SpecCubeTex, P_5);
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_4 * dot (_SpecularStrength, tmpvar_3)).xyz;
  spec_1 = tmpvar_6;
  c_2.xyz = (tmpvar_3.xyz + spec_1);
  c_2.xyz = (c_2.xyz * texture (_CustomLightmap, xlv_TEXCOORD1).xyz);
  _glesFragData[0] = c_2;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_HIGH" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _ScrollingSpeed;
uniform highp vec4 _Params;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  tmpvar_1.zw = (vec2(2.0, 1.0) * _Params.x);
  mat3 tmpvar_2;
  tmpvar_2[0] = _Object2World[0].xyz;
  tmpvar_2[1] = _Object2World[1].xyz;
  tmpvar_2[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = normalize((tmpvar_2 * normalize(_glesNormal)));
  highp vec3 i_4;
  i_4 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = _glesMultiTexCoord1.xy;
  xlv_TEXCOORD2 = (i_4 - (2.0 * (dot (tmpvar_3, i_4) * tmpvar_3)));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _CustomLightmap;
uniform lowp samplerCube _SpecCubeTex;
uniform highp vec4 _SpecularStrength;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec3 spec_1;
  lowp vec4 c_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  c_2.w = tmpvar_3.w;
  lowp vec4 tmpvar_4;
  highp vec3 P_5;
  P_5 = (xlv_TEXCOORD2 + ((tmpvar_3.xyz * xlv_TEXCOORD0.z) - xlv_TEXCOORD0.w));
  tmpvar_4 = textureCube (_SpecCubeTex, P_5);
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_4 * dot (_SpecularStrength, tmpvar_3)).xyz;
  spec_1 = tmpvar_6;
  c_2.xyz = (tmpvar_3.xyz + spec_1);
  c_2.xyz = (c_2.xyz * texture2D (_CustomLightmap, xlv_TEXCOORD1).xyz);
  gl_FragData[0] = c_2;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_HIGH" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _ScrollingSpeed;
uniform highp vec4 _Params;
out highp vec4 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  tmpvar_1.zw = (vec2(2.0, 1.0) * _Params.x);
  mat3 tmpvar_2;
  tmpvar_2[0] = _Object2World[0].xyz;
  tmpvar_2[1] = _Object2World[1].xyz;
  tmpvar_2[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = normalize((tmpvar_2 * normalize(_glesNormal)));
  highp vec3 i_4;
  i_4 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = _glesMultiTexCoord1.xy;
  xlv_TEXCOORD2 = (i_4 - (2.0 * (dot (tmpvar_3, i_4) * tmpvar_3)));
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _CustomLightmap;
uniform lowp samplerCube _SpecCubeTex;
uniform highp vec4 _SpecularStrength;
in highp vec4 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec3 spec_1;
  lowp vec4 c_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD0.xy);
  c_2.w = tmpvar_3.w;
  lowp vec4 tmpvar_4;
  highp vec3 P_5;
  P_5 = (xlv_TEXCOORD2 + ((tmpvar_3.xyz * xlv_TEXCOORD0.z) - xlv_TEXCOORD0.w));
  tmpvar_4 = texture (_SpecCubeTex, P_5);
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_4 * dot (_SpecularStrength, tmpvar_3)).xyz;
  spec_1 = tmpvar_6;
  c_2.xyz = (tmpvar_3.xyz + spec_1);
  c_2.xyz = (c_2.xyz * texture (_CustomLightmap, xlv_TEXCOORD1).xyz);
  _glesFragData[0] = c_2;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_VERY_HIGH" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _ScrollingSpeed;
uniform highp vec4 _Params;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  tmpvar_1.zw = (vec2(2.0, 1.0) * _Params.x);
  mat3 tmpvar_2;
  tmpvar_2[0] = _Object2World[0].xyz;
  tmpvar_2[1] = _Object2World[1].xyz;
  tmpvar_2[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = normalize((tmpvar_2 * normalize(_glesNormal)));
  highp vec3 i_4;
  i_4 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = _glesMultiTexCoord1.xy;
  xlv_TEXCOORD2 = (i_4 - (2.0 * (dot (tmpvar_3, i_4) * tmpvar_3)));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _CustomLightmap;
uniform lowp samplerCube _SpecCubeTex;
uniform highp vec4 _SpecularStrength;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec3 spec_1;
  lowp vec4 c_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  c_2.w = tmpvar_3.w;
  lowp vec4 tmpvar_4;
  highp vec3 P_5;
  P_5 = (xlv_TEXCOORD2 + ((tmpvar_3.xyz * xlv_TEXCOORD0.z) - xlv_TEXCOORD0.w));
  tmpvar_4 = textureCube (_SpecCubeTex, P_5);
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_4 * dot (_SpecularStrength, tmpvar_3)).xyz;
  spec_1 = tmpvar_6;
  c_2.xyz = (tmpvar_3.xyz + spec_1);
  c_2.xyz = (c_2.xyz * texture2D (_CustomLightmap, xlv_TEXCOORD1).xyz);
  gl_FragData[0] = c_2;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_VERY_HIGH" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _ScrollingSpeed;
uniform highp vec4 _Params;
out highp vec4 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  tmpvar_1.zw = (vec2(2.0, 1.0) * _Params.x);
  mat3 tmpvar_2;
  tmpvar_2[0] = _Object2World[0].xyz;
  tmpvar_2[1] = _Object2World[1].xyz;
  tmpvar_2[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = normalize((tmpvar_2 * normalize(_glesNormal)));
  highp vec3 i_4;
  i_4 = ((_Object2World * _glesVertex).xyz - _WorldSpaceCameraPos);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = _glesMultiTexCoord1.xy;
  xlv_TEXCOORD2 = (i_4 - (2.0 * (dot (tmpvar_3, i_4) * tmpvar_3)));
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _CustomLightmap;
uniform lowp samplerCube _SpecCubeTex;
uniform highp vec4 _SpecularStrength;
in highp vec4 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec3 spec_1;
  lowp vec4 c_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD0.xy);
  c_2.w = tmpvar_3.w;
  lowp vec4 tmpvar_4;
  highp vec3 P_5;
  P_5 = (xlv_TEXCOORD2 + ((tmpvar_3.xyz * xlv_TEXCOORD0.z) - xlv_TEXCOORD0.w));
  tmpvar_4 = texture (_SpecCubeTex, P_5);
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_4 * dot (_SpecularStrength, tmpvar_3)).xyz;
  spec_1 = tmpvar_6;
  c_2.xyz = (tmpvar_3.xyz + spec_1);
  c_2.xyz = (c_2.xyz * texture (_CustomLightmap, xlv_TEXCOORD1).xyz);
  _glesFragData[0] = c_2;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_LOW" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_LOW" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_MEDIUM" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_MEDIUM" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_HIGH" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_HIGH" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNITY_SHADER_DETAIL_VERY_HIGH" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNITY_SHADER_DETAIL_VERY_HIGH" }
"!!GLES3"
}
}
 }
}
}