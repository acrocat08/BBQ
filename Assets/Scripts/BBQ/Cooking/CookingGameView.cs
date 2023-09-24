using System;
using System.Collections.Generic;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Cooking {
    [CreateAssetMenu(menuName = "CookingGame/View")]
    public class CookingGameView : ScriptableObject {

        [SerializeField] private float leftBGMin;
        [SerializeField] private float leftBGMax;
        [SerializeField] private float rightBGMin;
        [SerializeField] private float rightBGMax;
        [SerializeField] private float closeDuration;
        [SerializeField] private Ease closeEasing;
        [SerializeField] private float openDuration;
        [SerializeField] private Ease openEasing;
        [SerializeField] private Color shoppingBGColor;
        [SerializeField] private float colorDuration;
        [SerializeField] private float hideDuration;

        [SerializeField] private CookingResultView resultView;


        public void Init(CookingGame cookingGame) {
            RectTransform leftBG = cookingGame.transform.Find("Information").Find("BG_L").Find("BG")
                .GetComponent<RectTransform>();
            RectTransform rightBG = cookingGame.transform.Find("Information").Find("BG_R").Find("BG")
                .GetComponent<RectTransform>();
            float height = leftBG.sizeDelta.y;
            leftBG.sizeDelta = new Vector2(leftBGMax, height);
            rightBG.sizeDelta = new Vector2(rightBGMax, height);
            Text dayText = cookingGame.transform.Find("Information").Find("BG_L").Find("UI").Find("Day").Find("Text").GetComponent<Text>();
            dayText.text = "Day " + cookingGame.GetDay();
        }
        
        public async UniTask OpenBG(CookingGame cookingGame) {

            RectTransform leftBG = cookingGame.transform.Find("Information").Find("BG_L").Find("BG")
                .GetComponent<RectTransform>();
            RectTransform rightBG = cookingGame.transform.Find("Information").Find("BG_R").Find("BG")
                .GetComponent<RectTransform>();

            List<UniTask> tasks = new List<UniTask> {
                MoveBG(leftBG, leftBGMin, openDuration, openEasing),
                MoveBG(rightBG, rightBGMin, openDuration, openEasing)
            };
            await tasks;
        }
        public async UniTask CloseBG(CookingGame cookingGame) {

            RectTransform leftBG = cookingGame.transform.Find("Information").Find("BG_L").Find("BG")
                .GetComponent<RectTransform>();
            RectTransform rightBG = cookingGame.transform.Find("Information").Find("BG_R").Find("BG")
                .GetComponent<RectTransform>();

            List<UniTask> tasks = new List<UniTask> {
                MoveBG(leftBG, leftBGMax, closeDuration, closeEasing),
                MoveBG(rightBG, rightBGMax, closeDuration, closeEasing)
            };
            HideUI(cookingGame);
            await tasks;
        }

         void HideUI(CookingGame cookingGame) {
            CanvasGroup leftCanvasGroup = cookingGame.transform.Find("Information").Find("BG_L").Find("UI")
                .GetComponent<CanvasGroup>();
            CanvasGroup rightCanvasGroup = cookingGame.transform.Find("Information").Find("BG_R").Find("UI")
                .GetComponent<CanvasGroup>();
            DOTween.To(() => leftCanvasGroup.alpha, x => leftCanvasGroup.alpha = x, 0f, hideDuration).SetEase(Ease.Linear);
            DOTween.To(() => rightCanvasGroup.alpha, x => rightCanvasGroup.alpha = x, 0f, hideDuration).SetEase(Ease.Linear);
        }
        
        public async UniTask ChangeColor(CookingGame cookingGame) {
            Image leftBG = cookingGame.transform.Find("Information").Find("BG_L").Find("BG")
                .GetComponent<Image>();
            Image rightBG = cookingGame.transform.Find("Information").Find("BG_R").Find("BG")
                .GetComponent<Image>();
            leftBG.DOColor(shoppingBGColor, colorDuration).SetEase(Ease.Linear);
            rightBG.DOColor(shoppingBGColor, colorDuration).SetEase(Ease.Linear);
            await UniTask.Delay(TimeSpan.FromSeconds(closeDuration));
        }
        
        

        private async UniTask MoveBG(RectTransform BG, float toWidth, float duration, Ease ease) {
            float height = BG.sizeDelta.y;
            DOTween.To(
                () => BG.sizeDelta,
                x => BG.sizeDelta = x,
                new Vector2(toWidth, height),
                duration).SetEase(ease);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }

        public async UniTask GameEnd(Transform container, List<MissionStatus> missions, int star, int gainStar, int life, int lostLife, bool isClear) {
            await resultView.ShowResult(container, missions, star, gainStar, life, lostLife, isClear);
            while (true) {
                await UniTask.DelayFrame(1);
                if (Input.GetMouseButtonDown(0)) break;
            }
            container.gameObject.SetActive(false);
        }
    }
}
