using ShadowFlareRemake.Combat;
using ShadowFlareRemake.Player;
using System.Threading.Tasks;
using UnityEngine;

namespace ShadowFlareRemake.GameManager {
    public class GameManager : MonoBehaviour {

        public static GameManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private PlayerController _player;
        [SerializeField] private PlayerStats _playerStats;

        private Unit _playerUnit;

        private async void Awake() {

            InitSingelton();
            await InitPlayer();
        }

        private void InitSingelton() {

            if(Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);

            } else if(this != Instance) {
                Destroy(this);
            }
        }

        private async Task InitPlayer() {

            _playerUnit = new Unit(_playerStats);

            await _player.InitPlayer(_playerUnit);

            _player.OnIGotHit += HandlePlayerGotHit;
        }

        private void HandlePlayerGotHit(Attack attack, IUnit unit) {

        }
    }
}

