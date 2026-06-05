Shader "Unlit/CornerMarkers"
{
    Properties
    {
       _Color("Color" , Color) = (1,1,1,1)
       _Size("Corner Size",float) = 0.1
       _Selected("Selected", float) = 0.0
    }
    SubShader
    {
        Tags { "Queue" = "Overlay"}
        LOD 100

        Pass
        {

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

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
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Size;
            float _Selected;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {

                if (_Selected < 0.5) return float4(0,0,0,0); //‘I‘ð‚³‚ê‚Ä‚È‚¢‚Æ“§–¾

                float2 uv = i.uv;
                float alpha = 0.0f;

                //¶‰º
                if (uv.x < _Size && uv.y < _Size) alpha = 1.0f;
                //‰E‰º
                if (uv.x > 1.0 - _Size && uv.y < _Size) alpha = 1.0f;
                //¶ã
                if (uv.x < _Size && uv.y > 1.0 - _Size) alpha = 1.0f;
                //‰Eã
                if (uv.x > 1.0 - _Size && uv.y > 1.0 - _Size) alpha = 1.0f;

                return float4(_Color.rgb, alpha * _Color.a);

            }
            ENDCG
        }
    }
}
