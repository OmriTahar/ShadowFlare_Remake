using TMPro;
using UnityEngine;

namespace ShadowFlareRemake.Loot
{
    public class LootView : MonoBehaviour
    {
        public string Name { get; private set; }
        public Texture2D Icon { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        [Header("References")]
        [SerializeField] private Loot_ScriptableObject _lootData;
        [SerializeField] private TMP_Text _nameText;

        private void Awake()
        {
            SetData();
            SetText();
        }

        private void SetData()
        {
            if(_lootData == null)
            {
                print("Loot Data is null!");
                return;
            }

            Name = _lootData.Name;
            Icon = _lootData.Icon;
            Width = _lootData.Width;
            Height = _lootData.Height;
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


