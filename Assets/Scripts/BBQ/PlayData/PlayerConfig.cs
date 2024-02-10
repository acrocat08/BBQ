using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BBQ.PlayData {
    public class PlayerConfig {

        private static PlayerConfig _saveData;

        private List<ShopPool> _shopPools;
        private int _poolIndex;
        private GameMode _mode;

        public static void Create(ShopPool shopPool, int poolIndex, GameMode mode) {
            PlayerPrefs.SetString(shopPool.poolName, shopPool.Encode());
            PlayerPrefs.SetInt("poolIndex", poolIndex);
            PlayerPrefs.SetInt("mode", (int)mode);
            _saveData = LoadData();
        }
        
        public static ShopPool GetShopPool(int index) {
            if (_saveData == null) _saveData = LoadData();
            return _saveData._shopPools[index];
        }
        public static int GetPoolIndex() {
            if (_saveData == null) _saveData = LoadData();
            return _saveData._poolIndex;
        }

        private static ShopPool GetDefaultShopPool(string poolName) {
            List<int> index = new List<int>();
            for (int i = 0; i < 5; i++) {
                for (int j = 0; j < 10; j++) {
                    index.Add(j + i * 20);
                }
            }
            return new ShopPool(index, poolName);
        }

        public static GameMode GetGameMode() {
            if (_saveData == null) _saveData = LoadData();
            return _saveData._mode;
        }

        private static PlayerConfig LoadData() {
            List<ShopPool> pools = new List<ShopPool>();
            for (int i = 0; i < 6; i++) {
                string poolName = "pool_" + (i + 1);
                string hashCode = PlayerPrefs.GetString(poolName, GetDefaultShopPool(poolName).Encode());
                pools.Add(ShopPool.Decode(hashCode, poolName));
            }
            int poolIndex = PlayerPrefs.GetInt("poolIndex", 0);
            GameMode mode = (GameMode)PlayerPrefs.GetInt("mode", (int)GameMode.easy);
            PlayerConfig config = new PlayerConfig();
            config._shopPools = pools;
            config._poolIndex = poolIndex;
            config._mode = mode;
            return config;
        }
    }

    public enum GameMode {
        easy,
        hard,
        random,
    }
}
