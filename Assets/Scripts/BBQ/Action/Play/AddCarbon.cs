using System;
using System.Collections.Generic;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/AddCarbon")]
    public class AddCarbon : PlayAction {
        [SerializeField] private float duration;
        [SerializeField] private ParamUpEffectFactory effect;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int num = v.GetNum(v.n1);
            env.carbon.Add(num);
            SoundMgr.SoundPlayer.I.Play(num >= 0 ? "se_addCarbon" : "se_consume");
            effect.Create("carbon", num, v.invoker?.GetObject());
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            if(num < 0 && v.GetNum(v.n2) == 0) await TriggerObserver.I.Invoke(ActionTrigger.UseCarbon, new List<DeckFood>(), false);
        }
    }
}