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
                .OrderBy(x => x.data.cost * (x.lank == 1 ? 1 : x.lank * 3 - 3))
                .Select(x => x)
                .ToList();
        }
    }
}