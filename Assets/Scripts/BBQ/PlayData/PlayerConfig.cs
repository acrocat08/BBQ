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

        private static string[] defaultCodes = {
            "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",
        };

        public static void Create(ShopPool shopPool, int poolIndex, int selectedIndex, GameMode mode) {
            PlayerPrefs.SetString("Pool_" + poolIndex, shopPool.Encode());
            PlayerPrefs.SetInt("selectedIndex", selectedIndex);
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
            for (int i = 0; i < 8; i++) {
                string poolName = "Pool_" + i;
                string hashCode = PlayerPrefs.GetString(poolName, defaultCodes[i]);
                pools.Add(ShopPool.Decode(hashCode));
            }
            int poolIndex = PlayerPrefs.GetInt("selectedIndex", 0);
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
