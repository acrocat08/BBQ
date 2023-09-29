using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/DiscountShop")]
    public class DiscountShop : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            env.shop.DiscountFood(v.GetNum(v.n1));
        }
    }
}