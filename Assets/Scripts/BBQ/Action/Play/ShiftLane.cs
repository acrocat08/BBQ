using System;
using System.Collections.Generic;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/ShiftLane")]
    public class ShiftLane : PlayAction {
        [SerializeField] private float duration;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            int dir = v.GetNum(v.n2);
            List<UniTask> tasks = new List<UniTask>();
            foreach (DeckFood deckFood in deckFoods) {
                int index = env.board.GetLaneIndex(deckFood);
                int nextIndex = (index + 2 + dir) % 3 + 1;
                if (env.board.SelectLane(nextIndex).Count == 5) continue;
                LaneFood laneFood = deckFood.Release();
                tasks.Add(env.board.AddFoodsRandomly(new List<LaneFood> { laneFood }, nextIndex));
            }

            if (tasks.Count == 0) {
                await UniTask.Delay(TimeSpan.FromSeconds(duration));
                return;
            }
            
            SoundMgr.SoundPlayer.I.Play("se_move");
            await tasks;
        }
    }
}