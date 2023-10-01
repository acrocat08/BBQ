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
        [SerializeField] private ActionAssembly assembly;
        [SerializeField] private ItemSet itemSet;
        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            List<DeckFood> deckFoods = v.GetFoods(v.n1);
            string effectName = v.GetString(v.n2);
            FoodEffect effect = null;
            if (!effectName.Contains("none")) effect = itemSet.effects.First(x => x.effectName == effectName);
            List<DeckFood> target = deckFoods.Where(x => x.effect != effect).ToList();

            if (target.Count == 0) {
                await UniTask.Delay(TimeSpan.FromSeconds(duration));
                return;
            }
            
            foreach (DeckFood deckFood in deckFoods) {
                if(effectName == "none*") env.deck.RemoveEffect(deckFood);
                if (deckFood.effect == null && effect == null) continue;
                TriggerObserver.I.UpdateEffect(deckFood, deckFood.effect, effect);
                if (deckFood.effect != null) await assembly.Run(deckFood.effect.onReleased, env, deckFood, v.target);
                if (effect != null) await assembly.Run(effect.onAttached, env, deckFood, v.target);
                deckFood.effect = effect;
                FoodObject foodObject = deckFood.GetObject();
                foodObject.SetEffect();
            }

            if (effect != null) {
                SoundMgr.SoundPlayer.I.Play("se_foodEffect");
            }
            else SoundMgr.SoundPlayer.I.Play("se_removeEffect");

            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
        
    }
}