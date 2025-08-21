Shader "Unlit/CloudLayer"
{
    Properties
    {
        _CloudTex ("Cloud Texture", 2D) = "white" {}
        _CloudColor ("Cloud Color", Color) = (1,1,1,1)
        _Speed ("Scroll Speed", Vector) = (0.01, 0.0, 0, 0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _CloudTex;
            float4 _CloudColor;
            float4 _Speed;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                
                // Use Unity's built-in _Time.y (scaled time) for scrolling
                o.uv = v.uv + _Time.y * _Speed.xy;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_CloudTex, i.uv);

                // Treat brightness (R channel) as alpha
                float cloudAlpha = tex.r;

                return fixed4(_CloudColor.rgb, cloudAlpha * _CloudColor.a);
            }

            ENDCG
        }
    }
}
