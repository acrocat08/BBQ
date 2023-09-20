using System;
using System.Collections.Generic;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/MoveLane")]
    public class MoveLane : PlayAction {
        [SerializeField] private float duration;

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            int index = v.GetNum(v.n2);
            List<UniTask> tasks = new List<UniTask>();
            foreach (DeckFood deckFood in deckFoods) {
                if (env.board.SelectLane(index).Count == 5) continue;
                FoodObject laneFood = deckFood.Release();
                tasks.Add(env.board.AddFoodsRandomly(new List<FoodObject> { laneFood }, index));
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