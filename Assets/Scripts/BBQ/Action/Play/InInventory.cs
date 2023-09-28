using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/InInventory")]
    public class InInventory : PlayAction {

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            bool isOk = deckFoods.All(x => env.inventory.GetDeckFoods().Contains(x) && !x.isFrozen && !x.isFired);
            v.x1 = isOk ? 1 : 0;
        }
    }
}