using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SelectShop")]
    public class SelectShop : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            if(env.isShopping) v.f1 = new List<DeckFood>(env.shop.GetShopFoods().Select(x => x.deckFood));
        }
    }
}