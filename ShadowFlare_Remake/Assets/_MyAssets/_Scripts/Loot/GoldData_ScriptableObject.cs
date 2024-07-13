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
        [field: SerializeField] public int GoldAmount { get; private set; }

        private Dictionary<string, int> _statsDict = new();

        public GoldData_ScriptableObject(int goldAmount)
        {
            GoldAmount = goldAmount;

            if(GoldAmount < 0)
            {
                GoldAmount = 0;
            }
            else if(GoldAmount > MaxGoldAmount)
            {
                GoldAmount = MaxGoldAmount;
            }
        }

        public Dictionary<string, int> GetStatsDict()
        {
            if(_statsDict.Count <= 0)
                InitStatsDict();

            return _statsDict;
        }

        private void InitStatsDict()
        {
            _statsDict.Add("Amount", GoldAmount);
        }
    }
}
