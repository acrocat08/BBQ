using System;
using System.Collections.Generic;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/AddLife")]
    public class AddLife : PlayAction {
        [SerializeField] private float duration;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            env.shoppingGame.AddLife();
            SoundMgr.SoundPlayer.I.Play("se_addLife");
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
    }
}