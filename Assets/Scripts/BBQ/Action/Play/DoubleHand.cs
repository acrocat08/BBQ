using System;
using BBQ.Common;
using BBQ.Cooking;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/DoubleHand")]
    public class DoubleHand : PlayAction {

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            if (env.isShopping) return;
            env.board.SetDouble();
        }
    }
}