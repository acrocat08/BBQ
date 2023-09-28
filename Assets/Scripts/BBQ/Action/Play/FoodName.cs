using System.Collections.Generic;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/FoodName")]
    public class FoodName : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            if (deckFoods.Count == 0) return;
            v.s1 = deckFoods[0].data.foodName;
        }
    }
}