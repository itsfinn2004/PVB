// Made by Niek Melet on 15/5/2025

Shader "FoF/CRTFilter"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Curvature ("Curvature", Range(0, 0.3)) = 0.1
        _PaniniDistance ("Panini Distance", Range(0, 1)) = 0.3
        _PaniniCropToFit ("Panini Crop To Fit", Range(0, 1)) = 0.5
        _ScanlineIntensity ("Scanline Intensity", Range(0, 1)) = 0.5
        _ScanlineCount ("Scanline Count", Range(1, 2000)) = 900
        _ScanlineSpeed ("Scanline Speed", Range(-10, 10)) = 2.0
        _RGBShift ("RGB Shift", Range(0, 5)) = 0.5
        _Brightness ("Brightness", Range(0.5, 1.5)) = 1.2
        _Contrast ("Contrast", Range(0.5, 1.5)) = 1.1
        _Flicker ("Flicker", Range(0, 0.1)) = 0.03
        _VignetteIntensity ("Vignette Intensity", Range(0, 1)) = 0.3
        _NoiseIntensity ("Noise Intensity", Range(0, 0.5)) = 0.05
    	
    	[Header(Vsync Issues)]
    	_VSyncLineWidth ("VSYnc Line Width", Range(0.001, 0.1)) = 0.03
    	_VSyncLineSpeed ("VSync Line Speed", Range(-3, 3)) = 0.8
    	_VSyncDistortion ("VSync Distortion Amount", Range(0, 0.01)) = 0.005
    	_VSyncFrequency ("VSync Frequency", Range(0, 1)) = 0.5
    	_VSyncRandomness ("VSYNC Randomness", Range(0, 1)) = 0.7
        _VSyncJitter ("VSYNC Jitter", Range(0, 1)) = 0.3
        _VSyncDoubleLineChance ("Double Line Chance", Range(0, 1)) = 0.3
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
            float _PaniniDistance;
            float _PaniniCropToFit;
            float _ScanlineIntensity;
            float _ScanlineCount;
            float _ScanlineSpeed;
            float _RGBShift;
            float _Brightness;
            float _Contrast;
            float _Flicker;
            float _VignetteIntensity;
            float _NoiseIntensity;
            float _ScanlineTime;

            float _VSyncLineWidth;
            float _VSyncLineSpeed;
            float _VSyncDistortion;
            float _VSyncFrequency;
            float _VSyncRandomness;
            float _VSyncJitter;
            float _VSyncDoubleLineChance;
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                return output;
            }
            
            // Function for noise
            float random(float2 st)
            {
                return frac(sin(dot(st.xy, float2(12.9898, 78.233))) * 43758.5453123);
            }
            
            // Panini projection function
            float2 paniniProjection(float2 uv)
            {
                // calc normalized coords
                float2 screenCoord = uv * 2.0 - 1.0;
                
                // Parameters
                float d = _PaniniDistance;
                float cropFactor = _PaniniCropToFit;
                
                // calc effect
                float paniniFactorX = (1.0 - d) + d / (1.0 + screenCoord.x * screenCoord.x);
                float2 paniniCoord;
                
                // apply
                paniniCoord.x = screenCoord.x * paniniFactorX;
                paniniCoord.y = screenCoord.y * paniniFactorX;

                float2 croppedCoord = paniniCoord * lerp(1.0, min(1.0 / max(abs(paniniCoord.x), abs(paniniCoord.y)), 1.0), cropFactor);
                
                // combine
                float2 offset = abs(croppedCoord.yx) / float2(6.0, 4.0);
                croppedCoord = croppedCoord + croppedCoord * offset * offset * _Curvature;
                
                // back to 0-1 range
                return croppedCoord * 0.5 + 0.5;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                // project panini
                float2 projectedUV = paniniProjection(input.uv);
                
                // Check if we're out bounds
                if (projectedUV.x < 0.0 || projectedUV.x > 1.0 || projectedUV.y < 0.0 || projectedUV.y > 1.0)
                {
                    return half4(0.0, 0.0, 0.0, 1.0);
                }

                // VSYNC issue implementation
                float2 vsyncUV = projectedUV;
                
                // More random time variations
                float randTime = _ScanlineTime + sin(_ScanlineTime * 0.7) * _VSyncRandomness;
                
                // Calculate base VSYNC line position with added jitter
                float jitter = (sin(_ScanlineTime * 32.457) * 0.5 + 0.5) * _VSyncJitter * 0.05;
                float vsyncLinePos = frac(randTime * _VSyncLineSpeed) + jitter;
                
                // Potentially add a second line if we hit the random chance
                float randomDouble = step(1.0 - _VSyncDoubleLineChance, frac(sin(randTime * 45.32) * 43758.5453));
                float secondLinePos = frac(vsyncLinePos + 0.2);
                
                // Determine if the current pixel is within the VSYNC line(s)
                float vsyncLine = smoothstep(0, _VSyncLineWidth * (1.0 + sin(randTime * 13.0) * 0.3), 
                                   1 - abs(vsyncUV.y - vsyncLinePos) / _VSyncLineWidth);
                
                // Add the second line if randomDouble is 1
                float secondLine = randomDouble * smoothstep(0, _VSyncLineWidth * 0.8, 
                                   1 - abs(vsyncUV.y - secondLinePos) / (_VSyncLineWidth * 0.8));
                vsyncLine = max(vsyncLine, secondLine);
                
                // Make the VSYNC effect appear randomly based on frequency and add some randomness
                float randomTrigger = step(1.0 - _VSyncFrequency * (0.8 + sin(randTime * 0.3) * 0.2), 
                                     frac(sin(floor(randTime * 0.5) * 78.233) * 43758.5453));
                
                vsyncLine *= randomTrigger;
                
                // Create more complex distortion pattern
                float distortionOffset = 0;
                if (vsyncLine > 0.01) {
                    // Base wave distortion
                    float baseWave = sin(vsyncUV.y * 100.0 + randTime * 10.0) * _VSyncDistortion;
                    
                    // Add secondary noise pattern
                    float noisePattern = random(float2(vsyncUV.y * 40.0, randTime)) * 2.0 - 1.0;
                    
                    // Combine with varying intensity
                    distortionOffset = baseWave + noisePattern * _VSyncDistortion * 0.7;
                    
                    // Add occasional stronger glitch
                    float glitchChance = step(0.95, random(float2(floor(randTime * 2.0), 0.5)));
                    distortionOffset *= 1.0 + glitchChance * 2.0;
                }
                
                // Apply the distortion with intensity based on position within the line
                vsyncUV.x += distortionOffset * vsyncLine;
                
                // Use the distorted UVs proportional to vsyncLine strength
                projectedUV = lerp(projectedUV, vsyncUV, vsyncLine * 0.95);

                // RGB shifting for color delay
                float shift = _RGBShift * 0.001;
                
                float3 col;
                col.r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(projectedUV.x + shift, projectedUV.y)).r;
                col.g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, projectedUV).g;
                col.b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(projectedUV.x - shift, projectedUV.y)).b;
                
                // Scanlines with movement
                float scanlineY = projectedUV.y + (_ScanlineTime * _ScanlineSpeed * 0.01);
                float scanline = sin(scanlineY * _ScanlineCount * 3.14159265359) * 0.5 + 0.5;
                scanline = 1.0 - (_ScanlineIntensity * (1.0 - scanline));
                col *= scanline;
                
                // Flicker effect
                float flicker = random(float2(_Time.x * 10, _Time.y * 10)) * 2.0 - 1.0;
                flicker *= _Flicker;
                col *= (1.0 + flicker);
                
                // Brightness & contrast
                col = (col - 0.5) * _Contrast + 0.5;
                col *= _Brightness;
                
                // Add noise
                float noise = random(projectedUV * _Time.x);
                col += (noise - 0.5) * _NoiseIntensity;
                
                // Add vignette
                float2 vignetteUV = projectedUV * 2.0 - 1.0;
                float vignette = 1.0 - dot(vignetteUV, vignetteUV) * _VignetteIntensity;
                col *= vignette;

                // enhance vsync line by increasing brightness along it
                col += vsyncLine * 0.05;
                
                return half4(col, 1.0);
            }
            ENDHLSL
        }
    }

    Fallback "Hidden/Universal Render Pipeline/FallbackError"
}