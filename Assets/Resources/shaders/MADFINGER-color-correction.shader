Shader "MADFINGER/PostFX/ColorCorrection" 
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "" {}
    }
    SubShader
    {
        Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off
            Fog { Mode Off }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex;
            float4x4 _ColorMatrix;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
            };
            struct v2f
            {
                float2 texcoord0 : TEXCOORD0;
                float4 vertex : POSITION;
            };
            v2f vert(appdata_t v)
            {
                v2f o;
                  float2 tmpvar_1;
                  float2 tmpvar_2;
                  tmpvar_2 = v.texcoord0.xy;
                  tmpvar_1 = tmpvar_2;
                  o.vertex = (UnityObjectToClipPos(v.vertex));
                  o.texcoord0 = tmpvar_1;
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                  float3 dst_1;
                  float4 tmpvar_2;
                  tmpvar_2 = tex2D (_MainTex, i.texcoord0);
                  float4 tmpvar_3;
                  tmpvar_3.w = 1.0;
                  tmpvar_3.xyz = tmpvar_2.xyz;
                  float3 tmpvar_4;
                  tmpvar_4 = mul(tmpvar_3, _ColorMatrix).xyz;
                  dst_1 = tmpvar_4;
                  float4 tmpvar_5;
                  tmpvar_5.xyz = dst_1;
                  tmpvar_5.w = tmpvar_2.w;
                  return tmpvar_5;
            }
            ENDCG
        }
    }
    Fallback Off
}