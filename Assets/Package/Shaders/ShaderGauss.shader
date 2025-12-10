Shader "Sprites/GrabBlur"
{
    Properties
    {
        _MainTex ("Mask (Sprite)", 2D) = "white" {} // sert de masque (forme/opacité du sprite)
        _BlurSize ("Blur Size", Range(0,6)) = 2
        _Darken ("Darken (0..1)", Range(0,1)) = 0.4 // assombrissement optionnel
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "CanUseSpriteAtlas"="True" }
        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        // Capture l'écran dans _GrabTexture (Built-in RP)
        GrabPass { }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;
            float _BlurSize;
            float _Darken;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f {
                float2 uv      : TEXCOORD0;
                float4 pos     : SV_POSITION;
                float4 grabPos : TEXCOORD1; // pour échantillonner l'écran
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.grabPos = ComputeGrabScreenPos(o.pos);
                return o;
            }

            fixed4 SampleScreen(float2 uv)
            {
                return tex2D(_GrabTexture, uv);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // UV écran (image derrière le sprite)
                float2 uv = i.grabPos.xy / i.grabPos.w;

                // 9-tap box blur (rapide). Augmente _BlurSize pour plus fort.
                float2 off = _GrabTexture_TexelSize.xy * _BlurSize;

                fixed4 c  = SampleScreen(uv);
                c += SampleScreen(uv + float2(+off.x, 0));
                c += SampleScreen(uv + float2(-off.x, 0));
                c += SampleScreen(uv + float2(0, +off.y));
                c += SampleScreen(uv + float2(0, -off.y));
                c += SampleScreen(uv + float2(+off.x, +off.y));
                c += SampleScreen(uv + float2(+off.x, -off.y));
                c += SampleScreen(uv + float2(-off.x, +off.y));
                c += SampleScreen(uv + float2(-off.x, -off.y));
                c *= 1.0/9.0;

                // Assombrissement optionnel (comme un panneau noir)
                c.rgb = lerp(c.rgb, 0, _Darken);

                // Masque = alpha du sprite (forme + opacité)
                float mask = tex2D(_MainTex, i.uv).a;

                return fixed4(c.rgb, mask);
            }
            ENDCG
        }
    }
    FallBack Off
}
