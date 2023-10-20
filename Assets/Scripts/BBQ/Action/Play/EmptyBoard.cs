using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/EmptyBoard")]
    public class EmptyBoard : PlayAction {

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int empty = 0;
            if (env.isShopping) {
                empty = 20 - env.inventory.GetDeckFoods().Count;
                if (empty >= v.GetNum(v.n1)) {
                    v.x1 = 1;
                }
                else v.x1 = 0;
                return;
            }
            empty = 15 - env.board.SelectAll(true).Count;
            if (empty >= v.GetNum(v.n1)) {
                v.x1 = 1;
            }
            else v.x1 = 0;
        }
    }
}