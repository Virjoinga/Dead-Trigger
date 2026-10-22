// Upgrade NOTE: commented out 'float4x4 _Object2World', a built-in variable
// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'

Shader "MADFINGER/FX/Water"
{
    Properties
    {
        _EnvTex ("Cube env tex", CUBE) = "black" {}
        _Normals ("Normals", 2D) = "bump" {}
        _Params ("Fresnel bias - x, Fresnel pow - y, Transparency bias - z", Vector) = (0,10,0.2,0)
        _Params2 ("Bumps tiling - x, Bumps scroll speed", Vector) = (0.25,0.1,0,0)
        _DeepColor ("Deep color", Color) = (0,1,0,1)
        _ShallowColor ("Shallow color", Color) = (1,0,0,1)
    }
    SubShader
    {
        LOD 100
        Tags { "QUEUE"="Transparent-10" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
        Pass
        {
            Tags { "QUEUE"="Transparent-10" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
            ZWrite Off
            Fog { Mode Off }
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile __ UNITY_SHADER_DETAIL_LOW UNITY_SHADER_DETAIL_MEDIUM UNITY_SHADER_DETAIL_HIGH UNITY_SHADER_DETAIL_VERY_HIGH UNITY_SHADER_DETAIL_ULTRA
            #include "UnityCG.cginc"
            samplerCUBE _EnvTex;
            sampler2D _Normals;
            float4 _Params, _Params2, _DeepColor, _ShallowColor;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 reflection : TEXCOORD0;
                float4 normalUV : TEXCOORD1;
                float3 normalWorld : TEXCOORD2;
                float3 viewWorld : TEXCOORD3;
                float4 color : COLOR0;
                float opacity : TEXCOORD4;
            };
            v2f vert(appdata_t v)
            {
                v2f o;
                float3 worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
                  float3 normalWorld = normalize(mul((float3x3)unity_ObjectToWorld, normalize(v.normal)));
                float3 viewWorld = _WorldSpaceCameraPos - worldPosition;
                float3 viewDirection = normalize(viewWorld);
                float facing = 1.0 - saturate(dot(viewDirection, normalWorld));
                float fresnel = saturate(_Params.x + (1.0 - _Params.x) * pow(facing, _Params.y));
                o.reflection = reflect(-viewDirection, normalWorld);
                o.reflection.x = -o.reflection.x;
                o.color = float4(lerp(_DeepColor.rgb, _ShallowColor.rgb, facing), fresnel);
                o.opacity = fresnel + _Params.z;
                o.normalUV.xy = worldPosition.xz * _Params2.x + frac(_Time.yy * _Params2.y * float2(-0.393919,0.919145));
                o.normalUV.zw = worldPosition.xz * _Params2.x + frac(float2(0.464238,0.185695) * _Time.yy * _Params2.y);
                o.normalWorld = normalWorld;
                o.viewWorld = viewWorld;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
            float4 frag(v2f i) : SV_Target
            {
                #if defined(UNITY_SHADER_DETAIL_LOW)
                return float4(lerp(i.color.rgb, texCUBE(_EnvTex,i.reflection).rgb, i.color.a), i.opacity);
                #else
                // Restore the original medium-and-higher scrolling normal path.
                float3 n1 = UnpackNormal(tex2D(_Normals,i.normalUV.xy));
                float3 n2 = UnpackNormal(tex2D(_Normals,i.normalUV.zw));
                float3 combined = normalize(float3(n1.xy+n2.xy,n1.z*n2.z));
                float3 normalWorld = normalize(float3(combined.xz+i.normalWorld.xy,combined.y*i.normalWorld.z));
                float3 viewDirection = normalize(i.viewWorld);
                float3 reflection = reflect(-viewDirection,normalWorld);
                float facing = 1.0-saturate(dot(viewDirection,normalWorld));
                float fresnel = saturate(_Params.x+(1.0-_Params.x)*pow(facing,_Params.y));
                float3 color = lerp(_DeepColor.rgb,_ShallowColor.rgb,facing);
                return float4(lerp(color,texCUBE(_EnvTex,reflection).rgb,fresnel),fresnel+_Params.z);
                #endif
            }
            ENDCG
        }
    }

}
