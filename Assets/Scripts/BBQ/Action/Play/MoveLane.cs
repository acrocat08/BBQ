using System.Collections.Generic;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/MoveLane")]
    public class MoveLane : PlayAction {
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            int dir = v.GetNum(v.n2);
            List<UniTask> tasks = new List<UniTask>();
            foreach (DeckFood deckFood in deckFoods) {
                int index = env.board.GetLaneIndex(deckFood);
                int nextIndex = (index + 3 + dir) % 3;
                LaneFood laneFood = deckFood.Release();
                tasks.Add(env.board.AddFoodsRandomly(new List<LaneFood> { laneFood }, nextIndex));
            }
            SoundMgr.SoundPlayer.I.Play("se_move");
            await tasks;
        }
    }
}