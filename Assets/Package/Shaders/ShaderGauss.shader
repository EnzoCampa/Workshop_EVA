Shader "Hidden/GaussianBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Float) = 1.0
        _Downsample ("Downsample", Float) = 1.0
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass // Horizontal
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _BlurSize;

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float4 pos:SV_POSITION; float2 uv:TEXCOORD0; };

            v2f vert (appdata v){ v2f o; o.pos=UnityObjectToClipPos(v.vertex); o.uv=v.uv; return o; }

            fixed4 frag (v2f i):SV_Target
            {
                float2 off = float2(_MainTex_TexelSize.x * _BlurSize, 0);
                fixed4 c = tex2D(_MainTex, i.uv) * 0.38774;   // centre
                c += tex2D(_MainTex, i.uv + off) * 0.24477;
                c += tex2D(_MainTex, i.uv - off) * 0.24477;
                c += tex2D(_MainTex, i.uv + 2*off) * 0.06136;
                c += tex2D(_MainTex, i.uv - 2*off) * 0.06136;
                return c;
            }
            ENDCG
        }

        Pass // Vertical
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _BlurSize;

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float4 pos:SV_POSITION; float2 uv:TEXCOORD0; };

            v2f vert (appdata v){ v2f o; o.pos=UnityObjectToClipPos(v.vertex); o.uv=v.uv; return o; }

            fixed4 frag (v2f i):SV_Target
            {
                float2 off = float2(0, _MainTex_TexelSize.y * _BlurSize);
                fixed4 c = tex2D(_MainTex, i.uv) * 0.38774;
                c += tex2D(_MainTex, i.uv + off) * 0.24477;
                c += tex2D(_MainTex, i.uv - off) * 0.24477;
                c += tex2D(_MainTex, i.uv + 2*off) * 0.06136;
                c += tex2D(_MainTex, i.uv - 2*off) * 0.06136;
                return c;
            }
            ENDCG
        }
    }
}
