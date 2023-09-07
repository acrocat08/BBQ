using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/CountFoods")]
    public class CountFoods : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int num = v.GetFoods(v.n1).Count;
            v.x1 = num;
        }
    }
}
