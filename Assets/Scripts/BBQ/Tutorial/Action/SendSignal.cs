using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Tutorial.Action {
    [CreateAssetMenu(menuName = "Tutorial/SendSignal")]
    public class SendSignal : TutorialAction {
        
        public override async UniTask Exec(Transform container, string text, string takoEmotion, float value, IReceiver receiver) {
            receiver.Receive(text);
        }

    }
}