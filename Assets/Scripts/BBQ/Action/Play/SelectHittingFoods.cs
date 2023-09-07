using System.Collections.Generic;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SelectHittingFoods")]
    public class SelectHittingFoods : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            v.f1 = new List<DeckFood>(env.board.GetHittingFoods());
        }
    }
}