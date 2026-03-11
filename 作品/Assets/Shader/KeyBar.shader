Shader "Unlit/KeyBar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TintColor("Tint Color",Color) = (1,1,1,1)
        _FadeStart("Fade Start(0 - 1)",Range(0,1)) = 0.5
        _FadeEnd("fade End(0 - 1)",Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" ="Transparent" }
        LOD 100


        Blend SrcAlpha OneMinusSrcAlpha
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
            float4 _TintColor;
            float _FadeStart;
            float _FadeEnd;


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
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
               //色変更
               col *= _TintColor;

               //上方向フェード
               float fade = smoothstep(_FadeStart,_FadeEnd,i.uv.y);

               col.a *= (1.0 - fade);

                return col;
            }
            ENDCG
        }
    }
}
