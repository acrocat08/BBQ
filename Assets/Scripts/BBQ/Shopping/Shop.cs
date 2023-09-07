using System.Collections.Generic;
using BBQ.Database;
using UnityEngine;
using System.Linq;
using SoundMgr;

namespace BBQ.Shopping {
    public class Shop : MonoBehaviour {
        [SerializeField] private ShopItemChoice choice;
        [SerializeField] private ShopItemFactory itemFactory;
        [SerializeField] private ShopView view;
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

        void Reroll() {
            if (_items != null) {
                foreach (var item in _items) {
                    Destroy(item.gameObject);
                }
                _items = null;
            }
            List<FoodData> foods = choice.Choice(1);
            _items = foods.Select(x => itemFactory.Create(x)).ToList();
            view.PlaceItem(_items, transform);
            

        }

    }
}
