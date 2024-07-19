using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.Loot
{
    [CreateAssetMenu(fileName = "NewEquipment", menuName = "Scriptable Objects/Create New Loot/Create New Equipment")]
    public class EquipmentData_ScriptableObject : LootData_ScriptableObject
    {
        [Space(15)]
        [SerializeField] private string ______PHYSICAL_____ = SpaceLine;
        [field: SerializeField] public int MaxHP { get; private set; }
        [field: SerializeField] public int Attack { get; private set; }
        [field: SerializeField] public int Defense { get; private set; }
        [field: SerializeField] public int HitRate { get; private set; }
        [field: SerializeField] public int EvasionRate { get; private set; }
        [field: SerializeField] public int MovementSpeed { get; private set; }
        [field: SerializeField] public int AttackSpeed { get; private set; }
        [field: SerializeField] public int Strength { get; private set; }

        [Space(15)]
        [SerializeField] private string ______MAGICAL_____ = SpaceLine;
        [field: SerializeField] public int MaxMP { get; private set; }
        [field: SerializeField] public int MagicalAttack { get; private set; }
        [field: SerializeField] public int MagicalDefense { get; private set; }
        [field: SerializeField] public int MagicalHitRate { get; private set; }
        [field: SerializeField] public int MagicalEvasionRate { get; private set; }
        [field: SerializeField] public int MagicalAttackSpeed { get; private set; }

        [Space(15)]
        [SerializeField] private string ______INFO_____ = SpaceLine;
        [field: SerializeField] public int Weight { get; private set; }
        [field: SerializeField] public int Durability { get; private set; }
        [field: SerializeField] public int RequiredLevel { get; private set; }
        [field: SerializeField] public int SellPrice { get; private set; }


        private Dictionary<string, int> _statsDict = new();

        public Dictionary<string, int> GetStatsDict()
        {
            if(_statsDict.Count <= 0)
                InitStatsDict();

            return _statsDict;
        }

        private void InitStatsDict()
        {
            _statsDict.Add("Max HP", MaxHP);
            _statsDict.Add("Attack", Attack);
            _statsDict.Add("Defense", Defense);
            _statsDict.Add("HitRate", HitRate);
            _statsDict.Add("Evasion Rate", EvasionRate);
            _statsDict.Add("Movement Speed", MovementSpeed);
            _statsDict.Add("Attack Speed", AttackSpeed);
            _statsDict.Add("Strength", Strength);

            _statsDict.Add("Max MP", MaxMP);
            _statsDict.Add("Magical Attack", MagicalAttack);
            _statsDict.Add("Magical Defense", MagicalDefense);
            _statsDict.Add("M. Hit Rate", MagicalHitRate);
            _statsDict.Add("M. Evasion Rate", MagicalEvasionRate);
            _statsDict.Add("M. Attack Speed", MagicalAttackSpeed);

            _statsDict.Add("Weight", Weight);
            _statsDict.Add("Durability", Durability);
            _statsDict.Add("Required Level", RequiredLevel);
            _statsDict.Add("Sell Price", SellPrice);
        }
    }
}
