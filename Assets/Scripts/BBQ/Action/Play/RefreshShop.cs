using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/RefreshShop")]
    public class RefreshShop : PlayAction {

        [SerializeField] private ItemSet itemSet;

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> foods = v.GetFoods(v.n1);
            SoundPlayer.I.Play("se_refresh");
            SoundPlayer.I.Play("se_reroll2");
            await env.shop.AddFoods(foods.Select(x => x.data).ToList(), true);
        }
    }
}