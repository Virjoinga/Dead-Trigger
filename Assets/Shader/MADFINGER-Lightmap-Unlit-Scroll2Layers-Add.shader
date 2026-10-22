// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'

Shader "MADFINGER/Environment/Scroll 2 Layers Additive (Supports Lightmap)"
{
    Properties
    {
        _MainTex ("Base layer (RGB)", 2D) = "white" {}
        _DetailTex ("2nd layer (RGB)", 2D) = "white" {}
        _ScrollX ("Base layer Scroll speed X", Float) = 1
        _ScrollY ("Base layer Scroll speed Y", Float) = 0
        _Scroll2X ("2nd layer Scroll speed X", Float) = 1
        _Scroll2Y ("2nd layer Scroll speed Y", Float) = 0
        _AMultiplier ("Layer Multiplier", Float) = 0.5
    }
    SubShader
    {
        LOD 100
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Tags { "RenderType"="Opaque" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile __ LIGHTMAP_ON
            #include "UnityCG.cginc"
            float _ScrollX;
            float _ScrollY;
            float _Scroll2X;
            float _Scroll2Y;
            float _AMultiplier;
            sampler2D _MainTex;
            sampler2D _DetailTex;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
            };
            struct v2f
            {
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float4 texcoord2 : TEXCOORD2;
                float2 lightmapUV : TEXCOORD3;
                float4 vertex : POSITION;
            };
            v2f vert(appdata_t v)
            {
                v2f o;
                  float4 tmpvar_1;
                  float2 tmpvar_2;
                  tmpvar_2.x = _ScrollX;
                  tmpvar_2.y = _ScrollY;
                  float2 tmpvar_3;
                  tmpvar_3.x = _Scroll2X;
                  tmpvar_3.y = _Scroll2Y;
                  float4 tmpvar_4;
                  tmpvar_4.x = _AMultiplier;
                  tmpvar_4.y = _AMultiplier;
                  tmpvar_4.z = _AMultiplier;
                  tmpvar_4.w = _AMultiplier;
                  tmpvar_1 = tmpvar_4;
                  o.vertex = (UnityObjectToClipPos(v.vertex));
                  o.texcoord0 = (v.texcoord0.xy + frac((tmpvar_2 * _Time.xy)));
                  o.texcoord1 = (v.texcoord0.xy + frac((tmpvar_3 * _Time.xy)));
                  o.texcoord2 = tmpvar_1;
                  o.lightmapUV = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                  float4 tmpvar_1;
                  tmpvar_1 = ((tex2D (_MainTex, i.texcoord0) + tex2D (_DetailTex, i.texcoord1)) * i.texcoord2);
                  #if defined(LIGHTMAP_ON)
                  tmpvar_1.rgb *= 2.0 * UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lightmapUV).rgb;
                  #endif
                  return tmpvar_1;
            }
            ENDCG
        }
    }

}
