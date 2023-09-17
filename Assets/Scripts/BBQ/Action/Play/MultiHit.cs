using System.Collections.Generic;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/MultiHit")]
    public class MultiHit : PlayAction {

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int hitCount = v.target.Count;
            v.x1 = hitCount > 1 ? 1 : 0;
        }
    }
}