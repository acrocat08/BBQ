using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/ChangeLaneSpeed")]
    public class ChangeLaneSpeed : PlayAction {
        [SerializeField] private float duration;
        [SerializeField] private ParamUpEffectFactory effect;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            if (env.isShopping) return;
            int index = v.GetNum(v.n1);
            if (v.n2 != "") {
                int limit = v.GetNum(v.n2);
                env.loop.SetSpeed(index - 1, 1, limit);
            }
            else env.loop.SetSpeed(index - 1, 1);

            SoundMgr.SoundPlayer.I.Play("se_laneSpeed");
            effect.Create("lane" + index, 1, v.invoker?.GetObject());
            
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
    }
}