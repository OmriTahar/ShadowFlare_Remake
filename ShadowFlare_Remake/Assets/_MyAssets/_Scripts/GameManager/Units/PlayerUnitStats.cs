using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Player;
using ShadowFlareRemake.Rewards;
using ShadowFlareRemake.UI.Stats;
using System;
using UnityEngine;

namespace ShadowFlareRemake.GameManager.Units
{
    [Serializable]
    public class PlayerUnitStats : IPlayerUnitStats
    {
        #region Stats Fields

        [Space(15)]
        [SerializeField] private string ______PLAYER_____ = _spaceLine;
        [field: SerializeField] public Vocation Vocation { get; private set; }
        [field: SerializeField] public int Strength { get; private set; }
        [field: SerializeField] public int CurrentExp { get; private set; }
        [field: SerializeField] public int ExpToLevelUp { get; private set; }

        [Space(15)]
        [SerializeField] private string ______BASE_____ = _spaceLine;
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int Level { get; private set; }

        [Space(15)]
        [SerializeField] private string ______PHYSICAL_____ = _spaceLine;
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
            Strength += reward.Strength;
            Attack += reward.Attack;
            Defense += reward.Defense;
            MagicalAttack += reward.MagicalAttack;
            MagicalDefense += reward.MagicalDefence;
        }

        public void RemoveEquippedGearAddedStats(IEquippedGearAddedStats addedStats)
        {
            MaxHP -= addedStats.MaxHP;
            Attack -= addedStats.Attack;
            Defense -= addedStats.Defense;
            HitRate -= addedStats.HitRate;
            EvasionRate -= addedStats.EvasionRate;
            MovementSpeed -= addedStats.MovementSpeed;
            AttackSpeed -= addedStats.AttackSpeed;
            Strength -= addedStats.Strength;
            MaxMP -= addedStats.MaxMP;
            MagicalAttack -= addedStats.MagicalAttack;
            MagicalDefense -= addedStats.MagicalDefense;
            MagicalHitRate -= addedStats.MagicalHitRate;
            MagicalEvasionRate -= addedStats.MagicalEvasionRate;
            MagicalAttackSpeed -= addedStats.MagicalAttackSpeed;
        }

        public void SetEquippedGearAddedStats(IEquippedGearAddedStats addedStats)
        {
            MaxHP += addedStats.MaxHP;
            Attack += addedStats.Attack;
            Defense += addedStats.Defense;
            HitRate += addedStats.HitRate;
            EvasionRate += addedStats.EvasionRate;
            MovementSpeed += addedStats.MovementSpeed;
            AttackSpeed += addedStats.AttackSpeed;
            Strength += addedStats.Strength;
            MaxMP += addedStats.MaxMP;
            MagicalAttack += addedStats.MagicalAttack;
            MagicalDefense += addedStats.MagicalDefense;
            MagicalHitRate += addedStats.MagicalHitRate;
            MagicalEvasionRate += addedStats.MagicalEvasionRate;
            MagicalAttackSpeed += addedStats.MagicalAttackSpeed;
        }
    }
}
