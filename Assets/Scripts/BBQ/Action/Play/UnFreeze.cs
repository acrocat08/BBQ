using System.Collections.Generic;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/UnFreeze")]
    public class UnFreeze : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            foreach (DeckFood deckFood in deckFoods) {
                deckFood.GetObject().UnFreeze();
            }
        }
    }
}