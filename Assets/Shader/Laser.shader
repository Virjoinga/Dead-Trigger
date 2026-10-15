// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'

Shader "__CAPA__/Laser" 
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _NoiseTex ("NoiseTex", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "QUEUE"="Transparent" "RenderType"="Transparent" "Reflection"="LaserScope" }
        Pass
        {
            Tags { "QUEUE"="Transparent" "RenderType"="Transparent" "Reflection"="LaserScope" }
            ZWrite Off
            Cull Off
            Blend SrcAlpha One
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float4 _MainTex_ST;
            float4 _NoiseTex_ST;
            sampler2D _MainTex;
            sampler2D _NoiseTex;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
            };
            struct v2f
            {
                float4 texcoord0 : TEXCOORD0;
                float4 vertex : POSITION;
            };
            v2f vert(appdata_t v)
            {
                v2f o;
                  float4 tmpvar_1;
                  float2 tmpvar_2;
                  tmpvar_2 = ((v.texcoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
                  tmpvar_1.xy = tmpvar_2;
                  float2 tmpvar_3;
                  tmpvar_3 = ((v.texcoord0.xy * _NoiseTex_ST.xy) + _NoiseTex_ST.zw);
                  tmpvar_1.zw = tmpvar_3;
                  o.vertex = (UnityObjectToClipPos(v.vertex));
                  o.texcoord0 = tmpvar_1;
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                  float4 tmpvar_1;
                  tmpvar_1 = (tex2D (_MainTex, i.texcoord0.xy) * tex2D (_NoiseTex, i.texcoord0.zw));
                  return tmpvar_1;
            }
            ENDCG
        }
    }
    Fallback Off
}