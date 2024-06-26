using ShadowFlareRemake;
using ShadowFlareRemake.Enums;

namespace ShadowFlare_Remake.VFX
{
    public class VisualEffectsModel : Model
    {
        public VfxType SelectedEffectType { get; private set; }
        public bool IsPlayingEffect { get; private set; }

        public VisualEffectsModel() { }
     
        public void SetIsPlayingEffect(VfxType effectType, bool isPlayingEffect)
        {
            SelectedEffectType = effectType;
            IsPlayingEffect = isPlayingEffect; 
            Changed();
        }
    }
}
