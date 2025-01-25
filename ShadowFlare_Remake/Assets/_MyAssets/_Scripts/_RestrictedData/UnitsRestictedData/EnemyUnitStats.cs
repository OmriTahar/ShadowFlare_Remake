using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.SkillsRestrictedData;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.UnitsRestrictedData
{
    [CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Scriptable Objects/Create New Enemy Stats")]
    public class EnemyUnitStats : ScriptableObject, IEnemyUnitStats
    {
        private const string _spaceLine = "------------------------------------";

        [Space(15)]
        [SerializeField] private string ______BASE_____ = _spaceLine;
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int Level { get; private set; }

        [Space(15)]
        [SerializeField] private string ______ENEMY_____ = _spaceLine;
        [field: SerializeField] public Color Color { get; private set; }
        [field: SerializeField] public float ScaleMultiplier { get; private set; } = 1f;
        [field: SerializeField] public int EvolutionLevel { get; private set; }

        [Space(15)]
        [SerializeField] private string ______REWARDS_____ = _spaceLine;
        [field: SerializeField] public int ExpReward { get; private set; }
        [field: SerializeField] public int LootDropChance { get; private set; }

        [Space(15)]
        [SerializeField] private string ______PHYSICAL_____ = _spaceLine;
        [field: SerializeField] public int MaxHP { get; private set; }
        [field: SerializeField] public int Attack { get; private set; }
        [field: SerializeField] public int Defense { get; private set; }
        [field: SerializeField] public int HitRate { get; private set; }
        [field: SerializeField] public int EvasionRate { get; private set; }
        [field: SerializeField] public int MovementSpeed { get; private set; }
        [field: SerializeField] public int AttackSpeed { get; private set; }
        [field: SerializeField] public float AttackDistance { get; private set; }
        [field: SerializeField] public int ChaseDistance { get; private set; }

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
    }
}
