using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/GotoBoard")]
    public class GotoBoard : PlayAction {
        [SerializeField] private float duration;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);

            int num = Mathf.Min(deckFoods.Count, 15 - env.board.SelectAll().Count);
            
            if (deckFoods.Count == 0) {
                return;
            }
            SoundMgr.SoundPlayer.I.Play("se_draw");
            List<DeckFood> target = deckFoods.Take(num).ToList();
            await env.board.AddFoodsRandomly(target[0].Releasable.ReleaseFoods(deckFoods));
        }
    }
}