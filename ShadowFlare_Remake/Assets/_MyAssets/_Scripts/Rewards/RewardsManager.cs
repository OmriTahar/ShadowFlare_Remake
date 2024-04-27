using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.PlayerStats;
using UnityEngine;

namespace ShadowFlareRemake.Rewards {
    public class RewardsManager : MonoBehaviour {

        private bool _isPendingLevelUp = false;

        public void GiveRewardsToPlayer(ConcretePlayerStats playerStats, EnemyStats enemyStats) {

            HandleExpReward(playerStats, enemyStats.ExpDrop);
        }

        private void HandleExpReward(ConcretePlayerStats playerStats, int expDrop) {

            if(expDrop <= 0) {
                return;
            }

            _isPendingLevelUp = playerStats.CurrentExp + expDrop >= playerStats.ExpToLevelUp;

            if(_isPendingLevelUp) {

                var totalExp = playerStats.CurrentExp + expDrop;
                var newCurrentExp = totalExp - playerStats.ExpToLevelUp;
                var expToLevelUp = playerStats.ExpToLevelUp * 2;           // Todo: Do this better
                var newLevel = playerStats.Level + 1;                      // Todo: Set max level

                playerStats.SetExp(newCurrentExp, expToLevelUp);
                playerStats.SetLevel(newLevel);

            } else {

                var newCurrentExp = playerStats.CurrentExp + expDrop;
                playerStats.SetExp(newCurrentExp, playerStats.ExpToLevelUp);
            }
        }

        private void HandleLevelUp() {


        }
    }
}
