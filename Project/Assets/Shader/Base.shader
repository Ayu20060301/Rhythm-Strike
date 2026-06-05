Shader "Unlit/Base"
{
    Properties
    {
        _LaneCount ("Lane Count", Float) = 4
        _LaneSpacing("Lane Spacing",Range(0,0.5)) = 0.1
        _LineWidth ("Line Width", Range(0.001,0.05)) = 0.01
        _LineColor ("Line Color", Color) = (1,1,1,1)
        _BgColor ("Bg Color", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"}
       
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
          
           
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

            float _LaneCount;
            float _LaneSpacing;
            float _LineWidth;
            fixed4 _LineColor;
            fixed4 _BgColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
             

                float usableWidth = 1.0 - _LaneSpacing;

                //中央基準でUVを圧縮
                float x = (i.uv.x - 0.5) / usableWidth + 0.5;

               //レーン分割
               float lanePos = x * _LaneCount;

               //各レーン中央からの距離
               float dist = abs(frac(lanePos) - 0.5);

               //ライン生成
               float lineMask = smoothstep(
                   _LineWidth * 0.5,
                   _LineWidth,
                   dist
               );

               return lerp(_BgColor,_LineColor,lineMask);

            }
            ENDCG
        }
    }
}
