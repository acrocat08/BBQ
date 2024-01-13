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
            deckFoods = deckFoods.Where(x => !(v.invoker.isFrozen || v.invoker.isFired)).ToList();
            if (deckFoods.Count == 0) return;

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

            int num = Mathf.Min(deckFoods.Count, 15 - env.board.SelectAll().Count);
            
            if (deckFoods.Count == 0 || num == 0) {
                return;
            }
            SoundMgr.SoundPlayer.I.Play("se_draw");
            List<DeckFood> target = deckFoods.Take(num).ToList();
            List<DeckFood> drawFoods = target.Where(x => x.Releasable == env.deck).ToList();
            await env.board.AddFoodsRandomly(target[0].Releasable.ReleaseFoods(deckFoods));

            
            await TriggerObserver.I.Invoke(ActionTrigger.Draw, drawFoods, true);
            await TriggerObserver.I.Invoke(ActionTrigger.DrawOthers, drawFoods, false);
            await TriggerObserver.I.Invoke(ActionTrigger.Placed, target, true);
            await TriggerObserver.I.Invoke(ActionTrigger.PlacedOthers, target, false);
            
            if (!env.isShopping && env.deck.SelectAll().Count == 0 
                                && env.board.SelectAll().Count < 15 && !env.board.HasResetEgg()) {
                await env.board.ResetEgg();
            }
        }
    }
}