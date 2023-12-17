using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/ResetFlag")]
    public class ResetFlag : PlayAction {

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            env.resetFlag = true;
        }
    }
}