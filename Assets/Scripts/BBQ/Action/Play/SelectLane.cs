using System.Collections.Generic;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SelectLane")]
    public class SelectLane : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            if (env.isShopping) return;
            int num = v.GetNum(v.n1);
            v.f1 = new List<DeckFood>(env.board.SelectLane(num));
        }
    }
}