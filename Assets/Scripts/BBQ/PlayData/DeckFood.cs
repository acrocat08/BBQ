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
        public bool isFired;
        public FoodEffect effect;
        [HideInInspector] public IReleasable Releasable;

        public DeckFood(FoodData data) {
            this.data = data;
            lank = 1;
            isFrozen = false;
            isFired = false;
            Releasable = null;
            effect = null;
        }

        public LaneFood Release() {
            return Releasable.ReleaseFoods(new List<DeckFood> { this }).First();
        }
    }
}
