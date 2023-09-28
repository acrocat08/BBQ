using System;
using BBQ.Common;
using BBQ.Cooking;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/GoldenHand")]
    public class GoldenHand : PlayAction {

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            env.board.SetGold();
        }
    }
}