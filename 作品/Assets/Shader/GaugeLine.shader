Shader "Unlit/GaugeLine"
{
    Properties
    {
        _MainColor("Main Color",Color) = (1,1,1,1)
       _LineColor("Line Color",Color) = (0.2,0.9,1,1)
       _LineCount("Line Count",float) = 12
       _LineWidth("Line Width",Range(0.01,0.2)) = 0.05
       _WaveStrength("Wave Strength",Range(0,0.2)) = 0.04
       _WaveSpeed("Wave Speed",float) = 1.5
       _ScrollSpeed("Scroll Speed",float) = 0.3

    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

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

            float4 _MainColor;
            float4 _LineColor;
            float _LineCount;
            float _LineWidth;
            float _WaveStrength;
            float _WaveSpeed;
            float _ScrollSpeed;
           
            v2f vert (appdata v)
            {
               v2f o;
               o.vertex = UnityObjectToClipPos(v.vertex);
               o.uv = v.uv;
               return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
              float2 uv = i.uv;

              //ècÉXÉNÉçÅ[Éã
              uv.y += _Time.y * _ScrollSpeed;

              //îgÇÃòcÇ›
              float wave = 
                   sin(uv.y * _LineCount * 2 + _Time.y * _WaveSpeed)
                   * _WaveStrength;


              //ÉâÉCÉìê∂ê¨
              float lineDist = abs(frac((uv.y + wave) * _LineCount) - 0.5);

              float mask = 1.0 - smoothstep(0.0,_LineWidth,lineDist);

              //çáê¨
              float4 col = lerp(_MainColor,_LineColor,mask);
              col.a = _LineColor.a;

              return col;

            }
            ENDCG
        }
    }
}
