using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SwapString")]
    public class SwapString : PlayAction {

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            string x = v.GetString(v.n1);
            string y = v.GetString(v.n2);
            v.SetString(v.n1, y);
            v.SetString(v.n2, x);
            if (v.invoker.GetObject()) {
                v.invoker.GetObject().UpdateMemory();
            }
        }
    }
}