using BBQ.Cooking;
using UnityEngine;

namespace BBQ.Common {
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
            view.AddCoin(this);
        }

        public int GetCoin() {
            return _nowCoin;
        }

    }
}
