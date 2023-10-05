using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Tutorial {
    public class TutorialAction : ScriptableObject {
        public bool requireClick;

        public virtual async UniTask Exec(Transform container, string text, string emotion, float value) {
        }

        protected Transform Tako(Transform container) {
            return container.Find("TakoImage");
        }
        protected Transform Message(Transform container) {
            return container.Find("Fukidashi");
        }
        
    }
}
