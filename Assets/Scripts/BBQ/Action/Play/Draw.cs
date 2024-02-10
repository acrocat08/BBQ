using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/Draw")]
    public class Draw : PlayAction {

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            if (env.isShopping) return;
            int drawNum = v.GetNum(v.n1);
            int laneIndex = 0;
            if (v.n2 != "") laneIndex = v.GetNum(v.n2);
            
            drawNum = Mathf.Min(drawNum, env.deck.SelectAll().Count);
            if(laneIndex == 0) drawNum = Mathf.Min(drawNum, 15 - env.board.SelectAll().Count);
            else drawNum = Mathf.Min(drawNum, 5 - env.board.GetFoodNum(laneIndex));
            
            if (drawNum <= 0) {
                return;
            }
            List<FoodObject> taken = env.deck.TakeFood(drawNum);
            SoundMgr.SoundPlayer.I.Play("se_draw");
            if (laneIndex == 0) await env.board.AddFoodsRandomly(taken);
            else await env.board.AddFoodsRandomly(taken, laneIndex);
            List<DeckFood> deckFoods = taken.Select(x => x.deckFood).ToList();
            await TriggerObserver.I.Invoke(ActionTrigger.Draw, deckFoods, true);
            await TriggerObserver.I.Invoke(ActionTrigger.DrawOthers, deckFoods, false);
            await TriggerObserver.I.Invoke(ActionTrigger.Placed, deckFoods, true);
            await TriggerObserver.I.Invoke(ActionTrigger.PlacedOthers, deckFoods, false);

            if (!env.isShopping && env.deck.SelectAll().Count == 0 
                                 && env.board.SelectAll().Count < 15 && !env.board.HasResetEgg() 
                                 && (!env.dump.HasResetEgg() || env.resetFlag)) {
                await env.board.ResetEgg();
            }
            
        }
        
    }
}
