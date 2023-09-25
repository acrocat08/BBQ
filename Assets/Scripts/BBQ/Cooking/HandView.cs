using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace BBQ.Cooking {
    [CreateAssetMenu(menuName = "Hand/View")]
    public class HandView : ScriptableObject {

        [SerializeField] private float waitSecond;
        [SerializeField] private float turnDuration;
        [SerializeField] private Ease turnEase;
        [SerializeField] private float fallLength;
        [SerializeField] private float fallDuration;
        [SerializeField] private float jumpLength;
        [SerializeField] private Material goldMat;

        public async UniTask AfterHit(Hand hand) {
            await UniTask.Delay(TimeSpan.FromSeconds(waitSecond));

            int dir = hand.transform.localPosition.x > 0 ? 1 : -1;
            DOTween.Sequence()
                .Append(hand.transform.DOLocalRotate(new Vector3(0, 0, 180 * dir), turnDuration).SetEase(turnEase))
                .Join(hand.transform.DOLocalJump(hand.transform.localPosition + fallLength * Vector3.down,
                    jumpLength, 1, fallDuration));
            await UniTask.Delay(TimeSpan.FromSeconds(turnDuration));
        }

        public void Golden(Hand hand, bool isGold) {
            hand.GetComponent<Image>().material = isGold ? goldMat : null;
        }
    }
}