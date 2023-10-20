using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/FoodValue")]
    public class FoodValue : PlayAction {

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            if (deckFoods.Count == 0) return;
            v.x1 = deckFoods.Sum(x => x.data.cost * (x.lank == 1 ? 1 : x.lank * 3 - 3));
        }
    }
}