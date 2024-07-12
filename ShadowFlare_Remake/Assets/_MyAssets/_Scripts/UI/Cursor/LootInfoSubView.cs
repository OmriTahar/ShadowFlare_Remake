using ShadowFlareRemake.Loot;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ShadowFlareRemake.UI.Cursor
{
    public class LootInfoSubView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _panel;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private List<LootInfoLine> _lootInfoLines;

        public void SetIsActive(bool isActive)
        {
            _panel.SetActive(isActive);
        }

        public void SetData(LootModel lootModel)
        {
            TurnOffLines();
            _nameText.text = lootModel.LootData.Name;

            if(lootModel.LootCategory == Enums.LootCategory.Equipment)
            {
                var data = lootModel.LootData as EquipmentData_ScriptableObject;
                HandleSetInfoLines(data.GetStatsDict());
            }

            else if(lootModel.LootCategory == Enums.LootCategory.Potion)
            {
                var data = lootModel.LootData as PotionData_ScriptableObject;
                HandleSetInfoLines(data.GetStatsDict());
            }
        }

        private void HandleSetInfoLines(Dictionary<string, int> statsDict)
        {
            var index = 0;

            foreach(var pair in statsDict)
            {
                if(pair.Value <= 0)
                    continue;

                var infoLine = _lootInfoLines[index];
                SetInfoLine(infoLine, pair.Key, pair.Value.ToString());
                _lootInfoLines[index].gameObject.SetActive(true);
                index++;
            }
        }

        private void SetInfoLine(LootInfoLine infoLine, string header, string text)
        {
            infoLine.Header.text = header;
            infoLine.Text.text = text;
        }

        private void TurnOffLines()
        {
            foreach (var line in _lootInfoLines)
            {
                line.gameObject.SetActive(false);
            }
        }
    }
}
