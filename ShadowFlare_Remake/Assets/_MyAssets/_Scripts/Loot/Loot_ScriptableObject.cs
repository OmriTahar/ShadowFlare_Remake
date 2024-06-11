using UnityEngine;
using ShadowFlareRemake.Enums;

namespace ShadowFlareRemake.Loot
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Scriptable Objects/Create New Item")]
    public class Loot_ScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public LootType Type { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Height { get; private set; }
    }
}
