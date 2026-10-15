// Upgrade NOTE: commented out 'float4x4 _Object2World', a built-in variable
// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "MADFINGER/Glass/Lightmap + cube env + per vertex alpha" 
{
    Properties
    {
        _MainTex ("Diffuse", 2D) = "white" {}
        _EnvMap ("EnvMap", CUBE) = "black" {}
        _ReflectivityMaskWeights ("Reflectivity Mask Weights (RGB)", Vector) = (0.3,0.59,0.11,0)
    }
    SubShader
    {
        LOD 100
        Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
        Pass
        {
            Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            // float4x4 _Object2World;
            sampler2D _MainTex;
            samplerCUBE _EnvMap;
            float4 _ReflectivityMaskWeights;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float3 normal : NORMAL;
                float4 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
            };
            struct v2f
            {
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float3 texcoord2 : TEXCOORD2;
                float4 color : COLOR;
                float4 vertex : POSITION;
            };
            v2f vert(appdata_t v)
            {
                v2f o;
                  float3x3 tmpvar_1;
                  tmpvar_1[0] = unity_ObjectToWorld[0].xyz;
                  tmpvar_1[1] = unity_ObjectToWorld[1].xyz;
                  tmpvar_1[2] = unity_ObjectToWorld[2].xyz;
                  float3 tmpvar_2;
                  tmpvar_2 = normalize(mul(tmpvar_1, normalize(v.normal)));
                  float3 i_3;
                  i_3 = (mul(unity_ObjectToWorld, v.vertex).xyz - _WorldSpaceCameraPos);
                  o.vertex = (UnityObjectToClipPos(v.vertex));
                  o.texcoord0 = v.texcoord0.xy;
                  o.texcoord1 = ((v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
                  o.texcoord2 = (i_3 - (2.0 * (dot (tmpvar_2, i_3) * tmpvar_2)));
                  o.color = v.color;
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                  float4 c_1;
                  float4 tmpvar_2;
                  tmpvar_2 = tex2D (_MainTex, i.texcoord0);
                  c_1.w = tmpvar_2.w;
                  float4 tmpvar_3;
                  tmpvar_3 = texCUBE (_EnvMap, i.texcoord2);
                  float3 tmpvar_4;
                  tmpvar_4 = (tmpvar_2.xyz + (tmpvar_3 * dot (tmpvar_2.xyz, _ReflectivityMaskWeights.xyz)).xyz);
                  c_1.xyz = tmpvar_4;
                  c_1.xyz = (c_1.xyz * (2.0 * UNITY_SAMPLE_TEX2D (unity_Lightmap, i.texcoord1).xyz));
                  c_1.w = i.color.w;
                  return c_1;
            }
            ENDCG
        }
    }
    
}