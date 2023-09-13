using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SetEphemeral")]
    public class SetEphemeral : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            int mode = v.GetNum(v.n2);
            foreach (DeckFood deckFood in deckFoods) {
                deckFood.isEphemeral = mode == 1;
            }
        }
    }
}