using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SetHelpActionPenalty")]
    public class SetHelpActionPenalty : PlayAction {

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int num = v.GetNum(v.n1);
            env.inventory.SetHelpPenaltyReduce(num);
        }

    }
}