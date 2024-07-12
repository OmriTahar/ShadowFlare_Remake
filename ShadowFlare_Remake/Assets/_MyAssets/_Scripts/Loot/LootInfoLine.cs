using TMPro;
using UnityEngine;

namespace ShadowFlareRemake.Loot
{
    public class LootInfoLine : MonoBehaviour
    {
        [field: SerializeField] public TMP_Text Header { get; private set; }
        [field: SerializeField] public TMP_Text Text { get; private set; }
    }
}
