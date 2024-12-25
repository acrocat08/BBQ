using System;
using System.Collections.Generic;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.Database;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/ChoiceAllFood")]
    public class ChoiceAllFood : PlayAction {

        [SerializeField] private ItemSet itemSet;

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int x = v.GetNum(v.n1);
            int y = v.GetNum(v.n2);
            FoodData food = itemSet.GetRandomAllFood(x, y, v.s1);
            v.s1 = food.foodName;
        }
    }
}