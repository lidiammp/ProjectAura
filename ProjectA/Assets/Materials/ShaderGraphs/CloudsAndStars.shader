Shader "Unlit/CloudsAndStars"
{
    Properties
    {
        _CloudTex("Cloud Texture", 2D) = "white" {}
        _CloudColor("Cloud Color", Color) = (1,1,1,1)
        _Speed("Scroll Speed", Vector) = (0.01, 0, 0, 0)
        _StarDensity("Star Density", Range(0,1)) = 0.5
        _StarBrightness("Star Brightness", Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _CloudTex;
            float4 _CloudColor;
            float4 _Speed;
            float _StarDensity;
            float _StarBrightness;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // Simple hash for per-pixel randomness
            float rand(float2 co)
            {
                return frac(sin(dot(co.xy, float2(12.9898,78.233))) * 43758.5453);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv + _Time.y * _Speed.xy; // move clouds over time
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // --- Clouds ---
                fixed4 cloudTex = tex2D(_CloudTex, i.uv);
                float cloudAlpha = cloudTex.r;
                fixed4 cloudColor = fixed4(_CloudColor.rgb, cloudAlpha * _CloudColor.a);

                // --- Stars ---
                float n = rand(i.uv * 1000.0); // random per pixel

                // Decide if star is twinkling or static
                float starAlpha = 0;
                if (n > 0.7)
                {
                    // Twinkling star
                    float phase = n * 6.2831;
                    starAlpha = abs(sin(_Time.y * 5.0 + phase));
                }
                else
                {
                    // Static star
                    starAlpha = 1.0;
                }

                // Star density mask
                starAlpha *= step(1.0 - _StarDensity, n);
                fixed4 starColor = fixed4(1,1,1, starAlpha * _StarBrightness);

                // --- Combine ---
                fixed4 finalColor = cloudColor + starColor * (1 - cloudAlpha); // clouds occlude stars

                return finalColor;
            }
            ENDCG
        }
    }
}
