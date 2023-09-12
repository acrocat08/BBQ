using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SelectEffect")]
    public class SelectEffect : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {

            DeckFood deckFood = v.GetFoods(v.n1).FirstOrDefault();
            if (deckFood == null || deckFood.effect == null) v.s1 = "none";
            else v.s1 = deckFood.effect.effectName;
        }
    }
}