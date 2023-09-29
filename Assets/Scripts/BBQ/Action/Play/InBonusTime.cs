using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/InBonusTime")]
    public class InBonusTime : PlayAction {

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            v.x1 = env.time.IsBonusMode() ? 1 : 0;
        }
    }
}