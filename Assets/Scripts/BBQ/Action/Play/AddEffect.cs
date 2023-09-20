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
    [CreateAssetMenu(menuName = "Action/AddEffect")]
    public class AddEffect : PlayAction {
        [SerializeField] private float duration;
        [SerializeField] private List<FoodEffect> effectList;
        [SerializeField] private ActionAssembly assembly;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            string effectName = v.GetString(v.n2);
            FoodEffect effect = null;
            if (effectName != "none") effect = effectList.First(x => x.effectName == effectName);
            List<DeckFood> target = deckFoods.Where(x => x.effect != effect).ToList();

            if (target.Count == 0) {
                await UniTask.Delay(TimeSpan.FromSeconds(duration));
                return;
            }
            
            foreach (DeckFood deckFood in deckFoods) {
                TriggerObserver.I.UpdateEffect(deckFood, deckFood.effect, effect);
                if (deckFood.effect != null) await assembly.Run(deckFood.effect.onReleased, env, deckFood, v.target);
                if (effect != null) await assembly.Run(effect.onAttached, env, deckFood, v.target);
                deckFood.effect = effect;
                FoodObject foodObject = env.board.FindFoodObject(deckFood);
                foodObject.SetEffect();
            }

            if (effect != null) {
                SoundMgr.SoundPlayer.I.Play("se_foodEffect");
            }
            else SoundMgr.SoundPlayer.I.Play("se_removeEffect");

            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }

        //TODO:別クラスに移行
        public FoodEffect GetEffect(string effectName) {
            FoodEffect effect = null;
            if (effectName != "none") effect = effectList.First(x => x.effectName == effectName);
            return effect;
        }
    }
}