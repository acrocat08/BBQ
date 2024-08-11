using System.Collections.Generic;
using System.Linq;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SetStack")]
    public class SetStack : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            if (deckFoods.Count == 0) return;
            deckFoods[0].stack = v.GetNum(v.n2);
            if (deckFoods[0].GetObject()) {
                deckFoods[0].GetObject().UpdateStack();
            }
        }
    }
}
