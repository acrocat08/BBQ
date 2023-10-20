using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace BBQ.Tutorial {
    public class TutorialAction : ScriptableObject, IReceiver {
        public bool requireClick;

        public static string Signal;
        public static Tween nowTween;
        
        public virtual async UniTask Exec(Transform container, string text, string emotion, float value, IReceiver receiver) {
        }

        public void Receive(string signal) {
            Signal = signal;
        }

        protected Transform TakoContainer(Transform container) {
            return container.Find("Tako");
        }
        protected Transform BG(Transform container) {
            return container.Find("BG");
        }
        protected Transform Tako(Transform container) {
            return TakoContainer(container).Find("TakoImage");
        }
        protected Transform Message(Transform container) {
            return TakoContainer(container).Find("Fukidashi");
        }
        
    }
}
