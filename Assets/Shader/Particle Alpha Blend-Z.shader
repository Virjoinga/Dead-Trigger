Shader "MADFINGER/Particles/Alpha Blended Z-enabled"
{
 Properties
 {
  _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
  _MainTex ("Particle Texture", 2D) = "white" {}
 }
 SubShader
 {
  Tags { "Queue"="Transparent" }
  Pass
  {
   Cull Off
   ZWrite On
   ZTest LEqual
   Blend SrcAlpha OneMinusSrcAlpha
   ColorMask RGB
   CGPROGRAM
   #pragma vertex vert
   #pragma fragment frag
   #pragma multi_compile_fog
   #include "UnityCG.cginc"

   sampler2D _MainTex;
   float4 _MainTex_ST;
   fixed4 _TintColor;
   struct appdata_t
   {
    float4 vertex : POSITION;
    float4 color : COLOR;
    float2 uv : TEXCOORD0;
   };
   struct v2f
   {
    float4 vertex : SV_POSITION;
    fixed4 color : COLOR;
    float2 uv : TEXCOORD0;
    UNITY_FOG_COORDS(1)
   };
   v2f vert(appdata_t v)
   {
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.color = v.color * _TintColor;
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    UNITY_TRANSFER_FOG(o, o.vertex);
    return o;
   }
   fixed4 frag(v2f i) : SV_Target
   {
    fixed4 result = 2.0 * tex2D(_MainTex, i.uv) * i.color;
    result.a = saturate(result.a);
    // The original shader explicitly fogged toward black. The desktop legacy
    // combiner substitutes the scene fog color, tinting the level fade blue.
    UNITY_APPLY_FOG_COLOR(i.fogCoord, result, fixed4(0,0,0,0));
    return result;
   }
   ENDCG
  }
 }
}
