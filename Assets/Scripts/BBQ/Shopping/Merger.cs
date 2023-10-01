using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Action;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SoundMgr;
using UnityEngine;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "Merge")]
    public class Merger : ScriptableObject {

        [SerializeField] private int tier1Count;
        [SerializeField] private int tier2Count;
        [SerializeField] private float mergeDuration;
        [SerializeField] private Ease mergeEasing;
        [SerializeField] private float discoverDuration;
        [SerializeField] private ItemSet itemSet;
        [SerializeField] private ActionAssembly assembly;
        public bool CheckCanMerge(FoodData food, int lank, int count) {
            if (food == null) return false;
            if (food.isToken) return false;
            if (lank == 1) return count >= tier1Count;
            if (lank == 2) return count >= tier2Count;
            return false;
        }

        public async UniTask Merge(List<InventoryFood> items, Shop shop) {
            InputGuard.Lock();            
            List<InventoryFood> target = items.OrderBy(x => x.GetIndex()).Take(3).ToList();
            for (int i = 1; i < target.Count; i++) {
                Transform image = target[i].transform.Find("Object").Find("Image");
                image.DOMove(target[0].transform.position, mergeDuration).SetEase(mergeEasing)
                    .OnComplete(() => {
                        image.localPosition = Vector3.zero;
                    });
            }
            await UniTask.Delay(TimeSpan.FromSeconds(mergeDuration));
            SoundPlayer.I.Play("se_merge");
            target[0].deckFood.lank += 1;
            target[0].LankUp();
            FoodEffect effect = target.Select(x => x.deckFood.effect).Where(x => x != null).OrderBy(x => Guid.NewGuid())
                .FirstOrDefault();
            if (target[0].deckFood.effect != null) await assembly.Run(target[0].deckFood.effect.onReleased, null, target[0].deckFood, null);
            if (effect != null) await assembly.Run(effect.onAttached, null, target[0].deckFood, null);
            target[0].deckFood.effect = effect;
            target[0].SetEffect();
            for (int i = 1; i < target.Count; i++) {
                TriggerObserver.I.RemoveFood(target[i].deckFood);
                DeckFood emptyFood = new DeckFood(null);
                target[i].SetFood(emptyFood);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(discoverDuration));
            await TriggerObserver.I.Invoke(ActionTrigger.LankUp, new List<DeckFood> { target[0].deckFood }, true);
            await TriggerObserver.I.Invoke(ActionTrigger.LankUpOthers, new List<DeckFood> { target[0].deckFood }, false);
            int discoverTier = Mathf.Min(5, shop.GetShopLevel() + 1);
            FoodData discovered = itemSet.GetRandomFood(discoverTier, discoverTier);
            await shop.AddFoods(new List<FoodData> { discovered }, false);
            InputGuard.UnLock();            
        }

    }
}
