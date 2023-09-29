using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/GetCoin")]
    public class GetCoin : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            v.x1 = env.coin.GetCoin();
        }
    }
}