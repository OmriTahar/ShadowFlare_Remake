using UnityEngine;
using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.Player;

namespace ShadowFlareRemake.Rewards {

    public class RewardsManager : MonoBehaviour {

        public void GiveRewardsToPlayer(IPlayerUnit playerUnit, IEnemyUnit enemyUnit) {

            HandleExpReward(playerUnit, enemyUnit.ExpDrop);
        }

        private void HandleExpReward(IPlayerUnit playerUnit, int expDrop) {

            if(expDrop <= 0) {
                return;
            }

            var isPendingLevelUp = playerUnit.CurrentExp + expDrop >= playerUnit.ExpToLevelUp;

            if(isPendingLevelUp) {

                var totalExp = playerUnit.CurrentExp + expDrop;
                var newCurrentExp = totalExp - playerUnit.ExpToLevelUp;
                var expToLevelUp = playerUnit.ExpToLevelUp * 2;           // Todo: Do this better
                var newLevel = playerUnit.Level + 1;                      // Todo: Set max level

                //playerUnit.SetExp(newCurrentExp, expToLevelUp);
                //playerUnit.SetLevel(newLevel);

            } else {

                var newCurrentExp = playerUnit.CurrentExp + expDrop;
                //playerUnit.SetExp(newCurrentExp, playerUnit.ExpToLevelUp);
            }
        }

        private void HandleLevelUp() {


        }
    }
}
