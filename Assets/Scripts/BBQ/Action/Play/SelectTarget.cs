using System.Collections.Generic;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SelectTarget")]
    public class SelectTarget : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            v.f1 = new List<DeckFood>(v.target);
        }
    }
}
