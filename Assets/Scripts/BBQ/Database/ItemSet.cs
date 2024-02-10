using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BBQ.PlayData;

namespace BBQ.Database {
    [CreateAssetMenu(menuName = "Database/ItemSet")]
    public class ItemSet : ScriptableObject {
        public List<FoodData> foods;
        public List<FoodData> supportFoods;
        public List<FoodEffect> effects;
        public List<ToolData> tools;

        private List<FoodData> _randomPool;
        
        public FoodData GetRandomFood(int minTier, int maxTier, string tag = "") {
            return GetFoodPool().Where(x => x.tier >= minTier && x.tier <= maxTier)
                .Where(x => tag == "" || x.tag == tag)
                .OrderBy(_ => Guid.NewGuid()).First();
        }

        public FoodData SearchFood(string foodName) {
            return foods.Concat(supportFoods).FirstOrDefault(x => x.foodName == foodName);
        }

        public ToolData GetRandomTool(int min, int max) {
            return tools.Where(x => x.tier >= min && x.tier <= max)
                .OrderBy(_ => Guid.NewGuid()).First();
        }
        
        public ToolData SearchTool(string toolName) {
            return tools.FirstOrDefault(x => x.toolName == toolName);
        }


        public int GetFoodIndex(FoodData data) {
            if (data.foodName == "タコ足") return 0;
            return GetFoodPool().Concat(supportFoods).ToList().IndexOf(data) + 1;
        }

        public List<FoodData> GetFoodPool() {
            if (PlayerConfig.GetGameMode() == GameMode.random) {
                return _randomPool;
            }
            ShopPool pool = PlayerConfig.GetShopPool(PlayerConfig.GetPoolIndex());
            return pool.foodsIndex.Select(x => foods[x]).ToList();
        }

        public void MakeRandomPool() {
            _randomPool = new List<FoodData>();
            for (int i = 1; i <= 5; i++) {
                _randomPool.AddRange(foods
                    .Where(x => x.tier == i)
                    .OrderBy(x => Guid.NewGuid())
                    .Take(10));
            }
            _randomPool = _randomPool.OrderBy(x => foods.IndexOf(x)).ToList();
        }
    }
    

}