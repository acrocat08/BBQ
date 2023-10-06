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
            TakoContainer(container).DOLocalMove(toPos, hideDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(hideDuration));
        }

    }
}