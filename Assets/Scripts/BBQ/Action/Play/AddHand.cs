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

            effect.Create("hand", num, env.board.FindLaneFood(v.invoker));
            SoundMgr.SoundPlayer.I.Play("se_addHand");
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
    }
}