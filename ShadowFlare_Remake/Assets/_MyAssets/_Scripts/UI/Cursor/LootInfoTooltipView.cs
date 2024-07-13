using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Loot;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ShadowFlareRemake.UI.Cursor
{
    public class LootInfoTooltipView : View<LootInfoTooltipModel>
    {
        [Header("References")]
        [SerializeField] private GameObject _panel;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private List<LootInfoTooltipLine> _lootInfoLines;

        protected override void ModelChanged()
        {
            SetData();
            SetIsActive();
        }

        private void SetIsActive()
        {
            _panel.SetActive(Model.IsActive);
        }

        public void SetData()
        {
            TurnOffLines();

            if(Model.LootModel == null)
                return;

            var lootModel = Model.LootModel;
            _nameText.text = lootModel.LootData.Name;

            if(lootModel.LootCategory == LootCategory.Equipment)
            {
                var data = lootModel.LootData as EquipmentData_ScriptableObject;
                HandleSetInfoLines(data.GetStatsDict());
            }
            else if(lootModel.LootCategory == LootCategory.Potion)
            {
                var data = lootModel.LootData as PotionData_ScriptableObject;
                HandleSetInfoLines(data.GetStatsDict());
            }
            else if(lootModel.LootCategory == LootCategory.Gold)
            {
                var data = lootModel.LootData as GoldData_ScriptableObject;
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

        private void SetInfoLine(LootInfoTooltipLine infoLine, string header, string text)
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
