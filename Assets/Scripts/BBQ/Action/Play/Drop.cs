using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/Drop")]
    public class Drop : PlayAction {
        [SerializeField] private float duration;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            if (deckFoods.Count == 0) {
                return;
            }

            if (env.isShopping) {
                deckFoods = deckFoods.Where(x => x.isFired).ToList();
                if (deckFoods.Count == 0) return;
                foreach (DeckFood deckFood in deckFoods) {
                    TriggerObserver.I.RemoveFood(deckFood);
                    deckFood.GetObject().Drop();
                }    
            }
            else env.dump.AddFoods(deckFoods[0].Releasable.ReleaseFoods(deckFoods));
            SoundMgr.SoundPlayer.I.Play("se_drop");
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            await TriggerObserver.I.Invoke(ActionTrigger.Drop, deckFoods, true);
            await TriggerObserver.I.Invoke(ActionTrigger.DropOthers, deckFoods, false);
        }
    }
}