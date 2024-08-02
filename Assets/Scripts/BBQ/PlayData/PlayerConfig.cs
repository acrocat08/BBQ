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
            "x65L+4BzqWTztj+bTFjiZ9tpXS8PVeNXY/y8SJ8TUpcDgd0M16C6jfL7txpFpRCyioFCgQ72KQjAIbcXadvtgSa7soSz17hW5UaRP1pwYSCw8kJBpzgQb9FWAZbN4pSomNU6BPzhWfWB2q0GBIFZvvsymWO/HtwoOYOq98xhrSKywx5zdGelQM0RjH+7XxpyFLfc29hL3fhe0q1p/5W1wQ==",
            "pF9QEPszyeSUk5TlYpbpwtgl9cj1QW/uQ0GznLciOsJ0zYM1tch9d0dvH/N2RUGZ9LWV/XNISbmNDYh6mz/gL7/cmGOHbVg8wbEWpF5DBw5Aec3nRptGLt6m2ycaBAy90p9gnGcNPdN9fVQnDKXOVgyqDZFRC+yQulNw3m4O3ZQHy4X2CIowMTwLLyHgHI9HSDDIPDILCCgbXyUAh+fBPG0a/N6wlgWMWyaP6IrcoQAoeTCcweDGuJO1VZBtsLPA",
            "MjGnhoTh1fIwsAwXKmVkZ4ASyBEBzIrnkyxj7chDQwC6rm0AfQlmlGU8j8bCoEXyVKxaSCa6isOEG0V5BnxDUL5/q5uDDxrT2U8Qgk1sdbUiEcA9o66hQE2x/5H+mXHscv7OSCHkWtJ4ZUZvR2v5ZkEd87B9BCFoYV5v5ExyaQ+Gp4ELsgXozRee693RQBLbI9+uuDIx9/QrPGy4pEXwRj7PpdAbYDfJ3JTIQ+oVDUBphqDVGAclHLzCejQ+W22y",
            "yFeofF6tNnbRjfx71bTUmVQgs1xvl1UczKhcudOyWmoKTSOVaPVzAMMjKh3fOCg2tlaRvuGyOTpaP8HWCfhqL7JL00bBwAmlBOYDFAMjpCGL3mbMmDxaV7jypHf30xZN6JSTQE7ovwa7B7MVzW3MoVck5WwIaAN98pqs+FdoTUuYTotPhWFAdGlxu+u/A+c5Rng1OQ3YfBmyVQOEwPO9UyBJ8JpEThYiUH/QNzA0CSA=",
            "m64rg8aXedWkMYn+6It5PwmpI7CXBZG4T1gUwZmEw6DeEYw21ICL0Ep5ydpvwFgxapwMcEh09SfM72g44T68O2v2HgghC33OMo8xI8zIbKgM0fMDVJfFPwqAj2y6p/TMr2AYzO1fXDR9bEonMbD3ct5JXTDwzqXn+Ku5e9NXYiO9jy0b0qTsgWba6h1+815qajbSqX1PUNz+yN65oZ0MHQxEA22zxu6HS8VaiAynl/Q=",
            "x65L+4BzqWTztj+bTFjiZ9tpXS8PVeNXY/y8SJ8TUpcDgd0M16C6jfL7txpFpRCyioFCgQ72KQjAIbcXadvtgSa7soSz17hW5UaRP1pwYSCw8kJBpzgQb9FWAZbN4pSomNU6BPzhWfWB2q0GBIFZvvsymWO/HtwoOYOq98xhrSKywx5zdGelQM0RjH+7XxpyFLfc29hL3fhe0q1p/5W1wQ==",
            "x65L+4BzqWTztj+bTFjiZ9tpXS8PVeNXY/y8SJ8TUpcDgd0M16C6jfL7txpFpRCyioFCgQ72KQjAIbcXadvtgSa7soSz17hW5UaRP1pwYSCw8kJBpzgQb9FWAZbN4pSomNU6BPzhWfWB2q0GBIFZvvsymWO/HtwoOYOq98xhrSKywx5zdGelQM0RjH+7XxpyFLfc29hL3fhe0q1p/5W1wQ==",
            "x65L+4BzqWTztj+bTFjiZ9tpXS8PVeNXY/y8SJ8TUpcDgd0M16C6jfL7txpFpRCyioFCgQ72KQjAIbcXadvtgSa7soSz17hW5UaRP1pwYSCw8kJBpzgQb9FWAZbN4pSomNU6BPzhWfWB2q0GBIFZvvsymWO/HtwoOYOq98xhrSKywx5zdGelQM0RjH+7XxpyFLfc29hL3fhe0q1p/5W1wQ==",
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
