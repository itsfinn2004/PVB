// Made by Niek Melet on 15/5/2025

using FistFury.Rendering;

namespace UnityEngine.Rendering.Universal
{
    public class CrtRenderFeature : ScriptableRendererFeature
    {
        public CrtSettings Settings = new CrtSettings();
        private CrtRenderPass _crtRenderPass;

        public override void Create()
        {
            _crtRenderPass = new CrtRenderPass("CRT Filter");
            _crtRenderPass.renderPassEvent = Settings.RenderPassEvent;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (!Settings.CRTMaterial)
            {
                Debug.LogWarning("CRT Material is not assigned. Create a material with the CRT Filter Shader and assign it.");
                return;
            }

            _crtRenderPass.Setup(renderer.cameraColorTarget, Settings.CRTMaterial, Settings);
            renderer.EnqueuePass(_crtRenderPass);
        }
    }
}
