using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/RandomFood")]
    public class RandomFood : PlayAction {
        [SerializeField] private ItemSet itemSet;

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int min = v.GetNum(v.n1);
            int max = v.GetNum(v.n2);
            v.s1 = itemSet.GetRandomFood(min, max).foodName;
        }
    }
}