using UnityEngine;

namespace ShadowFlareRemake.Loot
{
    [CreateAssetMenu(fileName = "NewPotion", menuName = "Scriptable Objects/Create New Loot/Create New Potion")]
    public class PotionData_ScriptableObject : LootData_ScriptableObject
    {
        [Space(15)]
        [SerializeField] private string _________ATTRIBUTES_________ = SpaceLine;
        [field: SerializeField] public int HpRestoreAmount { get; private set; }
        [field: SerializeField] public int MpRestoreAmount { get; private set; }
    }
}
