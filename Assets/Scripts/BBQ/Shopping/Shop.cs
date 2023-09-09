using System;
using System.Collections.Generic;
using BBQ.Database;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;
using SoundMgr;

namespace BBQ.Shopping {
    public class Shop : MonoBehaviour {
        [SerializeField] private ShopItemChoice choice;
        [SerializeField] private ShopItemFactory itemFactory;
        [SerializeField] private ShopView view;
        [SerializeField] private PointSensor pointSensor;
        [SerializeField] private Transform detailContainer;
        private List<ShopItem> _items;
        void Update() {
            if (Input.GetKeyDown(KeyCode.A)) {
                SoundPlayer.I.Play("se_reroll1");
                SoundPlayer.I.Play("se_reroll2");
                Reroll();
            }
        }

        void Start() {
            Reroll();
        }

        async void Reroll() {
            if (_items != null) {
                foreach (var item in _items) {
                    Destroy(item.gameObject);
                }
                _items = null;
            }
            List<FoodData> foods = choice.Choice(1);
            _items = foods.Select(x => itemFactory.CreateFood(x, this, detailContainer)).ToList();
            view.PlaceItem(_items, transform);
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            pointSensor.UpdateArea();
            
        }

        public async void BuyFood(ShopItem shopItem, DeckInventory inventory) {
            inventory.AddItem(shopItem.GetFoodData());
            Destroy(shopItem.gameObject);
            _items.Remove(shopItem);
            SoundPlayer.I.Play("se_buy");
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            pointSensor.UpdateArea();
        }

    }
}
