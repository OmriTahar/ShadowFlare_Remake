using ShadowFlareRemake.Loot;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.LootManagement
{
    public class LootManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _lootParent;
        [SerializeField] private LootView _lootPrefab;

        [Header("--- Player Starting Loot ---")]
        [SerializeField] private List<LootData_ScriptableObject> _playerStartingLoot;

        [Header("--- Enemies Drop ---")]
        [SerializeField] private List<LootData_ScriptableObject> _lootDropsData;

        private const int _lootPoolsStartingAmount = 30;

        private Stack<LootView> _lootViewsPool = new Stack<LootView>(_lootPoolsStartingAmount);
        private Stack<LootModel> _lootModelsPool = new Stack<LootModel>(_lootPoolsStartingAmount);

        #region Test Fields

        [Header("--- Test ---")]
        [SerializeField] private LootData_ScriptableObject _testLootDataToSpawn;
        [SerializeField] private List<LootData_ScriptableObject> _testLootDataToSpawnOnAwake;

        private List<int> _testGoldSpawnList = new List<int>() { 1, 50, 300, 800, 5000, 10000, 316, 3576 };
        private int _testGoldSpawnIndex = 0;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            ExpandLootViewsPool();
            ExpandLootModelsPool();
        }

        #endregion

        #region Meat & Potatos

        public LootModel[] GetPlayerStartingLoot()
        {
            var startingLootModels = new LootModel[_playerStartingLoot.Count];

            for (int i = 0; i < _playerStartingLoot.Count; i++)
            {
                var lootModel = GetLootModel();
                lootModel.SetLootData(_playerStartingLoot[i]);
                startingLootModels[i] = lootModel;
            }

            return startingLootModels;
        }

        public void HandleLootDrop(int level, int lootDropChance, Vector3 position)
        {
            var randomNum = Random.Range(0, 100);

            if(randomNum > lootDropChance)
                return;

            var lootModel = GetGeneratedLootModel(level);

            if(lootModel == null)
                return;

            DropLoot(lootModel, position, true);
        }

        private LootModel GetGeneratedLootModel(int level) // TODO: Guess
        {
            var randomIndex = Random.Range(0, _lootDropsData.Count - 1);
            var lootData = _lootDropsData[randomIndex];
            var lootModel = GetLootModel();
            lootModel.SetLootData(lootData);

            if(lootModel.LootCategory == LootCategory.Gold)
            {
                var randomAmount = Random.Range(1, 1250);
                randomAmount *= level;
                lootModel.SetAmountAndGetSpareWhenMaxed(randomAmount);
            }

            return lootModel;
        }

        public void DropLoot(LootModel lootModel, Vector3 pos, bool useOffset)
        {
            var lootView = GetLootView();
            var randomOffsetX = 0f;
            var randomOffsetZ = 0f;

            if(useOffset)
            {
                randomOffsetX = Random.Range(-1f, 1f);
                randomOffsetZ = Random.Range(-1f, 1f);
            }

            lootView.transform.position = new Vector3(pos.x + randomOffsetX, pos.y, pos.z + randomOffsetZ);
            lootView.gameObject.SetActive(true);

            lootView.SetModel(lootModel);
            lootModel.InvokeDropAnimation();
        }

        public void ReturnLootToPools(LootView lootView, LootModel lootModel)
        {
            lootView.gameObject.SetActive(false);
            _lootViewsPool.Push(lootView);
            _lootModelsPool.Push(lootModel);
        }

        private void ExpandLootViewsPool()
        {
            for(int i = 0; i < _lootPoolsStartingAmount; i++)
            {
                var addedLootView = Instantiate(_lootPrefab, _lootParent);
                _lootViewsPool.Push(addedLootView);
                addedLootView.gameObject.SetActive(false);
            }
        }

        private void ExpandLootModelsPool()
        {
            for(int i = 0; i < _lootPoolsStartingAmount; i++)
            {
                var addedLootModel = new LootModel();
                _lootModelsPool.Push(addedLootModel);
            }
        }

        private LootView GetLootView()
        {
            if(_lootViewsPool.Count == 0)
            {
                ExpandLootViewsPool();
            }

            return _lootViewsPool.Pop();
        }

        private LootModel GetLootModel()
        {
            if(_lootModelsPool.Count == 0)
            {
                ExpandLootModelsPool();
            }

            return _lootModelsPool.Pop();
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
                TestSpawnLootLogic(lootData, spawnPosX, spawnPosZ, false);

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
            TestSpawnLootLogic(_testLootDataToSpawn, pos.x, pos.z - 2, true);
        }

        private void TestSpawnLootLogic(LootData_ScriptableObject lootData, float posX, float posZ, bool isAnimatingDrop)
        {
            var lootModel = GetLootModel();
            lootModel.SetLootData(lootData);

            if(lootModel.LootCategory == LootCategory.Gold)
            {
                if(_testGoldSpawnIndex <= _testGoldSpawnList.Count - 1)
                {
                    lootModel.SetAmountAndGetSpareWhenMaxed(_testGoldSpawnList[_testGoldSpawnIndex]);
                    _testGoldSpawnIndex++;
                }
            }

            var lootView = GetLootView();
            var pos = lootView.transform.position;
            lootView.transform.position = new Vector3(posX, 0, posZ);

            lootView.SetModel(lootModel);
            lootView.gameObject.SetActive(true);

            if(isAnimatingDrop)
            {
                lootModel.InvokeDropAnimation();
            }
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
