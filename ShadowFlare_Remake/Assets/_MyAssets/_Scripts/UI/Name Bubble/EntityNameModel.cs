namespace ShadowFlareRemake.UI.NameBubble
{
    public class EntityNameModel : Model
    {
        public bool IsActive { get; private set; }
        public EntityType EntityType { get; private set; }
        public string Name { get; private set; }
        public int CurrentHP { get; private set; }
        public int MaxHP { get; private set; }

        public EntityNameModel()
        {
            EntityType = EntityType.None;
            Name = "";
            CurrentHP = 0;
            MaxHP = 0;
        }

        public void SetNameBubbleData(EntityType entityType, string name, int currentHP, int maxHP)
        {
            EntityType = entityType;
            Name = name;
            CurrentHP = currentHP;
            MaxHP = maxHP;
            Changed();
        }

        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
            Changed();
        }
    }
}
