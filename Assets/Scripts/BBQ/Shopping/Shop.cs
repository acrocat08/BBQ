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
using UnityEngine.UI;

namespace BBQ.Shopping {
    public class Shop : MonoBehaviour {
        [SerializeField] private ShopItemFactory itemFactory;
        [SerializeField] private ShopView view;
        [SerializeField] private PointSensor pointSensor;
        [SerializeField] private Transform detailContainer;
        [SerializeField] private Reroller reroller;
        private Coin _coin;
        private List<ShopItem> _items;
        private int _level;
        private int _levelUpDiscount;
        [SerializeField] private List<int> levelUpCosts;

        public void Init(int level, int levelupDiscount, Coin coin) {
            _level = level;
            _levelUpDiscount = levelupDiscount;
            _coin = coin;
            reroller.Init(this, _coin);
            reroller.Reroll();
            view.UpdateText(this, levelUpCosts[_level - 1] - _levelUpDiscount);
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

        public int GetShopLevel() {
            return _level;
        }

        public int GetLevelUpDiscount() {
            return _levelUpDiscount;
        }

        void LevelUp() {
            int cost = levelUpCosts[_level - 1] - _levelUpDiscount;
            _coin.Use(cost);
            _level += 1;
            _levelUpDiscount = 0;
            view.UpdateText(this, levelUpCosts[_level - 1]);
        }

        public void OnLevelUpButtonClicked() {
            int cost = levelUpCosts[_level - 1] - _levelUpDiscount;
            if (_coin.GetCoin() < cost) return;
            SoundPlayer.I.Play("se_levelup");
            LevelUp();
        }
    }
}