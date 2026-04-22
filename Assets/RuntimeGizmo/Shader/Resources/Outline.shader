//Taken and modified from github.com/Shrimpey/Outlined-Diffuse-Shader-Fixed/blob/master/CustomOutline.shader

Shader "Custom/Outline" {
	Properties {
        _OutlineColor ("Outline Color", Color) = (0.486, 0.380, 0.223, 1)
        _Outline ("Outline width", Range (0, 0.1)) = 0.05
    }
    SubShader {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" "LightMode" = "UniversalForward" }
        
        Pass {
            Name "OUTLINE"
            Cull Front
            ZWrite On

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct Varyings {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            float _Outline;
            float4 _OutlineColor;

            Varyings vert(Attributes v) {
                Varyings o;
                // Scale vertex for outline
                float3 posOS = v.vertex.xyz * (1 + _Outline);
                o.pos = TransformObjectToHClip(posOS);
                o.color = _OutlineColor;
                return o;
            }

            half4 frag(Varyings i) : SV_Target {
                return i.color;
            }
            ENDHLSL
        }
    }
    Fallback "Diffuse"
}
