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
    [CreateAssetMenu(menuName = "Action/Reset")]
    public class Reset : PlayAction {

        [SerializeField] private Draw draw;
        [SerializeField] private int drawNum;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            if(env.isShopping) return;
            await TriggerObserver.I.Invoke(ActionTrigger.BeforeReset, new List<DeckFood>(), false);
            List<UniTask> tasks = new List<UniTask>();
            List<FoodObject> boardFoods = env.board.ReleaseFoods(env.board.SelectAll());
            tasks.Add(env.deck.AddFoods(boardFoods));
            List<FoodObject> dumpFoods = env.dump.ReleaseFoods(env.dump.SelectAll());
            tasks.Add(env.deck.AddFoods(dumpFoods));
            SoundMgr.SoundPlayer.I.Play("se_reset");
            await tasks;
            await TriggerObserver.I.Invoke(ActionTrigger.AfterReset, new List<DeckFood>(), false);

            int num = Mathf.Min(drawNum, env.deck.SelectAll().Count);
            v.n1 = num.ToString();
            await draw.Execute(env, v);
        }
    }
}