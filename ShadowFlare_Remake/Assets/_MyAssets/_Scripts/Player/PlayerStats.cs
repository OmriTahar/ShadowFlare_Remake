using ShadowFlareRemake.Enums;
using System;
using UnityEngine;

namespace ShadowFlareRemake.Player {

    [Serializable]
    public class PlayerStats : IUnitStats {

        [field: SerializeField] public Vocation Vcocation { get; private set; }
        [field: SerializeField] public int CurrentExp { get; private set; }
        [field: SerializeField] public int ExpToLevelUp { get; private set; }

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int Level { get; private set; }

        [field: SerializeField] public int MaxHP { get; private set; }
        [field: SerializeField] public int Strength { get; private set; }
        [field: SerializeField] public int Attack { get; private set; }
        [field: SerializeField] public int Defense { get; private set; }
        [field: SerializeField] public int HitRate { get; private set; }
        [field: SerializeField] public int EvasionRate { get; private set; }
        [field: SerializeField] public int WalkingSpeed { get; private set; }
        [field: SerializeField] public int AttackSpeed { get; private set; }

        [field: SerializeField] public int MaxMP { get; private set; }
        [field: SerializeField] public int MagicalAttack { get; private set; }
        [field: SerializeField] public int MagicalDefence { get; private set; }
        [field: SerializeField] public int MagicalHitRate { get; private set; }
        [field: SerializeField] public int MagicalEvasionRate { get; private set; }
        [field: SerializeField] public int MagicalAttackSpeed { get; private set; }
    }
}
