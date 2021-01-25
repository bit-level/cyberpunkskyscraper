Shader "Unlit/Mask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Intensity ("Intensity", Float) = 1
        _Mask ("Mask", 2D) = "white" {}
        _MaskMul ("Mask Multiplier", Float) = 1
    }
    SubShader
    {
        Tags {
        	"RenderType"="Transparent"
        	"RenderQueue"="Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha

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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 position : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Intensity;
            sampler2D _Mask;
            float4 _Mask_ST;
            float _MaskMul;

            v2f vert (appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color * _Intensity;
                col.a *= clamp((1 - tex2D(_Mask, i.uv).r) * _MaskMul, 0, 1);
                return col;
            }
            ENDCG
        }
    }
}
