using System;
using System.Collections.Generic;
using System.Linq;
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
                return;
            }
            
            if (env.isShopping) {
                List<DeckFood> newItems = deckFoods.Where(x => !env.inventory.GetDeckFoods().Contains(x)).ToList();
                if (newItems.Count == 0) return;
                foreach (DeckFood deckFood in newItems) {
                    env.inventory.AddFood(deckFood);
                }
                SoundMgr.SoundPlayer.I.Play("se_draw");
                await UniTask.Delay(TimeSpan.FromSeconds(duration));
                return;
            }
            
            SoundMgr.SoundPlayer.I.Play("se_draw");
            await env.deck.AddFoods(deckFoods[0].Releasable.ReleaseFoods(deckFoods));
        }
    }
}