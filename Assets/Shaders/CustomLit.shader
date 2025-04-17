Shader "Custom/SimpleDiffuse"
{
    Properties
    {
        _Color ("Surface Color", Color) = (1,1,1,1) // Свойство для выбора цвета
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        CGPROGRAM
        #pragma surface surf Lambert // Используем стандартную модель освещения Lambert
        
        // Входные данные структуры SurfaceInput
        struct Input
        {
            float2 uv_MainTex;
        };
        
        // Свойство цвета
        fixed4 _Color;
        
        // Функция поверхности
        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = _Color.rgb; // Устанавливаем цвет поверхности
            o.Alpha = _Color.a;
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}