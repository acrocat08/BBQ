using System;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/AddTime")]
    public class AddTime : PlayAction {
        [SerializeField] private float duration;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int num = v.GetNum(v.n1);
            if (num < 0) {
                env.time.UseTime(-num);
                SoundPlayer.I.Play("se_useTime");
            }
            else {
                env.time.AddTime(num);
                SoundPlayer.I.Play("se_addTime");
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
    }
}