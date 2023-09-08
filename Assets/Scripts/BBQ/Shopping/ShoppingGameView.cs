using System;
using System.Collections.Generic;
using BBQ.Cooking;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "ShoppingGame/View")]
    public class ShoppingGameView : ScriptableObject {
        
        [SerializeField] private float topBGMax;
        [SerializeField] private float bottomBGMax;
        [SerializeField] private float closeDuration;
        [SerializeField] private Ease closeEasing;
        [SerializeField] private float openDuration;
        [SerializeField] private Ease openEasing;
        [SerializeField] private Color cookingBGColor;
        [SerializeField] private float colorDuration;

        public void Init(ShoppingGame shoppingGame) {
            RectTransform topBG = shoppingGame.transform.Find("BG").Find("BG_T")
                .GetComponent<RectTransform>();
            RectTransform bottomBG = shoppingGame.transform.Find("BG").Find("BG_B")
                .GetComponent<RectTransform>();
            float width = topBG.sizeDelta.x;
            topBG.sizeDelta = new Vector2(width, topBGMax);
            bottomBG.sizeDelta = new Vector2(width, bottomBGMax);
        }
        
        public async UniTask OpenBG(ShoppingGame shoppingGame) {

            RectTransform topBG = shoppingGame.transform.Find("BG").Find("BG_T")
                .GetComponent<RectTransform>();
            RectTransform bottomBG = shoppingGame.transform.Find("BG").Find("BG_B")
                .GetComponent<RectTransform>();

            List<UniTask> tasks = new List<UniTask> {
                MoveBG(topBG, 0, openDuration, openEasing),
                MoveBG(bottomBG, 0, openDuration, openEasing)
            };
            await tasks;
        }
        public async UniTask CloseBG(ShoppingGame shoppingGame) {

            RectTransform topBG = shoppingGame.transform.Find("BG").Find("BG_T")
                .GetComponent<RectTransform>();
            RectTransform bottomBG = shoppingGame.transform.Find("BG").Find("BG_B")
                .GetComponent<RectTransform>();

            List<UniTask> tasks = new List<UniTask> {
                MoveBG(topBG, topBGMax, closeDuration, closeEasing),
                MoveBG(bottomBG, bottomBGMax, closeDuration, closeEasing)
            };
            await tasks;
        }

        public async UniTask ChangeColor(ShoppingGame shoppingGame) {
            Image topBG = shoppingGame.transform.Find("BG").Find("BG_T")
                .GetComponent<Image>();
            Image bottomBG = shoppingGame.transform.Find("BG").Find("BG_B")
                .GetComponent<Image>();
            topBG.DOColor(cookingBGColor, colorDuration).SetEase(Ease.Linear);
            bottomBG.DOColor(cookingBGColor, colorDuration).SetEase(Ease.Linear);
            await UniTask.Delay(TimeSpan.FromSeconds(closeDuration));
        }

        private async UniTask MoveBG(RectTransform BG, float toHeight, float duration, Ease ease) {
            float width = BG.sizeDelta.x;
            await DOTween.To(
                () => BG.sizeDelta,
                x => BG.sizeDelta = x,
                new Vector2(width, toHeight),
                duration).SetEase(ease);
        }
    }
}
