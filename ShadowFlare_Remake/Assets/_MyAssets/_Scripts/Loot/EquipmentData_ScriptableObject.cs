using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.Loot
{
    [CreateAssetMenu(fileName = "NewEquipment", menuName = "Scriptable Objects/Create New Loot/Create New Equipment")]
    public class EquipmentData_ScriptableObject : LootData_ScriptableObject
    {
        public Dictionary<string, int> StatsDict { get; private set; } = new();

        [Space(15)]
        [SerializeField] private string ______PHYSICAL_____ = _spaceLine;
        [field: SerializeField] public int MaxHP { get; private set; }
        [field: SerializeField] public int Attack { get; private set; }
        [field: SerializeField] public int Defense { get; private set; }
        [field: SerializeField] public int HitRate { get; private set; }
        [field: SerializeField] public int EvasionRate { get; private set; }
        [field: SerializeField] public int MovementSpeed { get; private set; }
        [field: SerializeField] public int AttackSpeed { get; private set; }
        [field: SerializeField] public int Strength { get; private set; }

        [Space(15)]
        [SerializeField] private string ______MAGICAL_____ = _spaceLine;
        [field: SerializeField] public int MaxMP { get; private set; }
        [field: SerializeField] public int MagicalAttack { get; private set; }
        [field: SerializeField] public int MagicalDefense { get; private set; }
        [field: SerializeField] public int MagicalHitRate { get; private set; }
        [field: SerializeField] public int MagicalEvasionRate { get; private set; }
        [field: SerializeField] public int MagicalAttackSpeed { get; private set; }

        private const string _spaceLine = "------------------------------------";

        private void Awake()
        {
            StatsDict.Add("Max HP", MaxHP);
            StatsDict.Add("Attack", Attack);
            StatsDict.Add("Defense", Defense);
            StatsDict.Add("HitRate", HitRate);
            StatsDict.Add("Evasion Rate", EvasionRate);
            StatsDict.Add("MovementSpeed", MovementSpeed);
            StatsDict.Add("Attack Speed", AttackSpeed);
            StatsDict.Add("Strength", Strength);

            StatsDict.Add("Max MP", MaxMP);
            StatsDict.Add("Magical Attack", MagicalAttack);
            StatsDict.Add("Magical Defense", MagicalDefense);
            StatsDict.Add("Magical Hit Rate", MagicalHitRate);
            StatsDict.Add("Magical Evasion Rate", MagicalEvasionRate);
            StatsDict.Add("Magical Attack Speed", MagicalAttackSpeed);
        }
    }
}
