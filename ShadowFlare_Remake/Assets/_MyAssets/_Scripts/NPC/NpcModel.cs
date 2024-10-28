namespace ShadowFlareRemake.NPC
{
    public class NpcModel : Model
    {
        public string Name { get; private set; }
        public bool IsTalking { get; private set; } = false;

        public NpcModel(string name)
        {
            Name = name;
        }

        public void SetIsTalking(bool isTalking)
        {
            IsTalking = isTalking;
            Changed();
        }
    }
}
