using System.Collections.Generic;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action {
    public class PlayAction : ScriptableObject {

        public virtual async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            
        }

        public async UniTask Execute(ActionEnvironment env) {
            ActionVariable v = new ActionVariable(null, null);
            await Execute(env, v);
        }


        protected int ReadVariable(string str) {
            if (str == null) Debug.LogWarning("値が設定されていません。"); 
            return int.Parse(str);
        }


    }
}
