namespace ShadowFlareRemake.Behaviours
{
    public class EntityNameData
    {
        public EntityType EntityType { get; private set; }
        public string Name { get; private set; }
        public int CurrentHP { get; private set; }
        public int MaxHP { get; private set; }

        public EntityNameData(EntityType entityType, string name,int currentHP, int maxHP)
        {
            EntityType = entityType;
            Name = name;
            CurrentHP = currentHP;
            MaxHP = maxHP;
        }
    }
}
