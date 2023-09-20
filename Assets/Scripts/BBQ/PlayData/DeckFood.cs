using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.Database;
using Unity.Collections;
using UnityEngine;

namespace BBQ.PlayData {
    [Serializable]
    public class DeckFood {
        public FoodData data;
        [Range(1, 3)]
        public int lank;
        public bool isFrozen;
        public bool isFired;
        public bool isEphemeral;
        public FoodEffect effect;
        [HideInInspector] public IReleasable Releasable;

        public DeckFood(FoodData data) {
            this.data = data;
            lank = 1;
            isFrozen = false;
            isFired = false;
            isEphemeral = false;
            Releasable = null;
            effect = null;
        }

        public DeckFood Copy() {
            DeckFood ret = new DeckFood(data) {
                lank = lank,
                isFrozen = isFrozen,
                isFired = isFired,
                isEphemeral = isEphemeral,
                Releasable = Releasable,
                effect = effect
            };
            return ret;
        }

        public FoodObject Release() {
            return Releasable.ReleaseFoods(new List<DeckFood> { this }).First();
        }
    }
}
