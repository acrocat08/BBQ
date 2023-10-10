using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BBQ.Action.Play;

namespace BBQ.Database {
    [CreateAssetMenu(menuName = "Database/ItemSet")]
    public class ItemSet : ScriptableObject {
        public List<FoodData> foods;
        public List<FoodData> supportFoods;
        public List<FoodEffect> effects;
        public List<ToolData> tools;
        
        public FoodData GetRandomFood(int minTier, int maxTier, string tag = "") {
            return foods.Where(x => x.tier >= minTier && x.tier <= maxTier)
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
            return foods.Concat(supportFoods).ToList().IndexOf(data) + 1;
        }
    }
    

}