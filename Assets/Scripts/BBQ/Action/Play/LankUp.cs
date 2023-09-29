using System;

using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/LankUp")]
    public class LankUp : PlayAction {
        [SerializeField] private float duration;

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            int dir = v.GetNum(v.n2);
            List<DeckFood> lankupFoods = deckFoods.Where(x => x.lank + dir >= 1 && x.lank + dir <= 3).ToList();
            foreach (DeckFood deckFood in lankupFoods) {
                deckFood.lank += dir;
                FoodObject foodObject = null;
                if (!env.isShopping && env.dump.GetObject(deckFood) != null) {
                    foodObject = env.dump.GetObject(deckFood);
                }
                else foodObject = deckFood.GetObject();
                foodObject.LankUp();
            }
            SoundMgr.SoundPlayer.I.Play("se_merge");
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            await TriggerObserver.I.Invoke(ActionTrigger.LankUp, lankupFoods, true);
            await TriggerObserver.I.Invoke(ActionTrigger.LankUpOthers, lankupFoods, false);
        }
    }
}