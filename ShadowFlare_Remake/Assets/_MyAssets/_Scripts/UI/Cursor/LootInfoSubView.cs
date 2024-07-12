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
                HandleEquipmentData(data);
            }

            else if(lootModel.LootCategory == Enums.LootCategory.Potion)
            {
                var data = lootModel.LootData as PotionData_ScriptableObject;
                HandlePotionsData(data);
            }
        }

        private void HandleEquipmentData(EquipmentData_ScriptableObject data)
        {
            var index = 0;
            var statsDict = data.GetStatsDict();

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

        private void HandlePotionsData(PotionData_ScriptableObject data)
        {

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
