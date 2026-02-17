Shader "Unlit/BG_Load"
{
    Properties
    {
       _ColorA("Stripe Color",Color) = (1,1,1,1)
       _ColorB("BackGround Color",Color) = (0,0,0,1)

       _Density("Stripe Density",float) = 20
       _Width("Stripe Width",Range(0,1)) = 0.5

       _Angle("Angle (Degree)",Range(0,360)) = 45

       _Speed("Scroll Speed",float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
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

            float4 _ColorA;
            float4 _ColorB;
            float _Density;
            float _Width;
            float _Angle;
            float _Speed;
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
               //角度 → ラジアン
               float rad = radians(_Angle);

               //方向ベクトル
               float2 dir = float2(cos(rad),sin(rad));

               //斜め方向の座標
               float stripeCoord = 
                    dot(i.uv,dir) * _Density
                    + _Time.y * _Speed;

               //周期化
               float stripe = frac(stripeCoord);

               //アンチエイリアス
               float aa = fwidth(stripe);

               //ストライプマスク
               float mask = smoothstep(
                   _Width - aa,
                   _Width + aa,
                   stripe
               );


               return lerp(_ColorA,_ColorB,mask);
            }
            ENDCG
        }
    }
}
