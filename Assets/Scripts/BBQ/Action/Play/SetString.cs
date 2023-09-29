using System;
using System.Collections.Generic;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SetString")]
    public class SetString : PlayAction {

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            v.s1 = v.GetString(v.n1);
        }

    }
}