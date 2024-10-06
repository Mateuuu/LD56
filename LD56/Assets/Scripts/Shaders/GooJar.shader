Shader "Custom/GooJar"
{
    Properties
    {
        // Glass properties
        _GlassColor ("Glass Color", Color) = (1,1,1,0.1)
        _GlassSmoothness ("Glass Smoothness", Range(0,1)) = 0.9
        _GlassMetallic ("Glass Metallic", Range(0,1)) = 0.0

        // Liquid properties
        _LiquidColor ("Liquid Color", Color) = (0,0.5,1,1)
        _FillLevel ("Fill Level", Range(0,1)) = 0.5
        _SloshAmount ("Slosh Amount", Range(0,0.2)) = 0.05
        _SloshSpeed ("Slosh Speed", Range(0,5)) = 1.0

        // Bottle properties
        _BottleBaseY ("Bottle Base Y", Float) = 0.0
        _BottleHeight ("Bottle Height", Float) = 1.0

        // Optional texture for liquid surface distortion
        _BumpMap ("Normal Map", 2D) = "bump" {}

        // Emissive properties
        _LiquidEmissiveColor ("Liquid Emissive Color", Color) = (0,0.5,1,1)
        _LiquidEmissiveIntensity ("Liquid Emissive Intensity", Range(0,10)) = 1.0

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 300


            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back
            ZWrite Off

            CGPROGRAM
            #pragma surface surf Standard fullforwardshadows

            sampler2D _BumpMap;

            struct Input
            {
                float3 worldPos;
                float3 viewDir;
                float2 uv_BumpMap;
                float3 localPos;
            };

            // Glass properties
            half4 _GlassColor;
            half _GlassSmoothness;
            half _GlassMetallic;

            // Liquid properties
            half4 _LiquidColor;
            half _FillLevel;
            half _SloshAmount;
            half _SloshSpeed;

            // Bottle properties
            half _BottleBaseY;
            half _BottleHeight;

            // Emissive properties
            half4 _LiquidEmissiveColor;
            half _LiquidEmissiveIntensity;


            void surf (Input IN, inout SurfaceOutputStandard o)
            {
                // Calculate local position
                float3 localPos = mul(unity_WorldToObject, float4(IN.worldPos, 1.0)).xyz;

                // Glass material
                o.Albedo = _GlassColor.rgb;
                o.Alpha = _GlassColor.a;
                o.Metallic = _GlassMetallic;
                o.Smoothness = _GlassSmoothness;

                o.Emission = fixed3(0, 0, 0); // No emission for glass

                // Calculate sloshing effect
                float time = _Time.y * _SloshSpeed;
                float wave = sin(localPos.x * 5 + time) * _SloshAmount;

                // Calculate the liquid level based on fill level and bottle height
                float liquidLevel = _BottleBaseY + (_FillLevel * _BottleHeight) + wave;

                // If the fragment is below the liquid level, render liquid instead of glass
                if (localPos.y <= liquidLevel)
                {
                    o.Albedo = _LiquidColor.rgb;
                    o.Alpha = _LiquidColor.a;
                    o.Metallic = 0.0;
                    o.Smoothness = 0.8;

                    // Apply normal mapping for liquid surface detail
                    float3 bump = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
                    o.Normal = bump;

                    // Set emissive color for the liquid
                    o.Emission = _LiquidEmissiveColor.rgb * _LiquidEmissiveIntensity;
                }
            }
            ENDCG
    }
    FallBack "Transparent/Diffuse"
}
