using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/Unique")]
    public class Unique : PlayAction {

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> foods;
            if (env.isShopping) {
                foods = env.inventory.GetDeckFoods();
            }
            else {
                foods = env.board.SelectAll();
            }
            if (!foods.Contains(v.invoker)) {
                v.x1 = 0;
                return;
            }

            List<DeckFood> sameFoods = foods.Where(x => x.data == v.invoker.data).ToList();
            v.x1 = sameFoods.IndexOf(v.invoker) == 0 ? 1 : 0;
        }
    }
}