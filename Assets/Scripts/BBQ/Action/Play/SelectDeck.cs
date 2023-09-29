using System.Collections.Generic;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SelectDeck")]
    public class SelectDeck : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            if (env.isShopping) {
                v.f1 = new List<DeckFood>(env.inventory.GetDeckFoods());
            }
            else {
                v.f1 = new List<DeckFood>(env.deck.SelectAll());
            }
        }
    }
}