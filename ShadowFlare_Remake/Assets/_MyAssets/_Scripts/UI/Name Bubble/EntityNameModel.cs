using UnityEngine;

namespace ShadowFlareRemake.UI.NameBubble
{
    public class EntityNameModel : Model
    {
        public bool IsActive { get; private set; }
        public Transform CurrentEntityTransform { get; private set; }

        public EntityType EntityType { get; private set; }
        public string Name { get; private set; }
        public int CurrentHP { get; private set; }
        public int MaxHP { get; private set; }
        public int EvolutionLevel { get; private set; }
        public int UiOffest { get; private set; }
        public float ScaleMultiplier { get; private set; }

        public EntityNameModel()
        {
            EntityType = EntityType.None;
            Name = "";
            CurrentHP = 0;
            MaxHP = 0;
        }

        public void SetNameBubbleData(EntityType entityType, string name, int currentHP, int maxHP, int evolutionLevel,
                                      int uiOffset, float scaleMultiplier ,Transform entityTransform)
        {
            EntityType = entityType;
            Name = name;
            CurrentHP = currentHP;
            MaxHP = maxHP;
            EvolutionLevel = evolutionLevel;
            UiOffest = uiOffset;
            ScaleMultiplier = scaleMultiplier;
            CurrentEntityTransform = entityTransform;
            Changed();
        }

        public void SetEntityTransform(Transform entityTransform)
        {
            CurrentEntityTransform = entityTransform;
            Changed();
        }

        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
            Changed();
        }
    }
}
