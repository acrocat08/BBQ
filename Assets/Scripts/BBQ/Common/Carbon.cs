using BBQ.Cooking;
using UnityEngine;

namespace BBQ.Common {
    public class Carbon : MonoBehaviour {
        [SerializeField] private CarbonView view;

        private int _nowCarbon;

        public void Init(int carbon) {
            _nowCarbon = carbon;
            view.UpdateText(this);
        }

        public void Use(int mount) {
            _nowCarbon -= mount;
            view.UpdateText(this);
        }

        public void Add(int mount) {
            _nowCarbon += mount;
            view.AddCarbon(this);
        }

        public int GetCarbon() {
            return _nowCarbon;
        }

    }
}