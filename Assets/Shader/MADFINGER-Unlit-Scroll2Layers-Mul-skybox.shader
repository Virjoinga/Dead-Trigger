// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'

Shader "MADFINGER/Environment/Scroll 2 Layers Multiplicative - Skybox" 
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
        _ColorTint ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        LOD 100
        Tags { "QUEUE"="Geometry+10" "RenderType"="Opaque" }
        Pass
        {
            Tags { "QUEUE"="Geometry+10" "RenderType"="Opaque" }
            ZWrite Off
            Fog { Mode Off }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float4 _MainTex_ST;
            float4 _DetailTex_ST;
            float _ScrollX;
            float _ScrollY;
            float _Scroll2X;
            float _Scroll2Y;
            float _AMultiplier;
            float4 _ColorTint;
            sampler2D _MainTex;
            sampler2D _DetailTex;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
            };
            struct v2f
            {
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float4 texcoord2 : TEXCOORD2;
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
                  tmpvar_4 = (_ColorTint * _AMultiplier);
                  tmpvar_1 = tmpvar_4;
                  o.vertex = (UnityObjectToClipPos(v.vertex));
                  o.texcoord0 = (((v.texcoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + frac((tmpvar_2 * _Time.xy)));
                  o.texcoord1 = (((v.texcoord0.xy * _DetailTex_ST.xy) + _DetailTex_ST.zw) + frac((tmpvar_3 * _Time.xy)));
                  o.texcoord2 = tmpvar_1;
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                  float4 tmpvar_1;
                  tmpvar_1 = ((tex2D (_MainTex, i.texcoord0) * tex2D (_DetailTex, i.texcoord1)) * i.texcoord2);
                  return tmpvar_1;
            }
            ENDCG
        }
    }
    
}