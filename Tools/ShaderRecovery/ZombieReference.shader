// Independent expression of the original GLES shader's column-indexed TBN.
// Fixture SH values are supplied explicitly so scene ambient settings cannot interfere.
Shader "Hidden/ZombieReference"
{
    Properties
    {
        _MainTex ("Base", 2D) = "white" {}
        _BumpMap ("Normal", 2D) = "bump" {}
        _BRDFTex ("BRDF", 2D) = "white" {}
        _SpecularStrength ("Gloss", Vector) = (0,0,0,1)
        _LightProbesLightingAmount ("Probes", Float) = 0.9
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float4 _FixtureSHAr, _FixtureSHAg, _FixtureSHAb;
            float4 _FixtureSHBr, _FixtureSHBg, _FixtureSHBb, _FixtureSHC;
            float _LightProbesLightingAmount;
            float4 _SpecularStrength;
            sampler2D _MainTex, _BumpMap, _BRDFTex;
            struct Input { float4 vertex:POSITION; float3 normal:NORMAL; float4 tangent:TANGENT; float2 uv:TEXCOORD0; };
            struct Output { float4 vertex:SV_POSITION; float2 uv:TEXCOORD0; float3 light:TEXCOORD1; float3 halfVector:TEXCOORD2; float4 color:COLOR; };
            Output vert(Input v)
            {
                Output o;
                float3 n = normalize(v.normal);
                float3 t = normalize(v.tangent.xyz);
                float3 b = cross(n,t) * v.tangent.w;
                float3 lightWorld = _FixtureSHAr.xyz * 0.3 + _FixtureSHAg.xyz * 0.59 + _FixtureSHAb.xyz * 0.11;
                float3 lightObject = mul((float3x3)unity_WorldToObject, lightWorld);
                o.light = normalize(float3(dot(t,lightObject),dot(b,lightObject),dot(n,lightObject)));
                float3 viewObject = mul(unity_WorldToObject,float4(_WorldSpaceCameraPos,1)).xyz - v.vertex.xyz;
                float3 viewTangent = normalize(float3(dot(t,viewObject),dot(b,viewObject),dot(n,viewObject)));
                o.halfVector = normalize(viewTangent + o.light);
                float4 normalWorld = float4(mul((float3x3)unity_ObjectToWorld,n),1);
                float4 products = normalWorld.xyzz * normalWorld.yzzx;
                float3 lighting = float3(dot(_FixtureSHAr,normalWorld),dot(_FixtureSHAg,normalWorld),dot(_FixtureSHAb,normalWorld));
                lighting += float3(dot(_FixtureSHBr,products),dot(_FixtureSHBg,products),dot(_FixtureSHBb,products));
                lighting += _FixtureSHC.xyz * (normalWorld.x*normalWorld.x - normalWorld.y*normalWorld.y);
                o.color = saturate(lighting + (1-_LightProbesLightingAmount)).xyzz;
                o.uv = v.uv;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
            float4 frag(Output i):SV_Target
            {
                float4 baseColor = tex2D(_MainTex,i.uv);
                float3 normal = UnpackNormal(tex2D(_BumpMap,i.uv));
                float2 uv = float2(dot(normal,i.light)*0.5+0.5,dot(normal,i.halfVector));
                float4 brdf = tex2D(_BRDFTex,uv);
                return float4(baseColor.rgb*i.color.rgb*(brdf.rgb+dot(_SpecularStrength,baseColor)*brdf.a)*2,baseColor.a);
            }
            ENDCG
        }
    }
}
