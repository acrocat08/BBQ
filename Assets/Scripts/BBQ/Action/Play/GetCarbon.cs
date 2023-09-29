using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/GetCarbon")]
    public class GetCarbon : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            v.x1 = env.carbon.GetCarbon();
        }
    }
}