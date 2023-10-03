using System;
using BBQ.Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/GainRerollTicket")]
    public class GainRerollTicket : PlayAction {
        [SerializeField] private float duration;
        [SerializeField] private ParamUpEffectFactory effect;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int num = v.GetNum(v.n1);
            SoundMgr.SoundPlayer.I.Play("se_rerollTicket");
            effect.Create("reroll", num, v.invoker?.GetObject());
            if (env.isShopping) {
                env.shop.GainRerollTicket(num);
                await UniTask.Delay(TimeSpan.FromSeconds(duration));

                return;
            }
            env.rerollTicket += num;
            await UniTask.Delay(TimeSpan.FromSeconds(duration));

        }
    }
}