using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/HittingFoodIndex")]
    public class HittingFoodIndex : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            v.x1 = env.dump.GetHittingFoodLane(v.GetFoods(v.n1)[0]);
        }
    }
}