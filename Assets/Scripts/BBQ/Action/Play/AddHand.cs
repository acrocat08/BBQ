using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/AddHand")]
    public class AddHand : PlayAction {
        [SerializeField] private float duration;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int num = v.GetNum(v.n1);
            env.handCount.Add(num);
            SoundMgr.SoundPlayer.I.Play("se_addHand");
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
    }
}