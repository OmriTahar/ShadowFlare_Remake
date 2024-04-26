using ShadowFlareRemake.UI;
using UnityEngine;

namespace ShadowFlareRemake.Rewards {
    public class RewardsManager : MonoBehaviour {

        public static RewardsManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private UIController _uiController;
        [SerializeField] private IUnitStats _playerUnitStats;


        private void Awake() {
            InitSingelton();
        }

        public void HandleEnemyKilledRewards(IUnitStats killedEnemy) {

            //HandleExpReward(killedEnemy.ExpDrop);
        }

        private void InitSingelton() {

            if(Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);

            } else if(this != Instance) {
                Destroy(this);
            }
        }

        private void HandleExpReward(int expDrop) {

            //    if(expDrop <= 0) {
            //        return;
            //    }

            //    if(_playerUnitStats.CurrentExp + expDrop >= _playerUnitStats.ExpToLevelUp) {

            //        var newExp = _playerUnitStats.CurrentExp + expDrop;
            //        var spareExp = newExp - _playerUnitStats.ExpToLevelUp;

            //        _playerUnitStats.Level++; // Todo: Set max level
            //        _playerUnitStats.ExpToLevelUp *= 2; // Todo: Do this better
            //        _playerUnitStats.CurrentExp = spareExp;

            //        _uiController.UpdatePlayerLevel();
            //    }

            //    _playerUnitStats.CurrentExp += expDrop;
            //    _uiController.UpdatePlayerExp();
            //}\
        }
    }
}
