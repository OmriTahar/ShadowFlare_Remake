using TMPro;
using UnityEngine;

namespace ShadowFlareRemake.Loot
{
    public class LootView : View<LootModel>
    {
        public string Name { get; private set; }
        public Texture2D Icon { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        [Header("Refernces")]
        [SerializeField] private TMP_Text _nameText;

        protected override void ModelChanged()
        {
            SetData();
            SetText();
        }

        private void SetData()
        {
            if(Model.LootData == null)
            {
                print("Loot Data is null!");
                return;
            }

            Name = Model.LootData.Name;
            Icon = Model.LootData.Icon;
            Width = Model.LootData.Width;
            Height = Model.LootData.Height;
        }

        private void SetText()
        {
            if(!string.IsNullOrEmpty(Name))
            {
                _nameText.text = Name;
            }
        }
    }
}


