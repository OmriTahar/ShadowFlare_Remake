using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Player;
using ShadowFlareRemake.Rewards;
using System;
using UnityEngine;

namespace ShadowFlareRemake.GameManager.Units
{
    [Serializable]
    public class PlayerUnitStats : IPlayerUnitStats
    {
        #region Fields

        private const string _spaceLine = "------------------------------------";

        [Space(15)]
        [SerializeField] private string ______Enemy_____ = _spaceLine;
        [field: SerializeField] public Vocation Vcocation { get; private set; }
        [field: SerializeField] public int Strength { get; private set; }
        [field: SerializeField] public int CurrentExp { get; private set; }
        [field: SerializeField] public int ExpToLevelUp { get; private set; }

        [Space(15)]
        [SerializeField] private string ______Base_____ = _spaceLine;
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public int MovementSpeed { get; private set; }

        [Space(15)]
        [SerializeField] private string ______Physical_____ = _spaceLine;
        [field: SerializeField] public int MaxHP { get; private set; }
        [field: SerializeField] public int Attack { get; private set; }
        [field: SerializeField] public int Defense { get; private set; }
        [field: SerializeField] public int HitRate { get; private set; }
        [field: SerializeField] public int EvasionRate { get; private set; }
        [field: SerializeField] public int AttackSpeed { get; private set; }

        [Space(15)]
        [SerializeField] private string ______Magical_____ = _spaceLine;
        [field: SerializeField] public int MaxMP { get; private set; }
        [field: SerializeField] public int MagicalAttack { get; private set; }
        [field: SerializeField] public int MagicalDefence { get; private set; }
        [field: SerializeField] public int MagicalHitRate { get; private set; }
        [field: SerializeField] public int MagicalEvasionRate { get; private set; }
        [field: SerializeField] public int MagicalAttackSpeed { get; private set; }

        #endregion

        public void GiveExpReward(ExpReward reward)
        {
            CurrentExp = reward.NewCurrentExp;
            ExpToLevelUp = reward.NewExpToLevelUp;
            Level = reward.NewLevel;
        }

        public void GiveLevelUpReward(ILevelUpReward reward)
        {
            MaxHP += reward.HP;
            MaxMP += reward.MP;
            Strength += Strength += reward.Strength;
            Attack += reward.Attack;
            Defense += reward.Defense;
            MagicalAttack += reward.MagicalAttack;
            MagicalDefence += reward.MagicalDefence;
        }
    }
}