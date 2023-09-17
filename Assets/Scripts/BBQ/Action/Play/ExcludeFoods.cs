using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/ExcludeFoods")]
    public class ExcludeFoods : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            List<DeckFood> excludeFoods = v.GetFoods(v.n2);

            v.f1 = deckFoods.Where(x => !excludeFoods.Contains(x)).ToList();
        }
    }
}