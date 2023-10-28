using System.Collections.Generic;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SelectInvoker")]
    public class SelectInvoker : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            if (v.invoker == null) return;
            v.f1 = new List<DeckFood> { v.invoker };
        }
    }
}