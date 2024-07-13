using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.Loot
{
    [CreateAssetMenu(fileName = "NewPotion", menuName = "Scriptable Objects/Create New Loot/Create New Gold")]
    public class GoldData_ScriptableObject : LootData_ScriptableObject
    {
        public readonly int MaxGoldAmount = 10000;

        [Space(15)]
        [SerializeField] private string ______INFO_____ = SpaceLine;
        [field: SerializeField] public int Amount { get; private set; }

        private Dictionary<string, int> _statsDict = new();

        public Dictionary<string, int> GetStatsDict()
        {
            if(_statsDict.Count <= 0)
                InitStatsDict();

            return _statsDict;
        }

        private void InitStatsDict()
        {
            _statsDict.Add("Amount", Amount);
        }

        public int CombineGoldData(GoldData_ScriptableObject other)
        {
            int spareGold = 0;

            if(Amount + other.Amount <= MaxGoldAmount)
            {
                Amount += other.Amount;
            }
            else
            {
                var total = Amount + other.Amount;
                Amount = MaxGoldAmount;
                spareGold = total - Amount;
            }

            return spareGold;
        }
    }
}
