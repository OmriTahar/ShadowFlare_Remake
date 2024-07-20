using UnityEngine;

namespace ShadowFlareRemake.Loot
{
    [CreateAssetMenu(fileName = "NewPotion", menuName = "Scriptable Objects/Create New Loot/Create New Gold")]
    public class GoldData_ScriptableObject : LootData_ScriptableObject
    {
        [Space(15)]
        [SerializeField] private string _________IMAGES_________ = SpaceLine;
        [field: SerializeField] public Sprite GoldImage_1 { get; private set; }
        [field: SerializeField] public Sprite GoldImage_2_to_99 { get; private set; }
        [field: SerializeField] public Sprite GoldImage_100_to_499 { get; private set; }
        [field: SerializeField] public Sprite GoldImage_500_to_999 { get; private set; }
        [field: SerializeField] public Sprite GoldImage_1000_to_9999 { get; private set; }
        [field: SerializeField] public Sprite GoldImage_10000 { get; private set; }
    }
}
