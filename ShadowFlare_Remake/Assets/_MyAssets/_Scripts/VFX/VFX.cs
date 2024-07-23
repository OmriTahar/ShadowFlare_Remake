using System;
using UnityEngine;

namespace ShadowFlareRemake.VFX
{
    [Serializable]
    public class VFX
    {
        [field: SerializeField] public VfxType EffectType { get; private set; }
        [field: SerializeField] public ParticleSystem Effect { get; private set; }
    }
}
