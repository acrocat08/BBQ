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
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace BBQ.Action {
    public class TriggerObserver : MonoBehaviour {
        [SerializeField] private ActionAssembly assembly;
        [SerializeField] private ActionRegister register;
        [SerializeField] private ActionEnvironment env;
        [SerializeField] private Image invokerImage;

        public static TriggerObserver I;
        private Stack<FoodData> _invokerStack;
        
        private void Awake() {
            if (I == null) {
                I = this;
            }
            _invokerStack = new Stack<FoodData>();
        }

        public async UniTask Invoke(ActionTrigger trigger, List<DeckFood> target, bool isMyself) {
            List<InvokeSet> invokeSets = register.GetInvokers(trigger);
            List<InvokeSet> tmp = invokeSets.OrderBy(x => isMyself ? target.IndexOf(x.invoker) : x.sequence.priority).ToList();
            foreach (InvokeSet invokeSet in tmp) {
                bool isOk = await register.CheckCondition(invokeSet, target, isMyself);
                if(!isOk) continue;
                FoodObject food = invokeSet.invoker.GetObject();
                if(food != null) food.OnInvoke();
                _invokerStack.Push(invokeSet.invoker.data);
                UpdateInvokerImage();
                await assembly.Run(invokeSet.sequence.commands, env, invokeSet.invoker, target);
                _invokerStack.Pop();
                UpdateInvokerImage();
            }

        }

        private void UpdateInvokerImage() {
            if (invokerImage == null) return;
            if (_invokerStack.Count == 0) {
                invokerImage.enabled = false;
            }
            else {
                invokerImage.enabled = true;
                FoodData top = _invokerStack.Pop();
                invokerImage.sprite = top.foodImage;
                _invokerStack.Push(top);
            }
        }

        public void RegisterFood(DeckFood deckFood) {
            if (deckFood == null || deckFood.data == null) return;
            register.Add(deckFood, deckFood.data.action.sequences);
            if(deckFood.effect != null) register.Add(deckFood, deckFood.effect.action.sequences);
        }

        public void RemoveFood(DeckFood deckFood) {
            if (deckFood == null || deckFood.data == null) return;
            register.Remove(deckFood, deckFood.data.action.sequences);
            if(deckFood.effect != null) register.Remove(deckFood, deckFood.effect.action.sequences);
        }

        public void UpdateEffect(DeckFood deckFood, FoodEffect prev, FoodEffect after) {
            if(prev != null) register.Remove(deckFood, prev.action.sequences);
            if(after != null) register.Add(deckFood, after.action.sequences);
        }

        public void Reset() {
            register.Reset();
        }
    }

    
}
