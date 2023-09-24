using System;
using BBQ.Common;
using BBQ.Cooking;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/AddHand")]
    public class AddHand : PlayAction {
        [SerializeField] private float duration;
        [SerializeField] private ParamUpEffectFactory effect;

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int num = v.GetNum(v.n1);
            env.handCount.Add(num);
            FoodObject pos = v.invoker?.GetObject();
            
            effect.Create("hand", num, pos);
            SoundMgr.SoundPlayer.I.Play("se_addHand");
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
    }
}