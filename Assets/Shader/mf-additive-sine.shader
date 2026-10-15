// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'

Shader "MADFINGER/FX/Additive Sine" 
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
        _ScrollSpeed ("Scroll speed XY", Vector) = (0,0,0,0)
        _SineParams ("Sine params: amplitude XY, freq ZW", Vector) = (1,1,0.5,0.5)
    }
    SubShader
    {
        Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
        Pass
        {
            Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
            ZWrite Off
            Fog {
            Color (0,0,0,0)
            }
            Blend One One
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float4 _SineParams;
            float4 _ScrollSpeed;
            float4 _MainTex_ST;
            sampler2D _MainTex;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float4 texcoord0 : TEXCOORD0;
            };
            struct v2f
            {
                float4 color : COLOR;
                float2 texcoord0 : TEXCOORD0;
                float4 vertex : POSITION;
            };
            v2f vert(appdata_t v)
            {
                v2f o;
                  float2 tmpvar_1;
                  float2 tmpvar_2;
                  tmpvar_2 = (((v.texcoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + frac((_ScrollSpeed.xy * _Time.y)));
                  tmpvar_1.x = (tmpvar_2.x + (sin((_Time.y * _SineParams.z)) * _SineParams.x));
                  tmpvar_1.y = (tmpvar_2.y + (sin((_Time.y * _SineParams.w)) * _SineParams.y));
                  o.vertex = (UnityObjectToClipPos(v.vertex));
                  o.color = v.color;
                  o.texcoord0 = tmpvar_1;
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                  float4 tmpvar_1;
                  tmpvar_1 = (tex2D (_MainTex, i.texcoord0) * i.color);
                  return tmpvar_1;
            }
            ENDCG
        }
    }
    
}