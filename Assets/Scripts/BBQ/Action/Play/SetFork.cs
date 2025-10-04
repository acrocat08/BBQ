using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.Database;
using BBQ.PlayData;
using BBQ.Shopping;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/SetFork")]
    public class SetFork : PlayAction {
        
        [SerializeField] private ActionAssembly assembly;
        [SerializeField] private ItemSet itemSet;
        
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            //env.inventory.SetFork();
            
            string foodName = v.GetString(v.n1);
            DeckFood invoker = v.GetFoods(v.n2)[0];
            if (invoker.isFrozen) return;
            if (invoker.isFired) return;
            if (foodName == "") return;
            
            invoker.GetObject().Hit();
            SoundMgr.SoundPlayer.I.Play("se_hit1");
            SoundMgr.SoundPlayer.I.Play("se_hit2-2");
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            FoodData targetFood = itemSet.SearchFood(foodName);
            List<ActionSequence> seq = targetFood.action.sequences.Where(x => x.trigger == ActionTrigger.Hit).ToList();
            foreach (ActionSequence sequence in seq) {
                ActionVariable result = await assembly.Run(sequence.condition, env, invoker, v.target);
                if (sequence.condition.Count == 0 || result.x1 > 0) {
                    await assembly.Run(sequence.commands, env, invoker, new() { invoker });
                }    
            }
            if (invoker.isFired) return;
            TriggerObserver.I.RemoveFood(invoker);
            SoundMgr.SoundPlayer.I.Play("se_drop");
            await ((InventoryFood)invoker.GetObject()).ForkDrop();
        }    
            
    }
}
