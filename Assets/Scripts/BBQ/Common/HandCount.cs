using BBQ.Cooking;
using UnityEngine;

namespace BBQ.Common {
    public class HandCount : MonoBehaviour {
        [SerializeField] private HandCountView view;

        private int _nowHand;
        
        public void Init(int initialHand) {
            _nowHand = initialHand;
            view.UpdateText(this);
        }

        public void Use(int mount) {
            _nowHand -= mount;
            view.UpdateText(this);
        }

        public void Add(int mount) {
            _nowHand += mount;
            view.AddHand(this);
        }

        public int GetHandCount() {
            return _nowHand;
        }

    }
}
