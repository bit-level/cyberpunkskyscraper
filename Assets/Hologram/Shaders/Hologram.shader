Shader "My Shaders/Hologram"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScrollSpeed ("Scroll Speed", Float) = 1
        _Color("Color", Color) = (1,1,1,1)
        _LightDir("Light Direction", Vector) = (0, 1, 0, 0)
        _Intensity("Intensity", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull back
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ScrollSpeed;
            float4 _Color;
            float4 _LightDir;
            float _Intensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float4 screenPos = ComputeScreenPos(o.vertex);
                o.uv = screenPos.xy / screenPos.w;
                o.normal = v.normal;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                _MainTex_ST.w += _Time.y * _ScrollSpeed;

                fixed4 col = tex2D(_MainTex, i.uv * _MainTex_ST.y + _MainTex_ST.w);
                col.a = col.r;
                col *= _Color;

                float3 lit = clamp(dot(_LightDir, i.normal), 0, 1) * _Color * _Intensity;
                col += float4(lit, 0);

                return col;
            }
            ENDCG
        }
    }
}
