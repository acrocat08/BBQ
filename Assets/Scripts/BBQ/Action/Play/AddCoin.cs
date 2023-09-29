using System;
using System.Collections.Generic;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/AddCoin")]
    public class AddCoin : PlayAction {
        [SerializeField] private float duration;
        [SerializeField] private ParamUpEffectFactory effect;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int num = v.GetNum(v.n1);
            env.coin.Add(num);
            SoundMgr.SoundPlayer.I.Play(num >= 0 ? "se_addCoin" : "se_consume");
            effect.Create("coin", num, v.invoker?.GetObject());
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            if(num > 0) await TriggerObserver.I.Invoke(ActionTrigger.GainCoin, new List<DeckFood>(), false);
        }
    }
}