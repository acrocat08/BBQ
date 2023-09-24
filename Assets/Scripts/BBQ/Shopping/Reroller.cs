using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Action.Play;
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
        private int _rerollTicket;

        public void Init(Shop shop, Coin coin, int rerollTicket) {
            _shop = shop;
            _coin = coin;
            _rerollTicket = rerollTicket;
            _cost = 5;
            _isWaiting = false;
            Draw();
        }
        
        public async void Reroll() {
            _isWaiting = true;
            List<ShopFood> shopItems = _shop.GetShopItems();
            if(shopItems != null) _shop.DeleteItems(new List<ShopFood>(shopItems));
            List<FoodData> foods = choice.Choice(_shop.GetShopLevel());
            await _shop.AddItems(foods);
            _isWaiting = false;
        }
        
        public void OnClickRerollButton() {
            if(_isWaiting) return;
            if (_coin.GetCoin() < _cost) return;
            SoundPlayer.I.Play("se_reroll1");
            SoundPlayer.I.Play("se_reroll2");
            if (_rerollTicket > 0) {
                _rerollTicket--;
                SoundPlayer.I.Play("se_reroll3");
            }
            else {
                _coin.Use(_cost);
                _cost += 5;    
            }
            Reroll();
            Draw();
        }

        void Draw() {
            transform.Find("Cost").GetComponent<Text>().text = _rerollTicket > 0 ?  "0" : _cost.ToString();
            transform.Find("Onion").GetComponent<Image>().enabled = _rerollTicket > 0;
        }


    }
}
