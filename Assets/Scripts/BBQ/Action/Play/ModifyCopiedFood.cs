using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/ModifyCopiedFood")]
    public class ModifyCopiedFood : PlayAction {
        [SerializeField] private AddEffect addEffect;
        [SerializeField] private ActionAssembly assembly;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.f1;
            foreach (DeckFood deckFood in deckFoods) {
                deckFood.lank = v.GetNum(v.n1);
                FoodEffect effect = addEffect.GetEffect(v.GetString(v.n2));
                if (deckFood.effect != null) await assembly.Run(deckFood.effect.onReleased, env, deckFood, v.target);
                if (effect != null) await assembly.Run(effect.onAttached, env, deckFood, v.target);
                deckFood.effect = effect;
            }
        }
    }
}