using System;
using BBQ.Common;
using BBQ.Cooking;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/AddTime")]
    public class AddTime : PlayAction {
        [SerializeField] private float duration;
        [SerializeField] private ParamUpEffectFactory effect;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            if (env.isShopping) return;
            int num = v.GetNum(v.n1);
            if (env.time.IsBonusMode() && num > 0) return;
            if (num < 0) {
                env.time.UseTime(-num);
                SoundPlayer.I.Play("se_consume");
            }
            else {
                env.time.AddTime(num);
                SoundPlayer.I.Play("se_addTime");
            }
            
            effect.Create("time", num, v.invoker?.GetObject());
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
    }
}