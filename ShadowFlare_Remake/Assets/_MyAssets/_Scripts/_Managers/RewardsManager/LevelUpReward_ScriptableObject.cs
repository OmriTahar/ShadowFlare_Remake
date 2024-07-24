using ShadowFlareRemake.Rewards;
using UnityEngine;

namespace ShadowFlareRemake.Managers.Rewards {

    [CreateAssetMenu(fileName = "NewLevelUpReward", menuName = "Scriptable Objects/Create New Level Up Reward")]
    public class LevelUpReward_ScriptableObject : ScriptableObject, ILevelUpReward {

        [Space(15)]
        [SerializeField] private string ______PHYSICAL_____ = _spaceLine;
        [field: SerializeField] public int Strength { get; private set; }
        [field: SerializeField] public int MaxHP { get; private set; }
        [field: SerializeField] public int Attack { get; private set; }
        [field: SerializeField] public int Defense { get; private set; }
        [field: SerializeField] public int HitRate { get; private set; }
        [field: SerializeField] public int EvasionRate { get; private set; }
        [field: SerializeField] public int MovementSpeed { get; private set; } 
        [field: SerializeField] public int AttackSpeed { get; private set; } 

        [Space(15)]
        [SerializeField] private string ______MAGICAL_____ = _spaceLine;
        [field: SerializeField] public int MaxMP { get; private set; }
        [field: SerializeField] public int MagicalAttack { get; private set; }
        [field: SerializeField] public int MagicalDefense { get; private set; }
        [field: SerializeField] public int MagicalHitRate { get; private set; }
        [field: SerializeField] public int MagicalEvasionRate { get; private set; }
        [field: SerializeField] public int MagicalAttackSpeed { get; private set; }

        private const string _spaceLine = "------------------------------------";
    }
}
