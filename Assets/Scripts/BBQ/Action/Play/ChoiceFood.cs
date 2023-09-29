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
            int x = v.GetNum(v.n1);
            int y = v.GetNum(v.n2);
            FoodData food = itemSet.GetRandomFood(x, y, v.s1);
            v.s1 = food.foodName;
        }
    }
}