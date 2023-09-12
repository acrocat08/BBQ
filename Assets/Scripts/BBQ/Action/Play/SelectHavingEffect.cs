using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SelectHavingEffect")]
    public class SelectHavingEffect : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            string effect = v.GetString(v.n2);
            v.f1 = deckFoods.Where(x => CheckEffect(x, effect)).ToList();
            v.f2 = deckFoods.Where(x => !CheckEffect(x, effect)).ToList();
        }

        bool CheckEffect(DeckFood deckFood, string target) {
            if (deckFood.effect == null) return target == "none";
            return deckFood.effect.effectName == target;
        }
    }
}