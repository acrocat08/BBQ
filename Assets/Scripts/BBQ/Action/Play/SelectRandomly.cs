using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SelectRandomly")]
    public class SelectRandomly : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            int num = v.GetNum(v.n2);
            v.f1 = deckFoods.OrderBy(x => Guid.NewGuid()).Take(num).ToList();
        }
    }
}