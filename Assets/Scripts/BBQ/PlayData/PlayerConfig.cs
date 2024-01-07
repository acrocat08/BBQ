using System.Collections.Generic;
using UnityEngine;

namespace BBQ.PlayData {
    public class PlayerConfig {

        private static PlayerConfig _saveData;

        private ShopPool _shopPool;

        public static void Create(ShopPool shopPool) {
            _saveData = new PlayerConfig();
            _saveData._shopPool = shopPool;
        }
        
        public static ShopPool GetShopPool() {
            if (_saveData == null) return GetDefaultShopPool();
            return _saveData._shopPool;
        }

        private static ShopPool GetDefaultShopPool() {
            List<int> index = new List<int>();
            for (int i = 0; i < 5; i++) {
                for (int j = 0; j < 10; j++) {
                    index.Add(j + i * 20);
                }
            }
            return new ShopPool(index, "defaultPool");
        }
    }
}
