using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.Database;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Shopping {
    public class Reroller : MonoBehaviour {

        private Shop _shop;
        private Coin _coin;
        [SerializeField] private ShopItemChoice choice;

        private bool _isWaiting;

        private int _cost;

        public void Init(Shop shop, Coin coin) {
            _shop = shop;
            _coin = coin;
            _cost = 5;
            _isWaiting = false;
            transform.Find("Cost").GetComponent<Text>().text = _cost.ToString();
        }
        
        public async void Reroll() {
            _isWaiting = true;
            List<ShopItem> shopItems = _shop.GetShopItems();
            if(shopItems != null) _shop.DeleteItems(new List<ShopItem>(shopItems));
            List<FoodData> foods = choice.Choice(_shop.GetShopLevel());
            await _shop.AddItems(foods);
            _isWaiting = false;
        }
        
        public void OnClickRerollButton() {
            if(_isWaiting) return;
            if (_coin.GetCoin() < _cost) return;
            SoundPlayer.I.Play("se_reroll1");
            SoundPlayer.I.Play("se_reroll2");
            //_coin.Use(_cost);
            //_cost += 5;
            transform.Find("Cost").GetComponent<Text>().text = _cost.ToString();
            Reroll();
        }



    }
}
