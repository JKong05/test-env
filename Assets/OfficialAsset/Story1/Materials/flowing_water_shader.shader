Shader "Custom/flowing_water_shader"
{
    Properties
    {
        _MainTex ("Water Texture", 2D) = "white" {}
        _FlowSpeed ("Flow Speed", Range(0.0, 1.0)) = 0.1
        _Tiling ("Tiling", Vector) = (1, 1, 0, 0)
        _FlowDirection ("Flow Direction", Vector) = (1, 0, 0, 0)
        _Color ("Water Color", Color) = (0.1, 0.5, 0.8, 1.0)
        _Opacity ("Opacity", Range(0.0, 1.0)) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

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

            sampler2D _MainTex;
            float _FlowSpeed;
            float4 _Color;
            float _Opacity;
            float2 _Tiling;
            float2 _FlowDirection;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Apply tiling and flow direction with time-based UV offset
                float2 uvOffset = _FlowDirection * _FlowSpeed * _Time.y;
                o.uv = v.uv * _Tiling + uvOffset;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture and apply color tint and opacity
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                col.a *= _Opacity; // Apply opacity
                return col;
            }
            ENDCG
        }
    }
}
