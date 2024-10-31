Shader "Custom/two_sided"
{
     Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" }
        LOD 200

        // Disable backface culling for two-sided rendering
        Cull Off

        CGPROGRAM
        #pragma surface surf Standard alpha:clip

        sampler2D _MainTex;
        half _Glossiness;
        half _Metallic;
        half _Cutoff;
        fixed4 _Color;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;

            // Alpha clip for cutout effect
            clip(c.a - _Cutoff);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
