using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SwapNum")]
    public class SwapNum : PlayAction {

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int x = v.GetNum(v.n1);
            int y = v.GetNum(v.n2);
            v.SetNum(v.n1, y);
            v.SetNum(v.n2, x);
        }
    }
}