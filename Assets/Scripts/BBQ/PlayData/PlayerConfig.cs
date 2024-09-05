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
            "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"
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
                pools.Add(ShopPool.Decode(defaultCodes[i]));
            }
            
            pools.Add(ShopPool.Decode(defaultCodes[0]));
            for (int i = 0; i < 8; i++) {
                string poolName = "Pool_" + i;
                string hashCode = PlayerPrefs.GetString(poolName, defaultCodes[0]);
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
        normal,
        hard,
    }
}
