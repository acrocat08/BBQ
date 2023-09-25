using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BBQ.Database {
    [CreateAssetMenu(menuName = "Database/ItemSet")]
    public class ItemSet : ScriptableObject {
        public List<FoodData> foods;
        public List<FoodData> supportFoods;
        public List<ToolData> tools;
        
        public FoodData GetRandomFood(int tier) {
            return foods.Where(x => x.tier == tier)
                .OrderBy(_ => Guid.NewGuid()).First();
        }

        public FoodData SearchFood(string foodName) {
            return foods.Concat(supportFoods).FirstOrDefault(x => x.foodName == foodName);
        }

        public ToolData GetRandomTool(int level) {
            return tools.Where(x => x.tier <= level)
                .OrderBy(_ => Guid.NewGuid()).First();
        }
    }
    

}
