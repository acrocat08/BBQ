using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Tutorial {
    [CreateAssetMenu(menuName = "Tutorial/Player")]
    public class TutorialPlayer : ScriptableObject {

        private int _index;
        private TutorialParts _nowAction;
        [SerializeField] private TutorialAction ifAction;
        [SerializeField] private TutorialAction retryAction;

        public async UniTask Play(List<TutorialParts> parts, Transform tako, IReceiver receiver) {
            _index = 0;
            TutorialAction.Signal = "";
            while (_index < parts.Count()) {
                _nowAction = parts[_index];
                if (_nowAction.action == ifAction) {
                    if (TutorialAction.Signal == "success") {
                        _index += (int)_nowAction.value;
                        _nowAction = parts[_index];
                    }
                }
                if (_nowAction.action == retryAction) {
                    _index -= (int)_nowAction.value;
                    _nowAction = parts[_index];
                }
                InputGuard.Lock();
                await _nowAction.action.Exec(tako, _nowAction.message, _nowAction.emotion, _nowAction.value, receiver);
                InputGuard.UnLock();
                while (_nowAction.action.requireClick && !Input.GetMouseButtonDown(0)) {
                    await UniTask.DelayFrame(1);
                }
                _index++;
            }
        }

        public void Send(string signal) {
            _nowAction.action.Receive(signal);
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