using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Action;
using BBQ.Action.Play;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Shopping {
    public class Reroller : MonoBehaviour {

        private Shop _shop;
        private Coin _coin;
        [SerializeField] private ShopItemChoice choice;

        private int _cost;
        private int _rerollTicket;

        public void Init(Shop shop, Coin coin, int rerollTicket) {
            _shop = shop;
            _coin = coin;
            _rerollTicket = rerollTicket;
            _cost = 5;
            Draw();
        }
        
        public async void Reroll(bool isFirst) {
            InputGuard.Lock();
            if(!isFirst) await TriggerObserver.I.Invoke(ActionTrigger.BeforeReroll, new List<DeckFood>(), false);
            List<ShopFood> shopItems = _shop.GetShopFoods();
            if(shopItems != null) _shop.DeleteFoods(new List<ShopFood>(shopItems));
            List<FoodData> foods = choice.ChoiceFoods(_shop.GetShopLevel());
            ToolData tool = choice.ChoiceTool(_shop.GetShopLevel());
            List<UniTask> tasks = new List<UniTask>();
            tasks.Add(_shop.AddFoods(foods));
            tasks.Add(_shop.AddTool(tool));
            await tasks;
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            if(!isFirst) await TriggerObserver.I.Invoke(ActionTrigger.AfterReroll, new List<DeckFood>(), false);
            InputGuard.UnLock();
        }
        
        public void OnClickRerollButton() {
            if (InputGuard.Guard()) return;
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
            Reroll(false);
            Draw();
        }

        void Draw() {
            transform.Find("Cost").GetComponent<Text>().text = _rerollTicket > 0 ?  "0" : _cost.ToString();
            transform.Find("Onion").GetComponent<Image>().enabled = _rerollTicket > 0;
        }


    }
}
