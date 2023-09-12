using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Cooking;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace BBQ.Action {
    public class ActionRegister : MonoBehaviour {

        private Dictionary<ActionTrigger, List<InvokeSet>> _dict =  new Dictionary<ActionTrigger, List<InvokeSet>>();
        [SerializeField] private ActionEnvironment env;
        [SerializeField] private ActionAssembly assembly;
        
        public void Add(DeckFood food, List<ActionSequence> sequences) {
            foreach (ActionSequence sequence in sequences) {
                ActionTrigger trigger = sequence.trigger;
                if (!_dict.ContainsKey(trigger)) _dict[trigger] = new List<InvokeSet>();
                _dict[trigger].Add(new InvokeSet(food, sequence));
            }
        }
        
        public void Remove(DeckFood food, List<ActionSequence> sequences) {
            foreach (ActionSequence sequence in sequences) {
                InvokeSet target = _dict[sequence.trigger].First(x => x.invoker == food && x.sequence == sequence);
                _dict[sequence.trigger].Remove(target);
            }
        }

        public async UniTask<List<InvokeSet>> GetInvokers(ActionTrigger trigger, List<DeckFood> target, bool isMyself) {
            if (!_dict.ContainsKey(trigger)) return new List<InvokeSet>();
            List<InvokeSet> candidates = _dict[trigger];
            List<InvokeSet> ret = new List<InvokeSet>();
            if (isMyself) {
                foreach (DeckFood food in target) {
                    foreach (InvokeSet c in candidates.Where(x => x.invoker == food)) {
                        bool isOk = await CheckInvokable(c, target);
                        if(isOk) ret.Add(c);
                    }
                }
            }
            else {
                foreach (InvokeSet invokeSet in candidates.OrderBy(x => x.sequence.priority)) {
                    if (target.Contains(invokeSet.invoker)) continue;
                    bool isOk = await CheckInvokable(invokeSet, target);
                    if(isOk) ret.Add(invokeSet);
                }
            }
            return ret;
        }

        async UniTask<bool> CheckInvokable(InvokeSet invokeSet, List<DeckFood> target) {
            if (invokeSet.sequence.condition.Count == 0) return true;
            ActionVariable result = await assembly.Run(invokeSet.sequence.condition, env, invokeSet.invoker, target);
            return result.x1 > 0;
        }
    }

    [Serializable]
    public class InvokeSet {
        public DeckFood invoker;
        public ActionSequence sequence;

        public InvokeSet(DeckFood invoker, ActionSequence sequence) {
            this.invoker = invoker;
            this.sequence = sequence;
        }
    }

}
