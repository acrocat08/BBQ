using System.Collections.Generic;
using BBQ.Common;
using BBQ.PlayData;
using UnityEngine;

namespace BBQ.Cooking {
    public interface IReleasable {
        public List<FoodObject> ReleaseFoods(List<DeckFood> foods);

        public FoodObject GetObject(DeckFood food);
    }
}
