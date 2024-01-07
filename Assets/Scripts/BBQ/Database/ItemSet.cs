using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BBQ.Action.Play;
using UnityEngine.Serialization;

namespace BBQ.Database {
    [CreateAssetMenu(menuName = "Database/ItemSet")]
    public class ItemSet : ScriptableObject {
        public List<FoodData> foods;
        public List<FoodData> supportFoods;
        public List<FoodEffect> effects;
        public List<ToolData> tools;

        public List<FoodData> chosenFoods;

        public void ChooseFoods() {   //Debug
            hideFlags = HideFlags.DontSave;
            chosenFoods = new List<FoodData>();
            for (int i = 1; i <= 5; i++) {
                List<FoodData> chosen = foods.Where(x => x.tier == i)
                    .OrderBy(_ => Guid.NewGuid())
                    .Take(10)
                    .ToList();
                chosenFoods.AddRange(chosen);
            }
        }
        
        public FoodData GetRandomFood(int minTier, int maxTier, string tag = "") {
            return chosenFoods.Where(x => x.tier >= minTier && x.tier <= maxTier)
                .Where(x => tag == "" || x.tag == tag)
                .OrderBy(_ => Guid.NewGuid()).First();
        }

        public FoodData SearchFood(string foodName) {
            return chosenFoods.Concat(supportFoods).FirstOrDefault(x => x.foodName == foodName);
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
            return chosenFoods.Concat(supportFoods).ToList().IndexOf(data) + 1;
        }

        public void ChooseAll() {
            chosenFoods = new List<FoodData>(foods);
        }
    }
    

}