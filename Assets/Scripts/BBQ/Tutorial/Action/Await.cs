using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Tutorial.Action {
    [CreateAssetMenu(menuName = "Tutorial/Await")]
    public class Await : TutorialAction {
        
        public override async UniTask Exec(Transform container, string text, string takoEmotion, float value) {
            Message(container).gameObject.SetActive(false);
            await UniTask.Delay(TimeSpan.FromSeconds(value));
        }

    }
}