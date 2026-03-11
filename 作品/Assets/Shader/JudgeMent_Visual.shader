Shader "Unlit/JudgeMent_Visual"
{
    Properties
    {
       _Color("Color",Color) = (1,1,1,1)
       _OutlineColor("Outline Color", Color) = (0,1,1,1)
       _OutlineWidth("Outline Width",Range(0.0,0.08)) = 0.08
       _Glow("Glow",Range(0,7)) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent+10" "RenderType" = "Transparent"}

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color;
            float _Glow;
            fixed4 _OutlineColor;
            float _OutlineWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
              
                //中央からの距離
                float dist = abs(i.uv.y - 0.5);
                
                //本体マスク
                float core = smoothstep(
                    _OutlineWidth,
                    0.0,
                    dist
                );

                //アウトラインマスク
                float outline = smoothstep(
                    _OutlineWidth * 2.0,
                    _OutlineWidth,
                    dist
                ) - core;

                //色合成
                float3 color =
                    _Color.rgb * _Glow * core +
                    _OutlineColor.rgb * outline;

                float alpha = saturate(core + outline);

                return float4(color,alpha);

            }
            ENDCG
        }
    }
}
