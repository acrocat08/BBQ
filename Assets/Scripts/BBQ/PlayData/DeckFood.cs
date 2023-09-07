using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.Database;
using UnityEngine;

namespace BBQ.PlayData {
    [Serializable]
    public class DeckFood {
        public FoodData data;
        [Range(1, 3)]
        public int lank;
        public bool isFrozen;
        [HideInInspector] public IReleasable Releasable;

        public LaneFood Release() {
            return Releasable.ReleaseFoods(new List<DeckFood> { this }).First();
        }
    }
}
