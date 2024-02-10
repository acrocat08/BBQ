using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/CopyFoods")]
    public class CopyFoods : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            if (deckFoods.Count == 0) return;
            int num = v.GetNum(v.n2);
            if (!env.isShopping && env.dump.GetObject(v.invoker) != null) {
                env.copyArea.SetPosition(env.dump.GetObject(v.invoker));
            }
            else env.copyArea.SetPosition(v.invoker.GetObject());
            List<DeckFood> copied = new List<DeckFood>();
            for (int i = 0; i < num; i++) {
                copied.Add(env.copyArea.CopyFoods(deckFoods)[0]);
            }
            v.f1 = new List<DeckFood>(copied);
        }
    }
}