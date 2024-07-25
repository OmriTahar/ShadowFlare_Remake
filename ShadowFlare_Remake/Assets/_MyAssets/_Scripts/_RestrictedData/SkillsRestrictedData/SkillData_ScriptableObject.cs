using ShadowFlareRemake.Skills;
using UnityEngine;

namespace ShadowFlareRemake.SkillsRestrictedData
{
    [CreateAssetMenu(fileName = "NewSkill", menuName = "Scriptable Objects/Create New Skill")]
    public class SkillData_ScriptableObject : ScriptableObject, ISkillData
    {
        [Space(15)]
        [SerializeField] private string ______DATA_____ = _spaceLine;
        [field: SerializeField] public SkillType SkillType { get; private set; }
        [field: SerializeField] public SkillDamageType DamageType { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public int MpCost { get; private set; }
        [field: SerializeField] public int AddedDamage { get; private set; }

        private const string _spaceLine = "------------------------------------";
    }
}
