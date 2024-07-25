using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.Player;
using ShadowFlareRemake.Rewards;
using ShadowFlareRemake.RewardsRestrictedData;
using UnityEngine;

namespace ShadowFlareRemake.RewardsManagement
{
    public class RewardsManager : MonoBehaviour {

        [Header("Rewards")]
        [SerializeField] private LevelUpReward_ScriptableObject _mercenaryLevelUpReward;

        public ExpReward GetExpReward(IPlayerUnitStats playerUnit, IEnemyUnitStats enemyUnit) { // Todo: Do this better

            var expDrop = enemyUnit.ExpReward;
            var newCurrentExp = 0;
            var newExpToLevelUp = 0;
            var newLevel = 0;
            var isPendingLevelUp = playerUnit.CurrentExp + expDrop >= playerUnit.ExpToLevelUp;

            if(isPendingLevelUp) {

                var totalExp = playerUnit.CurrentExp + expDrop;

                newCurrentExp = totalExp - playerUnit.ExpToLevelUp;
                newExpToLevelUp = playerUnit.ExpToLevelUp * 2;
                newLevel = playerUnit.Level + 1;

            } else {

                newCurrentExp = playerUnit.CurrentExp + expDrop;
                newExpToLevelUp = playerUnit.ExpToLevelUp;
                newLevel = playerUnit.Level;
            }

            return new ExpReward(newCurrentExp, newExpToLevelUp, newLevel, isPendingLevelUp);
        }

        public ILevelUpReward GetLevelUpReward(IPlayerUnitStats playerUnit) {  // Todo: Do this better

            if(playerUnit.Level < 5) {
                return _mercenaryLevelUpReward;
            }
            return null;
        }
    }
}
