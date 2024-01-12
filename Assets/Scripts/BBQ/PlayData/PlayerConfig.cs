using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BBQ.PlayData {
    public class PlayerConfig {

        private static PlayerConfig _saveData;

        private ShopPool _shopPool;
        private GameMode _mode;

        public static void Create(ShopPool shopPool, GameMode mode) {
            _saveData = new PlayerConfig();
            _saveData._shopPool = shopPool;
            _saveData._mode = mode;
        }
        
        public static ShopPool GetShopPool() {
            if (_saveData == null) return GetDefaultShopPool();
            return _saveData._shopPool;
        }

        private static ShopPool GetDefaultShopPool() {
            List<int> index = new List<int>();
            for (int i = 0; i < 5; i++) {
                List<int> index2 = new List<int>();
                for (int j = 0; j < 20; j++) {
                    index2.Add(j + i * 20);
                }
                index.AddRange(index2.OrderBy(x => Guid.NewGuid()).Take(10));
            }
            return new ShopPool(index, "defaultPool");
        }

        public static GameMode GetGameMode() {
            if (_saveData == null) return GameMode.easy;
            return _saveData._mode;
        }
    }

    public enum GameMode {
        easy,
        hard,
    }
}
