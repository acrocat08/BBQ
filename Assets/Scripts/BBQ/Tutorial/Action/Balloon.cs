using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SoundMgr;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Tutorial.Action {
    [CreateAssetMenu(menuName = "Tutorial/Balloon")]
    public class Balloon : TutorialAction {
        [SerializeField] private float transformSpeed;
        [SerializeField] private float transformLength;
        [SerializeField] private float transformDuration;
        [SerializeField] private float whiteDuration;
        [SerializeField] private float waitDuration;
        [SerializeField] private float floatSpeed;
        [SerializeField] private float floatLength;
        [SerializeField] private Sprite balloonImage;
        [SerializeField] private Vector3 balloonPosition;
        [SerializeField] private Vector3 fukidashiPosition;

        public override async UniTask Exec(Transform container, string text, string takoEmotion, float value,
            IReceiver receiver) {
            Message(container).gameObject.SetActive(false);
            nowTween.Kill();
            Tween scaleTween = Tako(container).DOScale(Vector3.one * transformLength, transformSpeed);
            await UniTask.Delay(TimeSpan.FromSeconds(transformDuration));
            Image bgImage = GameObject.Find("WhiteBG").GetComponent<Image>();
            bgImage.DOColor(Color.white, whiteDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(whiteDuration));
            await UniTask.Delay(TimeSpan.FromSeconds(waitDuration));
            scaleTween.Kill();
            Tako(container).localScale = Vector3.one;
            Tako(container).localPosition = balloonPosition;
            Message(container).localPosition = fukidashiPosition;
            Tako(container).GetComponent<Image>().sprite = balloonImage;
            Tako(container).GetComponent<Image>().SetNativeSize();
            Tako(container).DOLocalMove(Tako(container).localPosition + Vector3.up * floatLength, floatSpeed)
                .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

        bgImage.DOColor(Color.clear, whiteDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(whiteDuration));

        }
        
        
        

    }
}