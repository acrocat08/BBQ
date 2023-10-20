using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SoundMgr;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Tutorial.Action {
    [CreateAssetMenu(menuName = "Tutorial/Shake")]
    public class Shake : TutorialAction {
        [SerializeField] private float speed;
        [SerializeField] private float length;
        
        public override async UniTask Exec(Transform container, string text, string takoEmotion, float value, IReceiver receiver) {
            Message(container).gameObject.SetActive(false);
            nowTween = Tako(container).DOShakePosition(speed, length, fadeOut:false).SetLoops(-1);
        }
        
        
        

    }
}