using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/NumEqual")]
    public class NumEqual : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int num1 = v.GetNum(v.n1);
            int num2 = v.GetNum(v.n2);
            v.x1 = num1 == num2 ? 1 : 0;
        }
    }
}