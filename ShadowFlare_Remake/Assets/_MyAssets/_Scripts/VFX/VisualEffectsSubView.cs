using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.VFX
{
    public class VisualEffectsSubView : MonoBehaviour
    {
        [Header("Effects")]
        [SerializeField] private List<VFX> _effectsList;

        private Dictionary<VfxType, ParticleSystem> _effectsDict = new();

        private void Awake()
        {
            InitEffectsDict();
        }

        public void SetIsPlayingEffect(VfxType effectType, bool isPlayingEffect)
        {
            if(effectType == VfxType.None)
                return;

            if(isPlayingEffect)
            {
                _effectsDict[effectType].Play();
                return;
            }

            _effectsDict[effectType].Stop();
        }

        private void InitEffectsDict()
        {
            foreach(var vfx in _effectsList)
            {
                _effectsDict.Add(vfx.EffectType, vfx.Effect);
            }
        }
    }
}
