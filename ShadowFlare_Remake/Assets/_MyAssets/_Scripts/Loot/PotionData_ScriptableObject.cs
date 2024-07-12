using System.Collections.Generic;
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

        [Space(15)]
        [SerializeField] private string ______INFO_____ = SpaceLine;
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
            _statsDict.Add("HP Restore", HpRestoreAmount);
            _statsDict.Add("MP Restore", MpRestoreAmount);
            _statsDict.Add("Sell Price", SellPrice);
        }
    }
}
