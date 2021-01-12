Shader "My Shaders/Cars"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Light ("Light Direction", Vector) = (1,1,1,1)
        _LightIntensity("Light Intensity", Float) = 1
        _Ambient("Ambient Color", Color) = (0,0,0,0)
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
            float4 _Light;
            float _LightIntensity;
            float4 _Ambient;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normal;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed diffuse = dot(i.normal, normalize(_Light)) * _LightIntensity;

                fixed4 col = tex2D(_MainTex, i.uv);
                col *= diffuse;
                col += _Ambient;

                return col;
            }
            ENDCG
        }
    }
}
