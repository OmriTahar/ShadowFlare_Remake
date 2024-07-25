using ShadowFlareRemake.Player;
using ShadowFlareRemake.Rewards;
using ShadowFlareRemake.SkillsRestrictedData;
using ShadowFlareRemake.UI.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.UnitsRestrictedData
{
    [CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Scriptable Objects/Create New Player Stats")]
    public class PlayerUnitStats : ScriptableObject, IPlayerUnitStats
    {
        #region Stats Fields

        [Space(15)]
        [SerializeField] private string ______PLAYER_____ = _spaceLine;
        [field: SerializeField] public Vocation Vocation { get; private set; }
        [field: SerializeField] public int Strength { get; private set; }
        [field: SerializeField] public int EquippedWeight { get; private set; }
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
        [field: SerializeField] public int MovementSpeed { get; private set; } = _defaultMovementSpeed;
        [field: SerializeField] public int AttackSpeed { get; private set; } = _defaultAttackSpeed;

        [Space(15)]
        [SerializeField] private string ______MAGICAL_____ = _spaceLine;
        [field: SerializeField] public int MaxMP { get; private set; }
        [field: SerializeField] public int MagicalAttack { get; private set; }
        [field: SerializeField] public int MagicalDefense { get; private set; }
        [field: SerializeField] public int MagicalHitRate { get; private set; }
        [field: SerializeField] public int MagicalEvasionRate { get; private set; }
        [field: SerializeField] public int MagicalAttackSpeed { get; private set; }

        [Space(15)]
        [SerializeField] private string ______SKILLS_____ = _spaceLine;
        [field: SerializeField] public List<SkillData_ScriptableObject> Skills { get; private set; }


        private const string _spaceLine = "------------------------------------";

        private static readonly int _defaultMovementSpeed = 100;
        private static readonly int _defaultAttackSpeed = 100;
        private static readonly int _defaultOverWeightSpeed = 60;

        #endregion

        public PlayerUnitStats(PlayerUnitStats statsToCopy)
        {
            Vocation = statsToCopy.Vocation;
            Strength = statsToCopy.Strength;
            EquippedWeight = statsToCopy.EquippedWeight;
            CurrentExp = statsToCopy.CurrentExp;
            ExpToLevelUp = statsToCopy.ExpToLevelUp;

            Name = statsToCopy.Name;
            Level = statsToCopy.Level;

            MaxHP = statsToCopy.MaxHP;
            Attack = statsToCopy.Attack;
            Defense = statsToCopy.Defense;
            HitRate = statsToCopy.HitRate;
            EvasionRate = statsToCopy.EvasionRate;
            MovementSpeed = statsToCopy.MovementSpeed;
            AttackSpeed = statsToCopy.AttackSpeed;

            MaxMP = statsToCopy.MaxMP;
            MagicalAttack = statsToCopy.MagicalAttack;
            MagicalDefense = statsToCopy.MagicalDefense;
            MagicalHitRate = statsToCopy.MagicalHitRate;
            MagicalEvasionRate = statsToCopy.MagicalEvasionRate;
            MagicalAttackSpeed = statsToCopy.MagicalAttackSpeed;
        }

        public void GiveExpReward(IExpReward reward)
        {
            CurrentExp = reward.NewCurrentExp;
            ExpToLevelUp = reward.NewExpToLevelUp;
            Level = reward.NewLevel;
        }

        public void GiveLevelUpReward(ILevelUpReward reward)
        {
            Strength += reward.Strength;

            MaxHP += reward.MaxHP;
            Attack += reward.Attack;
            Defense += reward.Defense;
            HitRate += reward.HitRate;
            EvasionRate += reward.EvasionRate;
            MovementSpeed += reward.MovementSpeed;
            AttackSpeed += reward.AttackSpeed;

            MaxMP += reward.MaxMP;
            MagicalAttack += reward.MagicalAttack;
            MagicalDefense += reward.MagicalDefense;
            MagicalHitRate += reward.MagicalHitRate;
            MagicalEvasionRate += reward.MagicalEvasionRate;
            MagicalAttackSpeed += reward.MagicalAttackSpeed;
        }

        public void RemoveEquippedGearAddedStats(IEquippedGearAddedStats addedStats)
        {
            Strength -= addedStats.Strength;
            EquippedWeight -= addedStats.EquippedWeight;

            MaxHP -= addedStats.MaxHP;
            Attack -= addedStats.Attack;
            Defense -= addedStats.Defense;
            HitRate -= addedStats.HitRate;
            EvasionRate -= addedStats.EvasionRate;
            MovementSpeed -= addedStats.MovementSpeed;
            AttackSpeed -= addedStats.AttackSpeed;

            MaxMP -= addedStats.MaxMP;
            MagicalAttack -= addedStats.MagicalAttack;
            MagicalDefense -= addedStats.MagicalDefense;
            MagicalHitRate -= addedStats.MagicalHitRate;
            MagicalEvasionRate -= addedStats.MagicalEvasionRate;
            MagicalAttackSpeed -= addedStats.MagicalAttackSpeed;
        }

        public void SetEquippedGearAddedStats(IEquippedGearAddedStats addedStats)
        {
            MovementSpeed = _defaultMovementSpeed;
            AttackSpeed = _defaultAttackSpeed;

            Strength += addedStats.Strength;
            EquippedWeight += addedStats.EquippedWeight;

            MaxHP += addedStats.MaxHP;
            Attack += addedStats.Attack;
            Defense += addedStats.Defense;
            HitRate += addedStats.HitRate;
            EvasionRate += addedStats.EvasionRate;
            MovementSpeed += addedStats.MovementSpeed;
            AttackSpeed += addedStats.AttackSpeed;

            MaxMP += addedStats.MaxMP;
            MagicalAttack += addedStats.MagicalAttack;
            MagicalDefense += addedStats.MagicalDefense;
            MagicalHitRate += addedStats.MagicalHitRate;
            MagicalEvasionRate += addedStats.MagicalEvasionRate;
            MagicalAttackSpeed += addedStats.MagicalAttackSpeed;

            if(EquippedWeight > Strength)
            {
                MovementSpeed = _defaultOverWeightSpeed;
                AttackSpeed = _defaultOverWeightSpeed;
            }
        }
    }
}
