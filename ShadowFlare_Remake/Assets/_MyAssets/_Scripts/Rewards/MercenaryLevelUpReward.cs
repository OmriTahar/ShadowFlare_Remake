using UnityEngine;

namespace ShadowFlareRemake.Rewards {

    [CreateAssetMenu(fileName = "NewMercenaryLevelUpReward", menuName = "Scriptable Objects/Create New Mercenary Level Up Reward")]
    public class MercenaryLevelUpReward : ScriptableObject, ILevelUpReward {

        [field: SerializeField] public int MaxHP { get; private set; }
        [field: SerializeField] public int MaxMP { get; private set; }
        [field: SerializeField] public int Strength { get; private set; }
        [field: SerializeField] public int Attack { get; private set; }
        [field: SerializeField] public int Defense { get; private set; }
        [field: SerializeField] public int MagicalAttack { get; private set; }
        [field: SerializeField] public int MagicalDefence { get; private set; }
    }
}
