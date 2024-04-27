using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.PlayerStats;
using UnityEngine;

namespace ShadowFlareRemake.Rewards {
    public class RewardsManager : MonoBehaviour {

        public void GiveRewardsToPlayer(ConcretePlayerStats playerStats, EnemyStats enemyStats) {

            if(enemyStats is not EnemyStats) {
                Debug.LogError("Rewards manager recieved stats that are NOT enemy stats!");
                return;
            }

            HandleExpReward(playerStats, enemyStats.ExpDrop);
        }

        private void HandleExpReward(ConcretePlayerStats playerStats, int expDrop) {

            if(expDrop <= 0) {
                return;
            }

            if(playerStats.CurrentExp + expDrop >= playerStats.ExpToLevelUp) {

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
    }
}
