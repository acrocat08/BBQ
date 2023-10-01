using System.Collections.Generic;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace BBQ.Action {
    [CreateAssetMenu(menuName = "ActionAssembly")]
    public class ActionAssembly : ScriptableObject {

        public async UniTask<ActionVariable> Run(List<ActionCommand> commands, ActionEnvironment env, DeckFood invoker, List<DeckFood> target) {
            ActionVariable v = new ActionVariable(invoker, target);
            for (int i = 0; i < commands.Count; i++) {
                v.n1 = commands[i].n1;
                v.n2 = commands[i].n2;
                await commands[i].action.Execute(env, v);
            }
            if(!env.isShopping) env.board.StoreHand();
            return v;
        }
    }
    


}
