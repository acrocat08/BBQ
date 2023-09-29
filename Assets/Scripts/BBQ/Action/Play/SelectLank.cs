using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SelectLank")]
    public class SelectLank : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int min = v.GetNum(v.n1);
            int max = v.GetNum(v.n2);
            v.f1 = new List<DeckFood>(v.f1.Where(x => x.lank >= min && x.lank <= max).ToList());
        }
    }
}