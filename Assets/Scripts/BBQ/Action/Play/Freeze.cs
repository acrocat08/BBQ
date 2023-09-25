using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/Freeze")]
    public class Freeze : PlayAction {
        [SerializeField] private float duration;
        [SerializeField] private Drop drop;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            List<DeckFood> hittingFoods = env.isShopping ? new List<DeckFood>() : env.dump.GetHittingFoods();

            deckFoods = deckFoods.Where(x => !x.isFrozen && !x.isFired).ToList();
            
            if (deckFoods.Count == 0) {
                return;
            }
            
            List<UniTask> tasks = new List<UniTask>();
            List<DeckFood> dropped = new List<DeckFood>();

            foreach (DeckFood deckFood in deckFoods) {
                tasks.Add(FreezeFood(env, v, deckFood));
                if (!hittingFoods.Contains(deckFood)) {
                    dropped.Add(deckFood);
                }
            }
            
            SoundMgr.SoundPlayer.I.Play("se_freeze");
            await tasks;
            await TriggerObserver.I.Invoke(ActionTrigger.Freeze, deckFoods, true);
            await TriggerObserver.I.Invoke(ActionTrigger.FreezeOthers, deckFoods, false);

            ActionVariable v2 = v.Copy("f1", "");
            v2.f1 = dropped;
            await drop.Execute(env, v2);

        }

        async UniTask FreezeFood(ActionEnvironment env, ActionVariable v, DeckFood deckFood) {
            FoodObject foodObject = null;
            if (!env.isShopping && env.dump.GetObject(deckFood) != null) {
                foodObject = env.dump.GetObject(deckFood);
            }
            else foodObject = deckFood.GetObject();
            foodObject.Freeze();
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
    }
    
    
}