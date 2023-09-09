using System;
using System.Collections.Generic;
using BBQ.Database;
using UnityEngine;
using System.Linq;
using BBQ.Common;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace BBQ.Shopping {
    public class Shop : MonoBehaviour {
        [SerializeField] private ShopItemFactory itemFactory;
        [SerializeField] private ShopView view;
        [SerializeField] private PointSensor pointSensor;
        [SerializeField] private Transform detailContainer;
        [SerializeField] private Reroller reroller;
        private Coin _coin;
        private List<ShopItem> _items;

        public void Init(Coin coin) {
            _coin = coin;
            reroller.Init(this, _coin);
            reroller.Reroll();
        }
        
        public async void BuyFood(ShopItem shopItem, DeckInventory inventory) {
            if (!CheckCanBuyFood(shopItem, _coin, inventory)) return;
            _coin.Use(shopItem.GetFoodData().cost);
            inventory.AddItem(shopItem.GetFoodData());
            DeleteItems(new List<ShopItem>{shopItem});            
            SoundPlayer.I.Play("se_buy");
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            pointSensor.UpdateArea();
        }

        bool CheckCanBuyFood(ShopItem shopItem, Coin coin, DeckInventory inventory) {
            if (coin.GetCoin() < shopItem.GetFoodData().cost) return false;
            return inventory.CheckIsEmpty();
        }


        public void DeleteItems(List<ShopItem> items) {
            if (_items == null) return;
            foreach (var item in items) {
                _items.Remove(item);
                Destroy(item.gameObject);
            }
        }
        
        public List<ShopItem> GetShopItems() {
            return _items;
        }

        public async UniTask AddItems(List<FoodData> data) {
            _items = data.Select(x => itemFactory.CreateFood(x, this, detailContainer)).ToList();
            view.PlaceItem(_items, transform);
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            pointSensor.UpdateArea();
        }

    }
}