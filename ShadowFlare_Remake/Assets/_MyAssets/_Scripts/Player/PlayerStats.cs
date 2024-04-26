using ShadowFlareRemake.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.Player
{
    public class PlayerStats : IUnitStats
    {

        [field: SerializeField] public Vocation Vcocation { get; private set; }
        [field: SerializeField] public int CurrentExp { get; private set; }
        [field: SerializeField] public int ExpToLevelUp { get; private set; }

        public string Name => throw new System.NotImplementedException();

        public int Level => throw new System.NotImplementedException();

        public int MaxHP => throw new System.NotImplementedException();

        public int Strength => throw new System.NotImplementedException();

        public int Attack => throw new System.NotImplementedException();

        public int Defence => throw new System.NotImplementedException();

        public int HitRate => throw new System.NotImplementedException();

        public int EvasionRate => throw new System.NotImplementedException();

        public int WalkingSpeed => throw new System.NotImplementedException();

        public int AttackSpeed => throw new System.NotImplementedException();

        public int MaxMP => throw new System.NotImplementedException();

        public int MagicalAttack => throw new System.NotImplementedException();

        public int MagicalDefence => throw new System.NotImplementedException();

        public int MagicalHitRate => throw new System.NotImplementedException();

        public int MagicalEvasionRate => throw new System.NotImplementedException();

        public int MagicalAttackSpeed => throw new System.NotImplementedException();
    }
}
