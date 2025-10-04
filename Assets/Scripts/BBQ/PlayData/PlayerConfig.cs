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
        private float _bgmVolume;
        private float _seVolume;

        private static string[] defaultCodes = {
            "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"
        };

        public static void Create(ShopPool shopPool, int poolIndex, int selectedIndex, GameMode mode, float bgmVolume, float seVolume, string cosplayName = "") {
            PlayerPrefs.SetString("Pool_" + poolIndex, shopPool.Encode());
            PlayerPrefs.SetInt("selectedIndex", selectedIndex);
            PlayerPrefs.SetInt("mode", (int)mode);
            if (cosplayName != "") SetCosplayState(cosplayName);
            _saveData = LoadData();
            PlayerPrefs.SetFloat("bgm", bgmVolume);
            PlayerPrefs.SetFloat("se", seVolume);

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

        public static float GetBgmVolume() {
            if (_saveData == null) _saveData = LoadData();
            return _saveData._bgmVolume;
        }
        
        public static float GetSeVolume() {
            if (_saveData == null) _saveData = LoadData();
            return _saveData._seVolume;
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
            config._bgmVolume = PlayerPrefs.GetFloat("bgm", 1);
            config._seVolume = PlayerPrefs.GetFloat("se", 1);
            return config;
        }
    }

    public enum GameMode {
        easy,
        normal,
        hard,
    }
}
