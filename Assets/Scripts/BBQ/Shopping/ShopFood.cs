using System.Collections.Generic;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using UnityEngine;
using UnityEngine.Serialization;

namespace BBQ.Shopping {
    public class ShopFood : FoodObject {
        [SerializeField] private ShopItemView shopView;
        private ItemDetail itemDetail;
        

        private Shop _shop;

        public void Init(FoodData data, Shop shop, string areaTag, string targetTag, ItemDetail detail) {
            itemDetail = detail;
            deckFood = new DeckFood(data);
            _shop = shop;
            PointableArea area = transform.Find("Image").Find("Pointable").GetComponent<PointableArea>();
            area.areaTag = areaTag;
            area.targetTag = targetTag;
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
            itemDetail.DrawDetail(GetFoodData());
        }

        public void OnPointCancel() {
            //detailView.Clear(_detailContainer);
        }
        
        public void OnPointUp(List<PointableArea> areas) {
            if (InputGuard.Guard()) return;
            DeckInventory inventory = areas[0].transform.parent.parent.GetComponent<DeckInventory>();
            _shop.BuyFood(this, inventory);
        }
    }
}
