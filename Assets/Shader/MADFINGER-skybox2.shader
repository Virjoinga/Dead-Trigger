// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'

Shader "MADFINGER/Environment/Skybox - opaque - no fog" 
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
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
            sampler2D _MainTex;
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
                  o.vertex = (UnityObjectToClipPos(v.vertex));
                  o.texcoord0 = ((v.texcoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                  float4 tmpvar_1;
                  tmpvar_1 = tex2D (_MainTex, i.texcoord0);
                  return tmpvar_1;
            }
            ENDCG
        }
    }
    
}