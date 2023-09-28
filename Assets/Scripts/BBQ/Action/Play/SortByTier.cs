using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SortByTier")]
    public class SortByTier : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            v.f1 = deckFoods
                .OrderBy(x => x.data.tier)
                .Select(x => x)
                .ToList();
        }
    }
}