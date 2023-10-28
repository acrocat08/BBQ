using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SelectTarget")]
    public class SelectTarget : PlayAction {
        [SerializeField] private DesignParam param;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            v.f1 = v.target.Where(x => x.data != param.resetFood).ToList();
        }
    }
}
