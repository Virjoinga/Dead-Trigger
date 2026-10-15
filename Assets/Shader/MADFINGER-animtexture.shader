// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'

Shader "MADFINGER/FX/Anim texture" 
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _NumTexTiles ("Num tex tiles", Vector) = (4,4,0,0)
        _ReplaySpeed ("Replay speed - FPS", Float) = 4
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
        Pass
        {
            Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
            ZWrite Off
            Cull Off
            Fog {
            Color (0,0,0,0)
            }
            Blend One One
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float4 _Color;
            float4 _NumTexTiles;
            float _ReplaySpeed;
            sampler2D _MainTex;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float4 texcoord0 : TEXCOORD0;
            };
            struct v2f
            {
                float4 texcoord0 : TEXCOORD0;
                float4 color : COLOR;
                float4 vertex : POSITION;
            };
            v2f vert(appdata_t v)
            {
                v2f o;
                  float4 tile_1;
                  float4 tmpvar_2;
                  float tmpvar_3;
                  tmpvar_3 = (((v.color.w * 60.0) + _Time.y) * _ReplaySpeed);
                  float tmpvar_4;
                  tmpvar_4 = floor(tmpvar_3);
                  float tmpvar_5;
                  tmpvar_5 = (tmpvar_4 + 1.0);
                  float tmpvar_6;
                  tmpvar_6 = (tmpvar_3 - tmpvar_4);
                  float2 tmpvar_7;
                  tmpvar_7 = (1.0/(_NumTexTiles.xy));
                  float2 tmpvar_8;
                  tmpvar_8.x = tmpvar_4;
                  tmpvar_8.y = floor((tmpvar_4 / _NumTexTiles.x));
                  tile_1.xy = tmpvar_8;
                  float2 tmpvar_9;
                  tmpvar_9.x = tmpvar_5;
                  tmpvar_9.y = floor((tmpvar_5 / _NumTexTiles.x));
                  tile_1.zw = tmpvar_9;
                  float4 tmpvar_10;
                  tmpvar_10 = (tile_1 / _NumTexTiles.xyxy);
                  float4 tmpvar_11;
                  tmpvar_11 = (frac(abs(tmpvar_10)) * _NumTexTiles.xyxy);
                  float tmpvar_12;
                  if ((tmpvar_10.x >= 0.0)) {
                  tmpvar_12 = tmpvar_11.x;
                  } else {
                  tmpvar_12 = -(tmpvar_11.x);
                  };
                  float tmpvar_13;
                  if ((tmpvar_10.y >= 0.0)) {
                  tmpvar_13 = tmpvar_11.y;
                  } else {
                  tmpvar_13 = -(tmpvar_11.y);
                  };
                  float tmpvar_14;
                  if ((tmpvar_10.z >= 0.0)) {
                  tmpvar_14 = tmpvar_11.z;
                  } else {
                  tmpvar_14 = -(tmpvar_11.z);
                  };
                  float tmpvar_15;
                  if ((tmpvar_10.w >= 0.0)) {
                  tmpvar_15 = tmpvar_11.w;
                  } else {
                  tmpvar_15 = -(tmpvar_11.w);
                  };
                  float4 tmpvar_16;
                  tmpvar_16.x = tmpvar_12;
                  tmpvar_16.y = tmpvar_13;
                  tmpvar_16.z = tmpvar_14;
                  tmpvar_16.w = tmpvar_15;
                  tile_1 = tmpvar_16;
                  float4 tmpvar_17;
                  tmpvar_17.xyz = (_Color.xyz * v.color.xyz);
                  tmpvar_17.w = tmpvar_6;
                  tmpvar_2 = tmpvar_17;
                  o.vertex = (UnityObjectToClipPos(v.vertex));
                  o.texcoord0 = ((v.texcoord0.xyxy + tmpvar_16) * tmpvar_7.xyxy);
                  o.color = tmpvar_2;
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                  float4 tmpvar_1;
                  tmpvar_1 = (lerp (tex2D (_MainTex, i.texcoord0.xy), tex2D (_MainTex, i.texcoord0.zw), i.color.wwww) * i.color);
                  return tmpvar_1;
            }
            ENDCG
        }
    }
    
}