using System;
using System.Collections.Generic;
using BBQ.Cooking;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action {
    public class TriggerObserver : MonoBehaviour {
        [SerializeField] private ActionAssembly assembly;
        [SerializeField] private ActionRegister register;
        [SerializeField] private ActionEnvironment env;

        public static TriggerObserver I;

        private void Awake() {
            if (I == null) {
                I = this;
            }
        }

        public async UniTask Invoke(ActionTrigger trigger, List<DeckFood> target, bool isMyself) {
            List<InvokeSet> invokeSets = await register.GetInvokers(trigger, target, isMyself);
            foreach (InvokeSet invokeSet in invokeSets) {
                //if (invokeSet.invoker.isFrozen) continue;
                LaneFood laneFood = env.board.FindLaneFood(invokeSet.invoker);
                if(laneFood != null) laneFood.OnInvoke();
                await assembly.Run(invokeSet.sequence.commands, env, invokeSet.invoker, target);
            }
        }

        public void RegisterFood(DeckFood deckFood) {
            register.Add(deckFood, deckFood.data.action.sequences);
            if(deckFood.effect != null) register.Add(deckFood, deckFood.effect.action.sequences);
        }

        public void UpdateEffect(DeckFood deckFood, FoodEffect prev, FoodEffect after) {
            if(prev != null) register.Remove(deckFood, prev.action.sequences);
            if(after != null) register.Add(deckFood, after.action.sequences);
        }
    }

    
}
