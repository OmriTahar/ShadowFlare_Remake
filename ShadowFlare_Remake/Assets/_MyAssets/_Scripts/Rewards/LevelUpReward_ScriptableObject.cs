using UnityEngine;

namespace ShadowFlareRemake.Rewards {

    [CreateAssetMenu(fileName = "NewLevelUpReward", menuName = "Scriptable Objects/Create New Level Up Reward")]
    public class LevelUpReward_ScriptableObject : ScriptableObject, ILevelUpReward {
         
        [field: SerializeField] public int HP { get; private set; }
        [field: SerializeField] public int MP { get; private set; }
        [field: SerializeField] public int Strength { get; private set; }
        [field: SerializeField] public int Attack { get; private set; }
        [field: SerializeField] public int Defense { get; private set; }
        [field: SerializeField] public int MagicalAttack { get; private set; }
        [field: SerializeField] public int MagicalDefence { get; private set; }
    }
}
