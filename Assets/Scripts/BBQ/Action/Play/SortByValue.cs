using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SortByValue")]
    public class SortByValue : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            v.f1 = deckFoods
                .OrderBy(Value)
                .Select(x => x)
                .ToList();
        }

        int Value(DeckFood food) {
            return food.data.cost * (food.lank == 1 ? 1 : food.lank * 3 - 3);
        }
    }
}