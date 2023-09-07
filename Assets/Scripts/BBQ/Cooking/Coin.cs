using UnityEngine;

namespace BBQ.Cooking {
    public class Coin : MonoBehaviour {
        [SerializeField] private CoinView view;

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
            Debug.Log(_nowCoin);
            view.AddCoin(this);
        }

        public int GetCoin() {
            return _nowCoin;
        }

    }
}
