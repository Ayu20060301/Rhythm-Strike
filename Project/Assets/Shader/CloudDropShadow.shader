Shader "Unlit/CloudDropShadow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ShadowColor ("Shadow Color", Color) = (0,0,0,0.5)
        _ShadowOffset ("Shadow Offset", Vector) = (0.02, -0.02, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off


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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ShadowColor;
            float4 _ShadowOffset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // âe
                fixed4 shadow = tex2D(_MainTex, i.uv + _ShadowOffset.xy);
                shadow.rgb = _ShadowColor.rgb;
                shadow.a *= _ShadowColor.a;

                // ñ{ëÃ
                fixed4 main = tex2D(_MainTex, i.uv);

                // çáê¨
                fixed4 col = shadow;
                col = lerp(col, main, main.a);

                return col;
            }
            ENDCG
        }
    }
}
