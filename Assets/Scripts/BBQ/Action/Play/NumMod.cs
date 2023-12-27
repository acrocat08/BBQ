using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/NumMod")]
    public class NumMod : PlayAction {

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int x = v.GetNum(v.n1);
            int y = v.GetNum(v.n2);
            v.x1 = x % y;
        }
    }
}