using System;
using BBQ.Common;
using BBQ.Cooking;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SetPig")]
    public class SetPig : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            env.inventory.SetPigFlag();
        }
    }
}