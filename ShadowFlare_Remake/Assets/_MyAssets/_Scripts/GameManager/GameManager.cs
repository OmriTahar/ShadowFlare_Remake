using ShadowFlareRemake.Combat;
using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.Player;
using ShadowFlareRemake.PlayerStats;
using ShadowFlareRemake.Rewards;
using ShadowFlareRemake.UI;
using System.Threading.Tasks;
using UnityEngine;

namespace ShadowFlareRemake.GameManager {
    public class GameManager : MonoBehaviour {

        [Header("General")]
        [SerializeField] private EnemiesManager _enemiesManager;
        [SerializeField] private RewardsManager _rewardsManager;
        [SerializeField] private UIController _uiController;

        [Header("Player")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private ConcretePlayerStats _playerStats;

        private Unit _playerUnit;

        private async void Awake() {

            DontDestroyOnLoad(gameObject);
            await InitPlayer();
        }

        private void Start() {

            RegisterEvents();
            _uiController.UpdatePlayerStats(_playerUnit);
        }

        private void OnDestroy() {

            DergisterEvents();
        }

        private void RegisterEvents() {

            _playerController.OnIGotHit += HandlePlayerGotHit;

            _enemiesManager.OnEnemyDied += HandleEnemyDied;
        }

        private void DergisterEvents() {

            _playerController.OnIGotHit -= HandlePlayerGotHit;

            _enemiesManager.OnEnemyDied -= HandleEnemyDied;
        }

        private async Task InitPlayer() {

            _playerUnit = new Unit(_playerStats);

            await _playerController.InitPlayer(_playerUnit);
        }

        private void HandlePlayerGotHit(Attack attack, IUnit unit) {

        }

        private void HandleEnemyDied(IUnitStats unitStats) {

            _rewardsManager.GiveRewardsToPlayer(_playerStats, unitStats as EnemyStats);
            _uiController.UpdatePlayerStats(_playerUnit);
        }
    }
}

