using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

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


        public void Init(CookingGame cookingGame) {
            RectTransform leftBG = cookingGame.transform.Find("Information").Find("BG_L").Find("BG")
                .GetComponent<RectTransform>();
            RectTransform rightBG = cookingGame.transform.Find("Information").Find("BG_R").Find("BG")
                .GetComponent<RectTransform>();
            float height = leftBG.sizeDelta.y;
            leftBG.sizeDelta = new Vector2(leftBGMax, height);
            rightBG.sizeDelta = new Vector2(rightBGMax, height);
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
            await tasks;
        }

        private async UniTask MoveBG(RectTransform BG, float toWidth, float duration, Ease ease) {
            float height = BG.sizeDelta.y;
            await DOTween.To(
                () => BG.sizeDelta,
                x => BG.sizeDelta = x,
                new Vector2(toWidth, height),
                duration).SetEase(ease);
        }
    }
}
