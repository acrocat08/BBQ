using System;
using System.Collections.Generic;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.Database;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/ChoiceFood")]
    public class ChoiceFood : PlayAction {

        [SerializeField] private ItemSet itemSet;

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            FoodData food = itemSet.GetRandomFood(env.shop.GetShopLevel(), env.shop.GetShopLevel());
            v.s1 = food.foodName;
        }
    }
}