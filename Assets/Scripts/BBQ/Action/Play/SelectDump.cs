using System.Collections.Generic;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SelectDump")]
    public class SelectDump : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            v.f1 = new List<DeckFood>(env.dump.SelectAll());
        }
    }
}