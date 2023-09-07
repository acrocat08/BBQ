using BBQ.Database;
using UnityEngine;

namespace BBQ.Shopping {
    public class ShopItem : MonoBehaviour {
        private FoodData _foodData;
        [SerializeField] private ShopItemView view;
        

        public void Init(FoodData data) {
            _foodData = data;
            view.Draw(this);
        }

        public void Fall() {
            view.Fall(this);
        }

        public FoodData GetFoodData() {
            return _foodData;
        }
    }
}
