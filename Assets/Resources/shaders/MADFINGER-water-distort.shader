Shader "MADFINGER/PostFX/WaterScreenRefraction"
{
 Properties
 {
  _MainTex ("Base (RGB)", 2D) = "white" {}
  _EnvMap ("2D EnvMap", 2D) = "black" {}
  _DropletMap ("Droplet slopes (RG), thickness (B)", 2D) = "black" {}
  _ScrollingSpeed ("xy - Layer0, zw - Layer1", Vector) = (0,0.05,0,0.01)
  _Color ("Color", Color) = (0,0,0,0)
  _Params ("x = refraction strength, y = Layer 0 tiling, z = Layer 1 tiling", Vector) = (0.01,1.5,2,0)
 }
 SubShader
 {
  Pass
  {
   ZTest Always
   ZWrite Off
   Cull Off
   CGPROGRAM
   #pragma vertex vert_img
   #pragma fragment frag
   #include "UnityCG.cginc"
   sampler2D _MainTex, _EnvMap, _DropletMap;
   float4 _MainTex_TexelSize, _Params, _Color;

   float4 frag(v2f_img i) : SV_Target
   {
    float2 dropletUV = i.uv;
    #if UNITY_UV_STARTS_AT_TOP
    if (_MainTex_TexelSize.y < 0) dropletUV.y = 1.0 - dropletUV.y;
    #endif
    float4 droplet = tex2D(_DropletMap, dropletUV);
    float thickness = droplet.b;
    // Neutral RG is quantized; the height mask guarantees zero offset off-drop.
    float mask = saturate(thickness * 8.0);
    float2 slope = droplet.rg * 2.0 - 1.0;
    float2 normal = -normalize(float3(slope, 0.25)).xy;
    float2 offset = normal * _Params.x * mask;
    #if UNITY_UV_STARTS_AT_TOP
    if (_MainTex_TexelSize.y < 0) offset.y = -offset.y;
    #endif
    float4 scene = tex2D(_MainTex, saturate(i.uv + offset));
    float3 environment = tex2D(_EnvMap, normal * 0.5 + 0.5).rgb;
    scene.rgb += environment * thickness + _Color.rgb * thickness * 2.0;
    return scene;
   }
   ENDCG
  }
 }
 Fallback Off
}
