using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.Player;
using ShadowFlareRemake.Rewards;
using UnityEngine;

namespace ShadowFlareRemake.Managers.GameManager {

    public class RewardsManager : MonoBehaviour {

        [Header("Rewards")]
        [SerializeField] private LevelUpReward_ScriptableObject _mercenaryLevelUpReward;

        public ExpReward GetExpReward(IPlayerUnitStats playerUnit, IEnemyUnitStats enemyUnit) { // Todo: Do this better

            var expDrop = enemyUnit.ExpReward;
            var expReward = new ExpReward();

            if(expDrop <= 0) {
                return expReward;
            }

            var isPendingLevelUp = playerUnit.CurrentExp + expDrop >= playerUnit.ExpToLevelUp;
            expReward.IsPendingLevelUp = isPendingLevelUp;

            if(isPendingLevelUp) {

                var totalExp = playerUnit.CurrentExp + expDrop;

                expReward.NewCurrentExp = totalExp - playerUnit.ExpToLevelUp;
                expReward.NewExpToLevelUp = playerUnit.ExpToLevelUp * 2;
                expReward.NewLevel = playerUnit.Level + 1;

            } else {

                expReward.NewCurrentExp = playerUnit.CurrentExp + expDrop;
                expReward.NewExpToLevelUp = playerUnit.ExpToLevelUp;
                expReward.NewLevel = playerUnit.Level;
            }

            return expReward;
        }

        public LevelUpReward_ScriptableObject GetLevelUpReward(IPlayerUnitStats playerUnit) {  // Todo: Do this better

            if(playerUnit.Level < 5) {
                return _mercenaryLevelUpReward;
            }
            return null;
        }
    }
}
