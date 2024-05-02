using UnityEngine;
using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.Player;

namespace ShadowFlareRemake.Rewards {

    public class RewardsManager : MonoBehaviour {

        [Header("Rewards")]
        [SerializeField] private MercenaryLevelUpReward _mercenaryLevelUpReward;

        public ExpReward GetExpReward(IPlayerUnitStats playerUnit, IEnemyUnitStats enemyUnit) {

            return HandleExpRewardInternal(playerUnit, enemyUnit.ExpDrop);
        }

        private ExpReward HandleExpRewardInternal(IPlayerUnitStats playerUnit, int expDrop) { // Todo: Do this better

            var expReward = new ExpReward();

            if(expDrop <= 0) {
                return expReward;
            }

            var isPendingLevelUp = playerUnit.CurrentExp + expDrop >= playerUnit.ExpToLevelUp;

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

        private void HandleLevelUp() {


        }
    }
}
