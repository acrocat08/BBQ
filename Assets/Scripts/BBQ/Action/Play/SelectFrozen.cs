using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SelectFrozen")]
    public class SelectFrozen : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            if (env.isShopping) {
                v.f1 = new List<DeckFood>(env.inventory.GetDeckFoods().Where(x => x.isFrozen));
            }
            else {
                List<DeckFood> deckFoods = env.dump.SelectFrozen();
                v.f1 = deckFoods;
            }
        }
    }
}