using ShadowFlareRemake.Enums;
using System;
using UnityEngine;

namespace ShadowFlare_Remake.VFX
{
    [Serializable]
    public class VFX
    {
        [field: SerializeField] public VfxType EffectType { get; private set; }
        [field: SerializeField] public ParticleSystem Effect { get; private set; }
    }
}
