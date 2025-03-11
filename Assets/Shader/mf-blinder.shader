ƒShader "MADFINGER/Utils/Blinder" {
SubShader { 
 LOD 100
 Tags { "QUEUE"="Geometry+1000" "RenderType"="Opaque" }
 Pass {
  Tags { "QUEUE"="Geometry+1000" "RenderType"="Opaque" }
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
uniform highp mat4 glstate_matrix_mvp;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
}



#endif
#ifdef FRAGMENT

void main ()
{
  gl_FragData[0] = vec4(0.0, 0.0, 0.0, 0.0);
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
uniform highp mat4 glstate_matrix_mvp;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
void main ()
{
  _glesFragData[0] = vec4(0.0, 0.0, 0.0, 0.0);
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