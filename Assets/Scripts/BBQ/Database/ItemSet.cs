using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BBQ.Database {
    [CreateAssetMenu(menuName = "Database/ItemSet")]
    public class ItemSet : ScriptableObject {
        public List<FoodData> foods;
        
        public FoodData GetRandomItem(int tier) {
            return foods.Where(x => x.tier == tier)
                .OrderBy(_ => Guid.NewGuid()).First();
        }
    }
    

}
