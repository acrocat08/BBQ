using System;
using System.Collections.Generic;
using BBQ.Database;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "Shop/ItemChoice")]
    public class ShopItemChoice : ScriptableObject {

        public List<TierTable> tierTables;
        public ItemSet itemSet;
        
        public List<FoodData> ChoiceFoods(int level) {
            List<FoodData> ret = new List<FoodData>();
            TierTable nowTable = tierTables[level - 1];
            for (int i = 0; i < 4; i++) {
                int r = Random.Range(0, 100);
                int cnt = 0;
                int[] per = { nowTable.tier1, nowTable.tier2, nowTable.tier3, nowTable.tier4, nowTable.tier5 };
                int tier = 0;
                for (tier = 0; tier < 5; tier++) {
                    cnt += per[tier];
                    if (r < cnt) break;
                }
                ret.Add(itemSet.GetRandomFood(tier + 1, tier + 1));
            }

            return ret;
        }
        
        public ToolData ChoiceTool(int level) {
            return itemSet.GetRandomTool(level);
        }

        [Serializable]
        public class TierTable {
            [Range(0,100)] public int tier1;
            [Range(0,100)] public int tier2;
            [Range(0,100)] public int tier3;
            [Range(0,100)] public int tier4;
            [Range(0,100)] public int tier5;
        }


    }
}
