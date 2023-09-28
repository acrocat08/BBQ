using System;

using System.Collections.Generic;
using BBQ.Common;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/LankUp")]
    public class LankUp : PlayAction {
        [SerializeField] private float duration;

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            int dir = v.GetNum(v.n2);
            foreach (DeckFood deckFood in deckFoods) {
                deckFood.lank = Mathf.Clamp(deckFood.lank + dir, 1, 3);
                FoodObject foodObject = null;
                if (!env.isShopping && env.dump.GetObject(deckFood) != null) {
                    foodObject = env.dump.GetObject(deckFood);
                }
                else foodObject = deckFood.GetObject();
                foodObject.LankUp();
            }
            SoundMgr.SoundPlayer.I.Play("se_merge");
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
    }
}