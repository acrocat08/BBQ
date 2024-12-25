using System;
using System.Collections.Generic;
using System.Net;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.Database;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/AddShopFood")]
    public class AddShopFood : PlayAction {

        [SerializeField] private ItemSet itemSet;

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            if (!env.isShopping) return;
            FoodData food = itemSet.SearchFood(v.GetString(v.n1));
            SoundPlayer.I.Play("se_addShopFood");
            await env.shop.AddFoods(new() { food }, false);
        }
    }
}