using System.Collections.Generic;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using UnityEngine;

namespace BBQ.Shopping {
    public class ShopFood : FoodObject {
        [SerializeField] private ShopItemView shopView;
        [SerializeField] DetailView detailView;
        

        private Transform _detailContainer;
        private Shop _shop;

        public void Init(FoodData data, Shop shop, string areaTag, string targetTag, Transform detail) {
            deckFood = new DeckFood(data);
            _shop = shop;
            PointableArea area = transform.Find("Image").Find("Pointable").GetComponent<PointableArea>();
            area.areaTag = areaTag;
            area.targetTag = targetTag;
            _detailContainer = detail;
            area.onPointDown.AddListener(OnPointDown);
            area.onPointCancel.AddListener(OnPointCancel);
            area.onPointUp.AddListener(OnPointUp);
            shopView.DrawFood(this);
        }

        public void Fall() {
            shopView.Fall(transform.Find("Image").transform);
        }

        public override void Drop() {
            shopView.Drop(this);
        }

        public FoodData GetFoodData() {
            return deckFood.data;
        }

        public void OnPointDown() {
            detailView.DrawDetail(_detailContainer, GetFoodData());
        }

        public void OnPointCancel() {
            //detailView.Clear(_detailContainer);
        }
        
        public void OnPointUp(List<PointableArea> areas) {
            DeckInventory inventory = areas[0].transform.parent.parent.GetComponent<DeckInventory>();
            _shop.BuyFood(this, inventory);
        }
    }
}
