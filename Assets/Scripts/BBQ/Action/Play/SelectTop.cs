using System.Collections.Generic;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SelectTop")]
    public class SelectTop : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            int num = v.GetNum(v.n2);
            v.f1 = new List<DeckFood>(deckFoods.GetRange(0, Mathf.Min(num, deckFoods.Count)));
        }
    }
}