// Made by Niek Melet 15/5/2025

using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FistFury.Rendering
{
    [System.Serializable]
    public class CrtSettings
    {
        [Header("General Settings")]
        public bool IsEnabled = true;
        public RenderPassEvent RenderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        public Material CRTMaterial;

        [Header("Distortion")]
        [Range(0f, 0.3f)] public float Curvature = 0.1f;
        [Range(0f, 1f)] public float PaniniDistance = 0.3f;
        [Range(0f, 1f)] public float PaniniCropToFit = 0.5f;

        [Header("Scan lines")]
        [Range(0f, 1f)] public float ScanlineIntensity = 0.5f;
        [Range(1f, 2000f)] public float ScanlineCount = 900f;
        [Range(-10, 10)] public float ScanlineSpeed = 2.0f;

        [Header("Color Adjustments")]
        [Range(0f, 5f)] public float RgbShift = 0.5f;
        [Range(0.5f, 1.5f)] public float Brightness = 1.2f;
        [Range(0.5f, 1.5f)] public float Contrast = 1.1f;

        [Header("Effects")]
        [Range(0f, 0.1f)] public float Flicker;
        [Range(0f, 1f)] public float VignetteIntensity = 0.3f;
        [Range(0f, 0.5f)] public float NoiseIntensity = 0.05f;

        [Header("VSYNC Issues")]
        [Range(0.001f, 0.1f)] public float VSyncLineWidth = 0.03f;
        [Range(-3f, 3f)] public float VSyncLineSpeed = 0.8f;
        [Range(0f, 0.1f)] public float VSyncDistortion = 0.04f;
        [Range(0f, 1f)] public float VSyncFrequency = 0.5f;
    }
}