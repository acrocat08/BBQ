using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/Draw")]
    public class Draw : PlayAction {
        [SerializeField] private int resetBorder;
        [SerializeField] private Reset reset;
        [SerializeField] private float nullDrawDuration;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int drawNum = ReadVariable(v.n1);
            if (!CheckDrawable(env, drawNum)) {
               await reset.Execute(env, v);
               return;
            }

            drawNum = Mathf.Min(drawNum, env.deck.SelectAll().Count);
            if (drawNum == 0) {
                await UniTask.Delay(TimeSpan.FromSeconds(nullDrawDuration));
                return;
            }
            List<LaneFood> taken = env.deck.TakeFood(drawNum);
            SoundMgr.SoundPlayer.I.Play("se_draw");
            await env.board.AddFoodsRandomly(taken);
        }

        bool CheckDrawable(ActionEnvironment env, int drawNum) {
            int deckNum = env.deck.SelectAll().Count;
            int boardNum = env.board.SelectAll().Count;
            if (deckNum >= drawNum) return true;
            if (boardNum > resetBorder) return true;
            return false;
        }
    }
}
