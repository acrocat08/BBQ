using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SelectNotSaled")]
    public class SelectNotSaled : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            if(env.isShopping) v.f1 = new List<DeckFood>(env.shop.GetShopFoods()
                .Where(x => x.GetCost() == x.GetFoodData().cost).Select(x => x.deckFood));
        }
    }
}