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
        private HashSet<string> _cosplay;

        private static string[] defaultCodes = {
            "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"
        };

        public static void Create(ShopPool shopPool, int poolIndex, int selectedIndex, GameMode mode, string cosplayName = "") {
            PlayerPrefs.SetString("Pool_" + poolIndex, shopPool.Encode());
            PlayerPrefs.SetInt("selectedIndex", selectedIndex);
            PlayerPrefs.SetInt("mode", (int)mode);
            if (cosplayName != "") SetCosplayState(cosplayName);
            _saveData = LoadData();
        }


        private static void SetCosplayState(string cosplayName) {
            List<string> state = PlayerPrefs.GetString("cosplay", "").Split(",").ToList();
            if (state.Contains(cosplayName)) state.Remove(cosplayName);
            else state.Add(cosplayName);
            PlayerPrefs.SetString("cosplay", string.Join(",", state));
            Debug.Log(string.Join(",", state));
        }
        

        public static bool CheckCosplay(string foodName) {
            if (_saveData == null) _saveData = LoadData();
            return _saveData._cosplay.Contains(foodName);
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
            List<int> index = new();
            for (int i = 0; i < 5; i++) {
                for (int j = 0; j < 10; j++) {
                    index.Add(j + i * 20);
                }
            }
            return new(index, poolName);
        }

        public static GameMode GetGameMode() {
            if (_saveData == null) _saveData = LoadData();
            return _saveData._mode;
        }

        private static PlayerConfig LoadData() {
            List<ShopPool> pools = new();
            for (int i = 0; i < 8; i++) {
                pools.Add(ShopPool.Decode(defaultCodes[i]));
            }
            
            pools.Add(ShopPool.Decode(defaultCodes[0]));
            for (int i = 0; i < 8; i++) {
                string poolName = "Pool_" + i;
                string hashCode = PlayerPrefs.GetString(poolName, defaultCodes[i]);
                pools.Add(ShopPool.Decode(hashCode));
            }
            int poolIndex = PlayerPrefs.GetInt("selectedIndex", 0);
            GameMode mode = (GameMode)PlayerPrefs.GetInt("mode", (int)GameMode.easy);
            PlayerConfig config = new();
            config._shopPools = pools;
            config._poolIndex = poolIndex;
            config._mode = mode;
            config._cosplay = new(PlayerPrefs.GetString("cosplay", "").Split(","));
            return config;
        }
    }

    public enum GameMode {
        easy,
        normal,
        hard,
    }
}
