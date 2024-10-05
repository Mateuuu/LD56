Shader "Custom/CustomOutline" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _Outline ("Outline Color", Color) = (0,0,0,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Size ("Outline Thickness", Float) = 1.5
        _EmissionColor ("Emission Color", Color) = (0,0,0,1)
        _Emission ("Emission", Float) = 0
    }
    SubShader {
        Tags { "RenderType" = "Opaque" }
        LOD 200


        // First pass: Render the outline
        Pass {
            Cull Front // Cull front faces to create the outline effect
            ZTest Always // Render this on top of everything
            ZWrite Off  // Do not write to the depth buffer

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            half _Size;
            fixed4 _Outline;

            struct v2f {
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata_base v) {
                v2f o;
                // Expand the vertices to create an outline effect
                v.vertex.xyz += normalize(v.vertex.xyz) * _Size;
                if(_Size == 0){
                    v.vertex.xyz = 0;
                }
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag (v2f i) : SV_Target {
                // Render the outline color
                return _Outline;
            }
            ENDCG
        }

        // Second pass: Render the actual object
        ZTest LEqual // Respect depth, allowing proper occlusion
        ZWrite On  // Write to the depth buffer
        Cull Back  // Cull back faces for the normal rendering

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        float4 _EmissionColor;
        float _Emission;

        struct Input {
            float2 uv_MainTex;
        };

        half _Glossiness;
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o) {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            o.Emission = _EmissionColor * _Emission;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
