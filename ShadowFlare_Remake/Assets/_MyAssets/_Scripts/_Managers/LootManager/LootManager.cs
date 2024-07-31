using ShadowFlareRemake.Loot;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.LootManagement
{
    public class LootManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _lootParent;
        [SerializeField] private GameObject _lootPrefab;

        [Header("--- Player Starting Loot ---")]
        [SerializeField] private List<LootData_ScriptableObject> _playerStartingLoot;

        [Header("--- Enemies Drop ---")]
        [SerializeField] private List<LootData_ScriptableObject> _lootDropsData;

        [Header("--- Test ---")]
        [SerializeField] private LootData_ScriptableObject _testLootDataToSpawn;
        [SerializeField] private List<LootData_ScriptableObject> _testLootDataToSpawnOnAwake;

        private List<int> _testGoldSpawnList = new List<int>() { 1, 50, 300, 800, 5000, 10000, 316, 3576 };
        private int _testGoldSpawnIndex = 0;

        #region Meat & Potatos

        public List<LootData_ScriptableObject> GetPlayerStartingLoot()
        {
            return _playerStartingLoot;
        }

        public void HandleLootDrop(int level, int lootDropChance, Vector3 enemyPosition)
        {
            var randomNum = Random.Range(0, 100);

            if(randomNum > lootDropChance)
                return;

            var loot = GenerateLoot(level);

            if(loot == null)
                return;

            DropLootReward(loot, enemyPosition);
        }

        private LootModel GenerateLoot(int level) // TODO: Guess
        {
            var randomIndex = Random.Range(0, _lootDropsData.Count - 1);
            var lootData = _lootDropsData[randomIndex];
            var lootModel = new LootModel(lootData);

            if(lootModel.LootCategory == LootCategory.Gold)
            {
                var randomAmount = Random.Range(1, 1250);
                randomAmount *= level;
                lootModel.SetAmountAndGetSpareWhenMaxed(randomAmount);
            }

            return lootModel;
        }

        public GameObject InstantiateLootPrefab()
        {
            return Instantiate(_lootPrefab);
        }

        private void DropLootReward(LootModel lootModel, Vector3 pos)
        {
            var loot = Instantiate(_lootPrefab, _lootParent);

            var randomOffsetX = Random.Range(-1f, 1f);
            var randomOffsetZ = Random.Range(-1f, 1f);
            loot.transform.position = new Vector3(pos.x + randomOffsetX, pos.y, pos.z + randomOffsetZ);

            var lootView = loot.GetComponentInChildren<LootView>();
            lootView.SetModel(lootModel);
            lootModel.InvokeDropAnimation();
        }

        #endregion

        #region Tests

        public void HandleTestSpawnLoot()
        {
            float spawnPosX = _lootParent.transform.position.x;
            float spawnPosZ = _lootParent.transform.position.z;
            bool isSecondRow = false;

            var offset_X = 1.5f;
            var offset_Z = 2f;

            foreach(var lootData in _testLootDataToSpawnOnAwake)
            {
                TestSpawnLootLogic(lootData, spawnPosX, spawnPosZ);

                if(isSecondRow)
                {
                    spawnPosX += offset_X;
                    spawnPosZ -= offset_Z;
                }
                else
                {
                    spawnPosZ += offset_Z;
                }

                isSecondRow = !isSecondRow;
            }
        }
        public void TestSpawnLootItem()
        {
            var pos = _lootParent.transform.position;
            TestSpawnLootLogic(_testLootDataToSpawn, pos.x, pos.z);
        }

        private void TestSpawnLootLogic(LootData_ScriptableObject lootData, float posX, float posZ)
        {
            var lootModel = new LootModel(lootData);

            if(lootModel.LootCategory == LootCategory.Gold)
            {
                if(_testGoldSpawnIndex <= _testGoldSpawnList.Count - 1)
                {
                    lootModel.SetAmountAndGetSpareWhenMaxed(_testGoldSpawnList[_testGoldSpawnIndex]);
                    _testGoldSpawnIndex++;
                }
            }

            var loot = Instantiate(_lootPrefab, _lootParent);
            var pos = loot.transform.position;
            loot.transform.position = new Vector3(posX, 0, posZ);

            var lootView = loot.GetComponentInChildren<LootView>();
            lootView.SetModel(lootModel);
        }

#if UNITY_EDITOR

        [UnityEditor.CustomEditor(typeof(LootManager))]
        public class Drawer : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                GUILayout.Space(20);

                var lootManager = target as LootManager;

                if(GUILayout.Button("Spawn Loot"))
                {
                    lootManager.TestSpawnLootItem();
                }

                GUILayout.Space(10);
            }
        }

#endif

        #endregion
    }
}
