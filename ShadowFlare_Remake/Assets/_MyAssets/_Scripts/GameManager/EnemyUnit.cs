using UnityEngine;
using ShadowFlareRemake.Enemies;

namespace ShadowFlareRemake.GameManager {

    [CreateAssetMenu(fileName = "NewEnemyStats", menuName = "ScriptableObjects/Enemy Stats")]
    public class EnemyUnit : ScriptableObject , IEnemyUnit {

        private const string _spaceLine = "------------------------------------";

        [Space(15)]
        [SerializeField] private string ______Enemy_____ = _spaceLine;
        [field: SerializeField] public Color Color { get; private set; }
        [field: SerializeField] public int ExpDrop { get; private set; }
        [field: SerializeField] public int CoinsDrop { get; private set; }

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
    }
}
