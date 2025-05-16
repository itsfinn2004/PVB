// Made by Niek Melet 15/5/2025

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FistFury.Rendering
{
    public class CrtRenderPass : ScriptableRenderPass
    {
        private static readonly int Curvature = Shader.PropertyToID("_Curvature");
        private static readonly int PaniniDistance = Shader.PropertyToID("_PaniniDistance");
        private static readonly int PaniniCropToFit = Shader.PropertyToID("_PaniniCropToFit");

        private static readonly int ScanlineIntensity = Shader.PropertyToID("_ScanlineIntensity");
        private static readonly int ScanlineCount = Shader.PropertyToID("_ScanlineCount");
        private static readonly int ScanlineSpeed = Shader.PropertyToID("_ScanlineSpeed");

        private static readonly int RGBShift = Shader.PropertyToID("_RGBShift");
        private static readonly int Brightness = Shader.PropertyToID("_Brightness");
        private static readonly int Contrast = Shader.PropertyToID("_Contrast");
        private static readonly int Flicker = Shader.PropertyToID("_Flicker");

        private static readonly int VignetteIntensity = Shader.PropertyToID("_VignetteIntensity");
        private static readonly int NoiseIntensity = Shader.PropertyToID("_NoiseIntensity");
        private static readonly int TimeProperty = Shader.PropertyToID("_ScanlineTime");

        private static readonly int VSyncLineWidth = Shader.PropertyToID("_VSyncLineWidth");
        private static readonly int VSyncLineSpeed = Shader.PropertyToID("_VSyncLineSpeed");
        private static readonly int VSyncDistortion = Shader.PropertyToID("_VSyncDistortion");
        private static readonly int VSyncFrequency = Shader.PropertyToID("_VSyncFrequency");

        private Material _crtMaterial;
        private RenderTargetIdentifier _source;
        private RenderTargetHandle _tempRenderTarget;
        private CrtSettings _settings;
        private readonly string _profilerTag;

        public CrtRenderPass(string tag)
        {
            _profilerTag = tag;
            _tempRenderTarget.Init("_TemporaryColorTexture");
        }

        public void Setup(RenderTargetIdentifier source, Material material, CrtSettings settings)
        {
            _source = source;
            _crtMaterial = material;
            _settings = settings;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!_crtMaterial)
            {
                Debug.LogError("CRT Material is not assigned!");
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get(_profilerTag);

            // apply material properties
            _crtMaterial.SetFloat(Curvature, _settings.Curvature);
            _crtMaterial.SetFloat(PaniniDistance, _settings.PaniniDistance);
            _crtMaterial.SetFloat(PaniniCropToFit, _settings.PaniniCropToFit);

            _crtMaterial.SetFloat(ScanlineIntensity, _settings.ScanlineIntensity);
            _crtMaterial.SetFloat(ScanlineCount, _settings.ScanlineCount);
            _crtMaterial.SetFloat(ScanlineSpeed, _settings.ScanlineSpeed);

            _crtMaterial.SetFloat(RGBShift, _settings.RgbShift);
            _crtMaterial.SetFloat(Brightness, _settings.Brightness);
            _crtMaterial.SetFloat(Contrast, _settings.Contrast);
            _crtMaterial.SetFloat(Flicker, _settings.Flicker);

            _crtMaterial.SetFloat(VignetteIntensity, _settings.VignetteIntensity);
            _crtMaterial.SetFloat(NoiseIntensity, _settings.NoiseIntensity);
            _crtMaterial.SetFloat(TimeProperty, Time.time);

            _crtMaterial.SetFloat(VSyncLineWidth, _settings.VSyncLineWidth);
            _crtMaterial.SetFloat(VSyncLineSpeed, _settings.VSyncLineSpeed);
            _crtMaterial.SetFloat(VSyncDistortion, _settings.VSyncDistortion);
            _crtMaterial.SetFloat(VSyncFrequency, _settings.VSyncFrequency);

            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            cmd.GetTemporaryRT(_tempRenderTarget.id, descriptor);

            // blit to temp RT with CRT shader
            Blit(cmd, _source, _tempRenderTarget.Identifier(), _crtMaterial);
            Blit(cmd, _tempRenderTarget.Identifier(), _source);
            
            // execute commands and clean up
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_tempRenderTarget.id);
        }
    }
}