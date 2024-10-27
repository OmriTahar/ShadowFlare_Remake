using UnityEngine;
using TMPro;

namespace ShadowFlareRemake.NPC
{
    public class NpcView : View<NpcModel>
    {
        [Header("References")]
        [SerializeField] private TMP_Text _name;

        [Header("Temp")]
        [SerializeField] private string _namePlaceHolder;
      
        private void Start()
        {
            _name.text = _namePlaceHolder;
        }

        protected override void ModelChanged()
        {
            throw new System.NotImplementedException();
        }

    }
}
