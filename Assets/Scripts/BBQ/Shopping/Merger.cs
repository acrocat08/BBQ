using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool CheckCanMerge(FoodData food, int lank, int count) {
            if (food == null) return false;
            if (food.isToken) return false;
            if (lank == 1) return count >= tier1Count;
            if (lank == 2) return count >= tier2Count;
            return false;
        }

        public async UniTask Merge(List<InventoryFood> items, Shop shop) {
            List<InventoryFood> target = items.OrderBy(x => x.GetIndex()).Take(3).ToList();
            for (int i = 1; i < target.Count; i++) {
                Transform image = target[i].transform.Find("Image");
                image.DOMove(target[0].transform.position, mergeDuration).SetEase(mergeEasing)
                    .OnComplete(() => {
                        image.localPosition = Vector3.zero;
                    });
            }
            await UniTask.Delay(TimeSpan.FromSeconds(mergeDuration));
            SoundPlayer.I.Play("se_merge");
            target[0].LankUp();
            for (int i = 1; i < target.Count; i++) {
                DeckFood emptyFood = new DeckFood(null);
                target[i].SetFood(emptyFood);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(discoverDuration));
            FoodData discovered = itemSet.GetRandomItem(Mathf.Min(5, shop.GetShopLevel() + 1));
            await shop.AddItems(new List<FoodData> { discovered });
        }

    }
}
