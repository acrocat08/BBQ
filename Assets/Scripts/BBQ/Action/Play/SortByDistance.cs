using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SortByDistance")]
    public class SortByDistance : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            LaneFood invoker = env.board.FindLaneFood(v.invoker);
            v.f1 = deckFoods
                .Select(x => env.board.FindLaneFood(x))
                .OrderBy(x => (x.transform.position - invoker.transform.position).magnitude)
                .Select(x => x.deckFood)
                .ToList();
        }
    }
}