using UnityEngine;

namespace ShadowFlareRemake.Loot
{
    public class LootData_ScriptableObject : ScriptableObject
    {
        protected const string SpaceLine = "------------------------------------";

        [Space(15)]
        [SerializeField] private string _________BASE_________ = SpaceLine;
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public LootType LootType { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }

        [Space(15)]
        [SerializeField] private string _________SIZE_________ = SpaceLine;
        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Height { get; private set; }
    }
}
