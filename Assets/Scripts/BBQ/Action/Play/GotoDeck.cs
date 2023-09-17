using System;
using System.Collections.Generic;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/GotoDeck")]
    public class GotoDeck : PlayAction {
        [SerializeField] private float duration;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            if (deckFoods.Count == 0) {
                await UniTask.Delay(TimeSpan.FromSeconds(duration));
                return;
            }
            SoundMgr.SoundPlayer.I.Play("se_draw");
            await env.deck.AddFoods(deckFoods[0].Releasable.ReleaseFoods(deckFoods));
        }
    }
}