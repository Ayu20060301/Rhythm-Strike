Shader "Unlit/SelectFrame"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor1("Main Color1",Color) = (1,1,1,1)
        _MainColor2("Main Color2",Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
            float4 _MainTex_ST;
            float4 _MainColor1; 
            float4 _MainColor2; 

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

                float y = i.uv.y;

                //ラインの太さ
                float lineWidth = 0.001;

                //ラインの位置
                float linePos = 0.15;

                //ラインの色
                fixed4 lineColor = fixed4(0,0,0,1);

                //ラインの描画
                if(abs(y - linePos) < lineWidth)
                {
                    return lineColor;
                }
                //上半分なら_MainColor1を、下半分なら_MainColor2に調整する
               if(y > 0.3)
               {
                   return _MainColor1;
               }
               else
               {
                   return _MainColor2;
               }
            }
            ENDCG
        }
    }
}
