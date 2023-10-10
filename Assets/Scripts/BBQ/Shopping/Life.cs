using UnityEngine;

namespace BBQ.Shopping {
    public class Life : MonoBehaviour {
        [SerializeField] private LifeView view;

        private int _nowCoin;

        public void Init(int coin) {
            _nowCoin = coin;
            view.UpdateText(this);
        }

        public void Use(int mount) {
            _nowCoin -= mount;
            view.UpdateText(this);
        }

        public void Add(int mount) {
            _nowCoin += mount;
            view.AddLife(this);
        }

        public int GetLife() {
            return _nowCoin;
        }

    }
}