Shader "Unlit/RunUI"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TopColor("Top Color",Color) = (1,1,1,1)
        _BottomColor("Bottob Color",Color) = (0,0,0,1)
        _Alpha("Alpha", Range(0,1)) = 1
        _Glow("Glow",Range(0,3)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "RenderType" = "Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha

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

            sampler2D _MainTex;
            float4 _TopColor;
            float4 _BottomColor;
            float _Alpha;
            float _Glow;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                

                //UV.yを使って縦グラデーション
                fixed4 grad = lerp(_BottomColor,_TopColor,i.uv.y);

               fixed4 result = col * grad;

               //発光
               result.rgb += grad.rgb * _Glow;

               result.a *= _Alpha;

               return result;

            }
            ENDCG
        }
    }
}
