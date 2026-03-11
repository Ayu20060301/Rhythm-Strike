Shader "Unlit/CloudSea"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CloudColor("Cloud Color",Color) = (1,1,1,1)
        _Density("Density",Range(0,1)) = 0.5
        _Speed("Speed",Float) = 0.02
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

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
            float4 _CloudColor;
            float _Density;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                

                float2 uv = i.uv;
                uv.y += _Time.y * _Speed;

                float noise = tex2D(_MainTex,uv).r;
                float alpha = smoothstep(_Density,1.0,noise);

                fixed4 col = fixed4(_CloudColor.rgb,alpha * 0.6);

                UNITY_APPLY_FOG(i.fogCoord,col);
                
                return col;

            }
            ENDCG
        }
    }
}
