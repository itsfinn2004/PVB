// Made by Niek Melet on 16/5/2025

using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FistFury.Rendering
{
    public class CrtEffectManager : MonoBehaviour
    {
        public static CrtEffectManager Singleton { get; private set; }

        [Tooltip("Reference to the Universal Render Pipeline Asset")]
        [SerializeField] private UniversalRenderPipelineAsset renderPipelineAsset;

        private CrtRenderFeature _crtFeature;
        private ScriptableRendererData _rendererData;

        private void Awake()
        {
            if (Singleton && Singleton != this)
            {
                Destroy(gameObject);
                return;
            }

            Singleton = this;
            DontDestroyOnLoad(gameObject);

            FindCrtFeature();
        }

        private void FindCrtFeature()
        {
            if (!renderPipelineAsset)
            {
                renderPipelineAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
                if (!renderPipelineAsset)
                {
                    Debug.LogError("Could not find Universal Render Pipeline Asset!");
                    return;
                }
            }

            // get the renderer data through reflection (needed because rendererData is not directly accessible)
            FieldInfo field = renderPipelineAsset.GetType().GetField("m_RendererDataList",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (field == null)
            {
                Debug.LogError("Could not find renderer data list in URP asset!");
                return;
            }

            // We'll use the first renderer data by default (typically the Forward Renderer)
            if (field.GetValue(renderPipelineAsset) is not ScriptableRendererData[] { Length: > 0 } rendererDataList) return;

            _rendererData = rendererDataList[0];

            // Find the CRT feature among the renderer features
            foreach (ScriptableRendererFeature feature in _rendererData.rendererFeatures)
            {
                if (feature is not CrtRenderFeature crtFeature) continue;

                _crtFeature = crtFeature;
                return;
            }

            Debug.LogWarning("CRT Render Feature not found in the renderer data!");
        }

        /// <summary>
        /// Enable or disable the CRT effect
        /// </summary>
        /// <param name="isEnabled">Whether the effect should be enabled</param>
        public void SetCrtEnabled(bool isEnabled)
        {
            if (!_crtFeature) return;

            // Set the effect state
            _crtFeature.Settings.IsEnabled = isEnabled;

            // Important: This forces URP to rebuild the render passes
            if (_rendererData)
            {
                _rendererData.SetDirty();
            }
        }

        /// <summary>
        /// Toggle the CRT effect on/off
        /// </summary>
        public void ToggleCrtEffect()
        {
            if (_crtFeature)
            {
                SetCrtEnabled(!_crtFeature.Settings.IsEnabled);
            }
        }

        /// <summary>
        /// Check if the CRT effect is currently enabled
        /// </summary>
        /// <returns>True if the effect is enabled, false otherwise</returns>
        public bool IsCrtEnabled()
        {
            return _crtFeature && _crtFeature.Settings.IsEnabled;
        }
    }
}
