using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SelectTag")]
    public class SelectTag : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            v.f1 = new List<DeckFood>(v.GetFoods(v.n1).Where(x => x.data.tag == v.GetString(v.n2)));
        }
    }
}