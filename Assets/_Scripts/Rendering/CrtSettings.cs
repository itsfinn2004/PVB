// Made by Niek Melet 15/5/2025

using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FistFury.Rendering
{
    [System.Serializable]
    public class CrtSettings
    {
        public RenderPassEvent RenderPassEvent =
            UnityEngine.Rendering.Universal.RenderPassEvent.AfterRenderingPostProcessing;
        public Material CRTMaterial = null;
        [Range(0f, 0.3f)] public float Curvature = 0.1f;
        [Range(0f, 1f)] public float ScanlineIntensity = 0.5f;
        [Range(1f, 2000f)] public float ScanlineCount = 900f;
        [Range(0f, 5f)] public float RgbShift = 0.5f;
        [Range(0.5f, 1.5f)] public float Brightness = 1.2f;
        [Range(0.5f, 1.5f)] public float Contrast = 1.1f;
        [Range(0f, 0.1f)] public float Flicker;
        [Range(0f, 1f)] public float VignetteIntensity = 0.3f;
        [Range(0f, 0.5f)] public float NoiseIntensity = 0.05f;
    }
}