using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Tutorial.Action {
    [CreateAssetMenu(menuName = "Tutorial/Appear")]
    public class Appear : TutorialAction {
        [SerializeField] private float appearDuration;
        [SerializeField] private Vector3 toPos;
        
        public override async UniTask Exec(Transform container, string text, string takoEmotion, float value) {
            Message(container).gameObject.SetActive(false);
            container.DOLocalMove(toPos, appearDuration);
            Tako(container).GetComponent<Image>().DOColor(Color.white, appearDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(appearDuration));
        }

    }
}