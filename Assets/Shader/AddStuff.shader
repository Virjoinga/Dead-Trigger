Shader "Hidden/AddStuff" 
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
            Blend One One
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex;
            float intensity;
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
                  float4 tmpvar_1;
                  float4 tmpvar_2;
                  tmpvar_2 = tex2D (_MainTex, i.texcoord0);
                  tmpvar_1 = (tmpvar_2 * intensity);
                  return tmpvar_1;
            }
            ENDCG
        }
    }
    Fallback Off
}