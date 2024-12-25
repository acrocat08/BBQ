using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SelectDump")]
    public class SelectDump : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            if(env.isShopping) v.f1 = new(env.inventory.GetDeckFoods().Where(x => !x.isFrozen && !x.isFired));
            else v.f1 = new(env.dump.SelectAll().Where(x => !env.dump.GetHittingFoods().Contains(x)));
        }
    }
}