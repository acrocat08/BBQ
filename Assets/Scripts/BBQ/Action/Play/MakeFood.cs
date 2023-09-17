using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/MakeFood")]
    public class MakeFood : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            string foodName = v.GetString(v.n1);
            List<DeckFood> deckFoods = env.copyArea.MakeFood(foodName);
            int num = v.GetNum(v.n2);
            env.copyArea.SetPosition(env.board.FindLaneFood(v.invoker));
            List<DeckFood> copied = new List<DeckFood>();
            for (int i = 0; i < num; i++) {
                copied.Add(env.copyArea.CopyFoods(deckFoods)[0]);
            }
            v.f1 = new List<DeckFood>(copied);
        }
    }
}