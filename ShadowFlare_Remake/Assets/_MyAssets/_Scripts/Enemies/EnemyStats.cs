using System;
using UnityEngine;

namespace ShadowFlareRemake.Enemies {

    [CreateAssetMenu(fileName = "NewEnemyStats", menuName = "ScriptableObjects/Enemy Stats")]
    public class EnemyStats : ScriptableObject , IUnitStats {

        [field: SerializeField] public Color color { get; private set; }
        [field: SerializeField] public int ExpDrop { get; private set; }
        [field: SerializeField] public int CoinsDrop { get; private set; }

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int Level { get; private set; }

        [field: SerializeField] public int MaxHP { get; private set; }
        [field: SerializeField] public int Strength { get; private set; }
        [field: SerializeField] public int Attack { get; private set; }
        [field: SerializeField] public int Defence { get; private set; }
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
