using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Database;
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

        public bool CheckCanMerge(FoodData food, int lank, int count) {
            if (lank == 1) return count >= tier1Count;
            if (lank == 2) return count >= tier2Count;
            return false;
        }

        public async UniTask Merge(List<DeckItem> items) {
            List<DeckItem> target = items.OrderBy(x => x.GetIndex()).Take(3).ToList();
            for (int i = 1; i < target.Count; i++) {
                Transform image = target[i].transform.Find("Food");
                image.DOMove(target[0].transform.position, mergeDuration).SetEase(mergeEasing)
                    .OnComplete(() => {
                        image.localPosition = Vector3.zero;
                    });
            }
            await UniTask.Delay(TimeSpan.FromSeconds(mergeDuration));
            SoundPlayer.I.Play("se_merge");
            target[0].LankUp();
            for (int i = 1; i < target.Count; i++) {
                target[i].SetFood(null);
            }
        }

    }
}
