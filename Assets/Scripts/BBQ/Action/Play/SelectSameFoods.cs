using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SelectSameFood")]
    public class SelectSameFood : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            string foodName = v.GetString(v.n2);
            v.f1 = deckFoods.Where(x => x.data.foodName == foodName).ToList();
        }
    }
}