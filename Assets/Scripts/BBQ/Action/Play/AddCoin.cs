using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/AddCoin")]
    public class AddCoin : PlayAction {
        [SerializeField] private float duration;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int num = v.GetNum(v.n1);
            env.coin.Add(num);
            SoundMgr.SoundPlayer.I.Play("se_addCoin");
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
    }
}