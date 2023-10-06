using System;
using System.Linq;
using BBQ.Common;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Tutorial.Action {
    [CreateAssetMenu(menuName = "Tutorial/WaitSignal")]
    public class WaitSignal : TutorialAction {

        
        public override async UniTask Exec(Transform container, string text, string takoEmotion, float value, IReceiver receiver) {
            Signal = "";
            while (Signal == "") {
                await UniTask.DelayFrame(1);
                if (Signal != "") {
                    break;
                }
            }
        }
        
        

    }
}