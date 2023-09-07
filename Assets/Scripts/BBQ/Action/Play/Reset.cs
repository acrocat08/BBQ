using System.Collections.Generic;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/Reset")]
    public class Reset : PlayAction {

        [SerializeField] private Draw draw;
        [SerializeField] private int drawNum;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<UniTask> tasks = new List<UniTask>();
            List<LaneFood> boardFoods = env.board.ReleaseFoods(env.board.SelectAll());
            tasks.Add(env.deck.AddFoods(boardFoods));
            List<LaneFood> dumpFoods = env.dump.ReleaseFoods(env.dump.SelectAll());
            tasks.Add(env.deck.AddFoods(dumpFoods));
            SoundMgr.SoundPlayer.I.Play("se_reset");
            await tasks;
            v.n1 = drawNum.ToString();
            await draw.Execute(env, v);
        }
    }
}
