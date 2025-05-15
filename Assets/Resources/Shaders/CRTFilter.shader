// Made by Niek Melet on 15/5/2025

Shader "FoF/CRTFilter"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Curvature ("Curvature", Range(0, 0.3)) = 0.1
        _ScanlineIntensity ("Scanline Intensity", Range(0, 1)) = 0.5
        _ScanlineCount ("Scanline Count", Range(1, 2000)) = 900
        _RGBShift ("RGB Shift", Range(0, 5)) = 0.5
        _Brightness ("Brightness", Range(0.5, 1.5)) = 1.2
        _Contrast ("Contrast", Range(0.5, 1.5)) = 1.1
        _Flicker ("Flicker", Range(0, 0.1)) = 0.03
        _VignetteIntensity ("Vignette Intensity", Range(0, 1)) = 0.3
        _NoiseIntensity ("Noise Intensity", Range(0, 0.5)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }
        LOD 100
        ZTest Always
        ZWrite Off
        Cull Off
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
            };
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            
            float _Curvature;
            float _ScanlineIntensity;
            float _ScanlineCount;
            float _RGBShift;
            float _Brightness;
            float _Contrast;
            float _Flicker;
            float _VignetteIntensity;
            float _NoiseIntensity;
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                return output;
            }
            
            // function for noise
            float random(float2 st)
            {
                return frac(sin(dot(st.xy, float2(12.9898, 78.233))) * 43758.5453123);
            }
            
            // calculates curved uv coordinates
            float2 curveUV(float2 uv)
            {
                float2 curvedUV = uv * 2.0 - 1.0;
                float2 offset = abs(curvedUV.yx) / float2(6.0, 4.0);
                curvedUV = curvedUV + curvedUV * offset * offset * _Curvature;
                return curvedUV * 0.5 + 0.5;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                // apply curvature
                float2 curvedUV = curveUV(input.uv);
                
                // check if we're out bounds
                if (curvedUV.x < 0.0 || curvedUV.x > 1.0 || curvedUV.y < 0.0 || curvedUV.y > 1.0)
                {
                    return half4(0.0, 0.0, 0.0, 1.0);
                }
                
                // rgb shifting for color delay
                float shift = _RGBShift * 0.001;
                
                float3 col;
                col.r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(curvedUV.x + shift, curvedUV.y)).r;
                col.g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, curvedUV).g;
                col.b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(curvedUV.x - shift, curvedUV.y)).b;
                
                // Scanlines
                float scanline = sin(curvedUV.y * _ScanlineCount * 3.14159265359) * 0.5 + 0.5;
                scanline = 1.0 - (_ScanlineIntensity * (1.0 - scanline));
                col *= scanline;
                
                // flicker effect
                float flicker = random(float2(_Time.y * 10, _Time.y * 10)) * 2.0 - 1.0;
                flicker *= _Flicker;
                col *= (1.0 + flicker);
                
                // brightness & contrast
                col = (col - 0.5) * _Contrast + 0.5;
                col *= _Brightness;
                
                // add noise
                float noise = random(curvedUV * _Time.y);
                col += (noise - 0.5) * _NoiseIntensity;
                
                // add vignette
                float2 vignetteUV = curvedUV * 2.0 - 1.0;
                float vignette = 1.0 - dot(vignetteUV, vignetteUV) * _VignetteIntensity;
                col *= vignette;
                
                return half4(col, 1.0);
            }
            ENDHLSL
        }
    }

    Fallback "Hidden/Universal Render Pipeline/FallbackError"
}
