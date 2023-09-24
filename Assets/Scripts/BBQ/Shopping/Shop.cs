using System;
using System.Collections.Generic;
using BBQ.Database;
using UnityEngine;
using System.Linq;
using BBQ.Action;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BBQ.Shopping {
    public class Shop : MonoBehaviour, IReleasable {
        [SerializeField] private ShopItemFactory itemFactory;
        [SerializeField] private ShopView view;
        [SerializeField] private PointSensor pointSensor;
        [SerializeField] private Transform detailContainer;
        [SerializeField] private Reroller reroller;
        private Coin _coin;
        private ShopFood[] _items;
        private int _level;
        private int _levelUpDiscount;
        [SerializeField] private List<int> levelUpCosts;

        public void Init(int level, int levelupDiscount, Coin coin, int rerollTicket) {
            _items = new ShopFood[] { null, null, null, null, null };
            _level = level;
            _levelUpDiscount = levelupDiscount;
            _coin = coin;
            reroller.Init(this, _coin, rerollTicket);
            reroller.Reroll();
            view.UpdateText(this, levelUpCosts[_level - 1] - _levelUpDiscount);
        }
        
        public async void BuyFood(ShopFood shopFood, DeckInventory inventory) {
            if (!CheckCanBuyFood(shopFood, _coin, inventory)) return;
            _coin.Use(shopFood.GetFoodData().cost);
            inventory.AddItem(shopFood.deckFood);
            DeleteItems(new List<ShopFood>{shopFood});            
            SoundPlayer.I.Play("se_buy");
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            await TriggerObserver.I.Invoke(ActionTrigger.Buy, new List<DeckFood>{shopFood.deckFood}, true);
            await TriggerObserver.I.Invoke(ActionTrigger.BuyOthers, new List<DeckFood>{shopFood.deckFood}, false);
            pointSensor.UpdateArea();
        }

        bool CheckCanBuyFood(ShopFood shopFood, Coin coin, DeckInventory inventory) {
            if (coin.GetCoin() < shopFood.GetFoodData().cost) return false;
            return inventory.CheckIsEmpty();
        }


        public void DeleteItems(List<ShopFood> items) {
            if (_items == null) return;
            for (int i = 0; i < 5; i++) {
                if (items.Contains(_items[i])) {
                    TriggerObserver.I.RemoveFood(_items[i].deckFood);
                    Destroy(_items[i].gameObject);
                    _items[i] = null;
                }
            }
        }
        
        public List<ShopFood> GetShopItems() {
            return _items.Where(x => x != null).ToList();
        }

        public async UniTask AddItems(List<FoodData> data) {
            await MoveItems();
            int cnt = 0;
            for (int i = 0; i < 5; i++) {
                if (cnt >= data.Count) break;
                if (_items[i] != null) continue;
                _items[i] = itemFactory.CreateFood(data[cnt], this, detailContainer);
                _items[i].deckFood.Releasable = this;
                view.PlaceItem(_items[i], i, transform);
                TriggerObserver.I.RegisterFood(_items[i].deckFood);
                cnt++;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            pointSensor.UpdateArea();
        }

        async UniTask MoveItems() {
            List<ShopFood> listed = _items.Where(x => x != null).ToList();
            if (listed.Count == 0) return;
            if (listed.Count == 5) {
                _items[0].Drop();
                _items[0] = null;
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                listed = _items.Where(x => x != null).ToList();
            }
            for (int i = 0; i < listed.Count; i++) {
                if(_items[i] == null) break;
                if (i == listed.Count - 1) return;
            }
            _items = new ShopFood[] { null, null, null, null, null };
            for (int i = 0; i < listed.Count; i++) {
                _items[i] = listed[i];
                view.MoveItem(_items[i], i, transform);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
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

        public List<FoodObject> ReleaseFoods(List<DeckFood> foods) {
            return new List<FoodObject>();
        }

        public FoodObject GetObject(DeckFood food) {
            return _items.FirstOrDefault(x => x.deckFood == food);
        }
    }
}