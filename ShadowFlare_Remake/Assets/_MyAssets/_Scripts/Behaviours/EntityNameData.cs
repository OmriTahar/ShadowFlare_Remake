namespace ShadowFlareRemake.Behaviours
{
    public class EntityNameData
    {
        public EntityType EntityType { get; private set; }
        public string Name { get; private set; }
        public int CurrentHP { get; private set; }
        public int MaxHP { get; private set; }
        public int EvolutionLevel { get; private set; }
        public int UiOffest { get; private set; }
        public float ScaleMultiplier { get; private set; }

        public EntityNameData(EntityType entityType, string name,int currentHP, int maxHP, int evolutionLevel,
                              int uiOffest, float scaleMultiplier)
        {
            EntityType = entityType;
            Name = name;
            CurrentHP = currentHP;
            MaxHP = maxHP;
            EvolutionLevel = evolutionLevel;
            UiOffest = uiOffest;
            ScaleMultiplier = scaleMultiplier;
        }
    }
}
