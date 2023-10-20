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
                InvokeSet target = _dict[sequence.trigger].FirstOrDefault(x => x.invoker == food && x.sequence == sequence);
                if (target == null) continue;
                _dict[sequence.trigger].Remove(target);
            }
        }

        public List<InvokeSet> GetInvokers(ActionTrigger trigger) {
            if (!_dict.ContainsKey(trigger)) return new List<InvokeSet>();
            return _dict[trigger];
        }
        
        public async UniTask<bool> CheckCondition(InvokeSet set, List<DeckFood> target, bool isMyself) {
            if (isMyself && target.Contains(set.invoker)) {
                return await CheckInvokable(set, target);
            }
            if (!isMyself && !target.Contains(set.invoker)) {
                return await CheckInvokable(set, target);
            }
            return false;
        }

        async UniTask<bool> CheckInvokable(InvokeSet invokeSet, List<DeckFood> target) {
            if (invokeSet.sequence.condition.Count == 0) return true;
            ActionVariable result = await assembly.Run(invokeSet.sequence.condition, env, invokeSet.invoker, target);
            return result.x1 > 0;
        }

        public void Reset() {
            _dict =  new Dictionary<ActionTrigger, List<InvokeSet>>();
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
