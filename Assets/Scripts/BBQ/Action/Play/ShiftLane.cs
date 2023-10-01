using System;
using System.Collections.Generic;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/ShiftLane")]
    public class ShiftLane : PlayAction {
        [SerializeField] private float duration;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            if (env.isShopping) return;
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            int dir = v.GetNum(v.n2);
            List<UniTask> tasks = new List<UniTask>();
            foreach (DeckFood deckFood in deckFoods) {
                int index = env.board.GetLaneIndex(deckFood);
                int nextIndex = (index + 2 + dir) % 3 + 1;
                if (env.board.GetFoodNum(nextIndex) == 5) continue;
                FoodObject laneFood = deckFood.Release();
                tasks.Add(env.board.AddFoodsRandomly(new List<FoodObject> { laneFood }, nextIndex));
            }

            if (tasks.Count == 0) {
                return;
            }
            
            SoundMgr.SoundPlayer.I.Play("se_move");
            await tasks;
            await TriggerObserver.I.Invoke(ActionTrigger.Placed, deckFoods, true);
            await TriggerObserver.I.Invoke(ActionTrigger.PlacedOthers, deckFoods, false);
        }
    }
}