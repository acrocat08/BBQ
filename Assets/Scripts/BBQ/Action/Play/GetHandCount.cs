using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/GetHandCount")]
    public class GetHandCount : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            v.x1 = env.handCount.GetHandCount();
        }
    }
}