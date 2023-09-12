using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/Fire")]
    public class Fire : PlayAction {
        [SerializeField] private float duration;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            List<DeckFood> hittingFoods = env.board.GetHittingFoods();
            if (deckFoods.Count == 0) {
                return;
            }
            
            foreach (DeckFood deckFood in deckFoods) {
                if (deckFood.isFrozen || deckFood.isFired) continue;
                if (hittingFoods.Contains(deckFood)) {
                    LaneFood laneFood = env.board.FindLaneFood(deckFood);
                    laneFood.Fire();
                }
                else {
                    LaneFood laneFoods = deckFood.Release();
                    laneFoods.Fire();
                    laneFoods.Drop();
                    env.dump.AddFoods(new List<LaneFood> { laneFoods });
                }
            }
            SoundMgr.SoundPlayer.I.Play("se_fire");
            await UniTask.Delay(TimeSpan.FromSeconds(duration));

            await TriggerObserver.I.Invoke(ActionTrigger.Fire, deckFoods, true);
            await TriggerObserver.I.Invoke(ActionTrigger.FireOthers, deckFoods, false);
        }
    }
}