using ShadowFlareRemake.UI.Items;
using UnityEngine;

namespace ShadowFlareRemake.UI.Inventory
{
    public class Item : MonoBehaviour
    {
        public string Name { get; private set; }
        public Texture2D Icon { get; private set; }
        public int Width {  get; private set; } 
        public int Height { get; private set; }

        [Header("References")]
        [SerializeField] private Item_ScriptableObject _itemData;

        private void Awake()
        {
            SetData();
        }

        private void SetData()
        {
            Name = _itemData.Name;
            Icon = _itemData.Icon;
            Width = _itemData.Width;
            Height = _itemData.Height;
        }
    }
}


