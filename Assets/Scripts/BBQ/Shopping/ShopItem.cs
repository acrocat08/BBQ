using BBQ.Database;
using UnityEngine;

namespace BBQ.Shopping {
    public class ShopItem : MonoBehaviour {
        private FoodData _foodData;
        [SerializeField] private ShopItemView view;
        [SerializeField] DetailView detailView;
        

        private Transform _detailContainer;
        private Shop _shop;

        public void Init(FoodData data, Shop shop, string areaTag, string targetTag, Transform detail) {
            _foodData = data;
            _shop = shop;
            PointableArea area = transform.Find("Food").Find("Pointable").GetComponent<PointableArea>();
            area.areaTag = areaTag;
            area.targetTag = targetTag;
            _detailContainer = detail;
            area.onPointDown.AddListener(OnPointDown);
            area.onPointCancel.AddListener(OnPointCancel);
            area.onPointUp.AddListener(OnPointUp);
            view.Draw(this);
        }

        public void Fall() {
            view.Fall(this);
        }

        public FoodData GetFoodData() {
            return _foodData;
        }

        public void OnPointDown() {
            detailView.DrawDetail(_detailContainer, _foodData);
        }

        public void OnPointCancel() {
            //detailView.Clear(_detailContainer);
        }
        
        public void OnPointUp(PointableArea area) {
            DeckInventory inventory = area.transform.parent.parent.GetComponent<DeckInventory>();
            _shop.BuyFood(this, inventory);
        }
    }
}
