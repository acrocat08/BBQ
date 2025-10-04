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
    [CreateAssetMenu(menuName = "Action/SetFork")]
    public class SetFork : PlayAction {
        
        [SerializeField] private ActionAssembly assembly;
        [SerializeField] private ItemSet itemSet;
        
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            env.inventory.SetFork();
            
            string foodName = v.GetString(v.n1);
            DeckFood invoker = v.GetFoods(v.n2)[0];
            if (invoker.isFrozen) return;
            if (invoker.isFired) return;
            if (foodName == "") return;
            FoodData targetFood = itemSet.SearchFood(foodName);
            List<ActionSequence> seq = targetFood.action.sequences.Where(x => x.trigger == ActionTrigger.Hit).ToList();
            foreach (ActionSequence sequence in seq) {
                ActionVariable result = await assembly.Run(sequence.condition, env, invoker, v.target);
                if (sequence.condition.Count == 0 || result.x1 > 0) {
                    await assembly.Run(sequence.commands, env, invoker, new() { invoker });
                }    
            }
            
            TriggerObserver.I.RemoveFood(invoker);
            await invoker.GetObject().Drop();
        }    
            
    }
}
