using System;
using BBQ.Common;
using BBQ.Cooking;
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
            SoundMgr.SoundPlayer.I.Play("se_addCarbon");
            effect.Create("carbon", num, v.invoker?.GetObject());
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
    }
}