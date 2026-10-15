Shader "MADFINGER/Environment/Cubemap specular + Lightmap" 
{
    Properties
    {
        _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
        _SpecCubeTex ("SpecCube", CUBE) = "black" {}
        _SpecularStrength ("Specular strength weights", Vector) = (0,0,0,2)
        _ScrollingSpeed ("Scrolling speed", Vector) = (0,0,0,0)
    }
    SubShader
    {
        LOD 100
        Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
        Pass
        {
            Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float4 _ScrollingSpeed;
            float4 _MainTex_ST;
            sampler2D _MainTex;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float4 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
            };
            struct v2f
            {
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float4 color : COLOR;
                float4 vertex : POSITION;
            };
            v2f vert(appdata_t v)
            {
                v2f o;
                  o.vertex = (UnityObjectToClipPos(v.vertex));
                  o.texcoord0 = (((v.texcoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + frac((_ScrollingSpeed * _Time.y)).xy);
                  o.texcoord1 = ((v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
                  o.color = v.color;
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                  float4 c_1;
                  float4 tmpvar_2;
                  tmpvar_2 = tex2D (_MainTex, i.texcoord0);
                  c_1.w = tmpvar_2.w;
                  c_1.xyz = (tmpvar_2.xyz * (2.0 * UNITY_SAMPLE_TEX2D (unity_Lightmap, i.texcoord1).xyz));
                  c_1.xyz = (c_1.xyz * i.color.xyz);
                  return c_1;
            }
            ENDCG
        }
    }
    
}