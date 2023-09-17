using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SwapFoods")]
    public class SwapFoods : PlayAction {

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> x = v.GetFoods(v.n1);
            List<DeckFood> y = v.GetFoods(v.n2);
            v.SetFoods(v.n1, y);
            v.SetFoods(v.n2, x);
        }
    }
}