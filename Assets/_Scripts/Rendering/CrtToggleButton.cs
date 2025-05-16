// Made by Niek Melet on 16/5/2025

using UnityEngine;
using UnityEngine.UI;

namespace FistFury.Rendering
{
    public class CrtToggleButton : MonoBehaviour
    {
        private Toggle _toggle;

        private void Start()
        {
            _toggle = GetComponent<Toggle>();
            if (_toggle)
            {
                if (CrtEffectManager.Singleton)
                    _toggle.isOn = CrtEffectManager.Singleton.IsCrtEnabled();

                _toggle.onValueChanged.AddListener(OnToggleValueChanged);
            }
            else
                Debug.LogError("No Toggle component found on this object");
        }

        private void OnToggleValueChanged(bool isOn)
        {
            if (CrtEffectManager.Singleton)
                CrtEffectManager.Singleton.SetCrtEnabled(isOn);
        }
    }
}
