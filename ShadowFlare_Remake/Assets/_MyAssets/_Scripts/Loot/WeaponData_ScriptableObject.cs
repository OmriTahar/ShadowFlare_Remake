using UnityEngine;

namespace ShadowFlareRemake.Loot
{
    [CreateAssetMenu(fileName = "NewPotion", menuName = "Scriptable Objects/Create New Loot/Create New Weapon")]
    public class WeaponData_ScriptableObject : LootData_ScriptableObject
    {
        [Space(15)]
        [SerializeField] private string _________ATTRIBUTES_________ = SpaceLine;
        [field: SerializeField] public int Attack { get; private set; }
    }
}
