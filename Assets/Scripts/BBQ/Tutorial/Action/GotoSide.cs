using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Tutorial.Action {
    [CreateAssetMenu(menuName = "Tutorial/GotoSide")]
    public class GotoSide : TutorialAction {
        [SerializeField] private float duration;
        [SerializeField] private Vector3 toPos;
        
        public override async UniTask Exec(Transform container, string text, string takoEmotion, float value, IReceiver receiver) {
            Message(container).gameObject.SetActive(false);
            BG(container).GetComponent<Image>().enabled = false;
            TakoContainer(container).DOLocalMove(toPos, duration);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }

    }
}