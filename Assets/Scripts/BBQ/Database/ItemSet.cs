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
        
        public FoodData GetRandomFood(int minTier, int maxTier, string tag = "") {
            return GetFoodPool().Where(x => x.tier >= minTier && x.tier <= maxTier)
                .Where(x => tag == "" || x.tag == tag)
                .OrderBy(_ => Guid.NewGuid()).First();
        }

        public FoodData SearchFood(string foodName) {
            return GetFoodPool().Concat(supportFoods).FirstOrDefault(x => x.foodName == foodName);
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
            ShopPool pool = PlayerConfig.GetShopPool();
            return pool.foodsIndex.Select(x => foods[x]).ToList();
        }
    }
    

}