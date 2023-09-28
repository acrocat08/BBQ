using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/Draw")]
    public class Draw : PlayAction {
        [SerializeField] private int resetBorder;
        [SerializeField] private Reset reset;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int drawNum = v.GetNum(v.n1);
            int laneIndex = 0;
            if (v.n2 != "") laneIndex = v.GetNum(v.n2);
            
            if (!CheckDrawable(env, drawNum)) {
                await reset.Execute(env, v);
                return;
            }
            drawNum = Mathf.Min(drawNum, env.deck.SelectAll().Count);
            if(laneIndex == 0) drawNum = Mathf.Min(drawNum, 15 - env.board.SelectAll().Count);
            else drawNum = Mathf.Min(drawNum, 5 - env.board.SelectLane(laneIndex).Count);
            
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
        }

        bool CheckDrawable(ActionEnvironment env, int drawNum) {
            int deckNum = env.deck.SelectAll().Count;
            int boardNum = env.board.SelectAll().Count;
            if (deckNum > 0) return true;
            if (boardNum > resetBorder) return true;
            return false;
        }
    }
}
