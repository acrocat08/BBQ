using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Tutorial {
    [CreateAssetMenu(menuName = "Tutorial/Player")]
    public class TutorialPlayer : ScriptableObject {

        public async UniTask Play(List<TutorialParts> parts, Transform tako) {
            int index = 0;
            while (index < parts.Count()) {
                TutorialParts tutorialParts = parts[index];
                InputGuard.Lock();
                await tutorialParts.action.Exec(tako, tutorialParts.message, tutorialParts.emotion, tutorialParts.value);
                InputGuard.UnLock();
                while (tutorialParts.action.requireClick && !Input.GetMouseButtonDown(0)) {
                    await UniTask.DelayFrame(1);
                }
                index++;
            }
        }

    }

    [Serializable]
    public class TutorialParts {
        public TutorialAction action;
        [Multiline]
        public string message;
        public string emotion;
        public float value;
    }
}