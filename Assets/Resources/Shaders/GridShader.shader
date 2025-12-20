Shader "Custom/GridShader"
{
    Properties
    {
        _GridColor ("Grid Color", Color) = (0, 0, 0, 1)
        _BackgroundColor ("Background Color", Color) = (1, 1, 1, 1)
        _GridSize ("Grid Size", Float) = 1.0
        _LineThickness ("Line Thickness", Range(0.01, 0.5)) = 0.05
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

            float4 _GridColor;
            float4 _BackgroundColor;
            float _GridSize;
            float _LineThickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Skala UV-koordinaterna med rutnätsstorleken
                float2 scaledUV = i.uv * _GridSize;
                
                // Beräkna avståndet till närmaste gridlinje
                float2 grid = abs(frac(scaledUV - 0.5) - 0.5);
                float dist = min(grid.x, grid.y);
                
                // Skapa linjer baserat på tjockleken
                float gridMask = step(dist, _LineThickness / _GridSize);
                
                // Blanda mellan bakgrundsfärg och gridfärg
                fixed4 col = lerp(_BackgroundColor, _GridColor, gridMask);
                
                return col;
            }
            ENDCG
        }
    }
}
