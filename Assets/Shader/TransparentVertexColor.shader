Shader "MADFINGER/Transparent/Vertex Color" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _SpecColor ("Spec Color", Color) = (1,1,1,0)
 _Emission ("Emmisive Color", Color) = (0,0,0,0)
 _Shininess ("Shininess", Range(0.1,1)) = 0.7
 _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}
SubShader
{
 Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
 Pass
 {
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #pragma multi_compile_fog
  #include "UnityCG.cginc"

  sampler2D _MainTex;
  float4 _MainTex_ST;
  fixed4 _Color;
  struct appdata_t
  {
   float4 vertex : POSITION;
   float4 color : COLOR;
   float2 uv : TEXCOORD0;
  };
  struct v2f
  {
   float4 vertex : SV_POSITION;
   float4 color : COLOR;
   float2 uv : TEXCOORD0;
   UNITY_FOG_COORDS(1)
  };
  v2f vert(appdata_t v)
  {
   v2f o;
   o.vertex = UnityObjectToClipPos(v.vertex);
   o.color = v.color;
   o.uv = TRANSFORM_TEX(v.uv, _MainTex);
   UNITY_TRANSFER_FOG(o, o.vertex);
   return o;
  }
  fixed4 frag(v2f i) : SV_Target
  {
   fixed4 textureColor = tex2D(_MainTex, i.uv);
   fixed4 result;
   // Retain the original doubled RGB tint and baked vertex lighting.
   result.rgb = textureColor.rgb * i.color.rgb * _Color.rgb * 2.0;
   // The legacy combiner's Double modifier also scaled alpha on desktop.
   // Keep opacity independent of RGB intensity and bounded for blending.
   result.a = saturate(textureColor.a * i.color.a * _Color.a);
   UNITY_APPLY_FOG(i.fogCoord, result);
   return result;
  }
  ENDCG
 }
}
}
