using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.PlayData;
using BBQ.Shopping;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SoundMgr;
using UnityEngine;

namespace BBQ.Cooking {
    public class FoodSearch : MonoBehaviour {

        [SerializeField] private Board board;

        private bool _isActive;

        void Update() {
            if (!_isActive) return;
            if (Input.GetMouseButtonDown(0)) {
                DeckFood food = GetTargetFood();
                if(food != null) transform.Find("FoodDetail")
                    .GetComponent<ItemDetail>().DrawDetail(food);
            }
        }

        private DeckFood GetTargetFood() {
            return board.GetFoodObjects()
                .Where(x => x != null)
                .FirstOrDefault(x => (x.transform.position - Input.mousePosition).magnitude <= 100f)
                ?.deckFood;
        }

        public void Open() {
            InputGuard.Lock();
            board.Pause();
            transform.localScale = Vector3.one;
            CanvasGroup cg = GetComponent<CanvasGroup>();
            DOTween.To(
                () => cg.alpha,
                x => cg.alpha = x,
                1,
                0.25f
            );
            _isActive = true;
            AudioLowPassFilter filter = SoundPlayer.I.gameObject.AddComponent<AudioLowPassFilter>();
            filter.cutoffFrequency = 1500;
        }
        
        public async void Close() {
            _isActive = false;
            InputGuard.UnLock();
            board.Resume();
            CanvasGroup cg = GetComponent<CanvasGroup>();
            DOTween.To(
                () => cg.alpha,
                x => cg.alpha = x,
                0,
                0.25f
            );
            AudioLowPassFilter filter = SoundPlayer.I.gameObject.GetComponent<AudioLowPassFilter>();
            Destroy(filter);
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f));
            transform.localScale = Vector3.zero;

        }

    }
}
