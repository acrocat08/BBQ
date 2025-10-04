using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Tutorial.Action {
    [CreateAssetMenu(menuName = "Tutorial/Hide")]
    public class Hide : TutorialAction {
        [SerializeField] private float hideDuration;
        [SerializeField] private Vector3 toPos;
        
        public override async UniTask Exec(Transform container, string text, string takoEmotion, float value, IReceiver receiver) {
            Message(container).gameObject.SetActive(false);
            Tako(container).GetComponent<Image>().transform.localScale = new Vector3(-1, 1, 0);
            TakoContainer(container).DOLocalJump(toPos, 50f, 3, hideDuration).SetEase(Ease.Linear);
            Tako(container).GetComponent<Image>().transform.localScale = new Vector3(1, 1, 0);
            await UniTask.Delay(TimeSpan.FromSeconds(hideDuration));
        }

    }
}