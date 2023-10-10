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
        [SerializeField] private List<Color> textColor;
        [SerializeField] private bool isTutorial;
        [SerializeField] private List<FoodData> tutorialFirstFoods;
        [SerializeField] private List<FoodData> tutorialSecondFoods;
        [SerializeField] private ToolData tutorialTool;
        
        
        
        private int _cost;
        private int _rerollTicket;
        private bool _canClick;

        public void Init(Shop shop, Coin coin, int rerollTicket, bool canClick) {
            _shop = shop;
            _coin = coin;
            _rerollTicket = rerollTicket;
            _cost = 5;
            _canClick = canClick;
            Draw();
        }
        
        public async void Reroll(bool isFirst) {
            InputGuard.Lock();
            if(!isFirst) await TriggerObserver.I.Invoke(ActionTrigger.BeforeReroll, new List<DeckFood>(), false);
            List<FoodData> foods = choice.ChoiceFoods(_shop.GetShopLevel());
            ToolData tool = choice.ChoiceTool(_shop.GetShopLevel());
            List<UniTask> tasks = new List<UniTask>();

            if (isTutorial) foods = isFirst ? tutorialFirstFoods : tutorialSecondFoods;
            if (isTutorial) tool = tutorialTool;
            
            tasks.Add(_shop.AddFoods(foods, true));
            tasks.Add(_shop.AddTool(tool));
            await tasks;
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            if(!isFirst) await TriggerObserver.I.Invoke(ActionTrigger.AfterReroll, new List<DeckFood>(), false);
            InputGuard.UnLock();
        }
        
        public void OnClickRerollButton() {
            if (InputGuard.Guard()) return;
            if (_rerollTicket == 0 && _coin.GetCoin() < _cost) return;
            if (!_canClick) return;
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

        public void GainRerollTicket(int num) {
            _rerollTicket += num;
            Draw();
        }

        void Draw() {
            transform.Find("Cost").GetComponent<Text>().text = _rerollTicket > 0 ?  "" : _cost.ToString();
            transform.Find("Text").GetComponent<Text>().text = _rerollTicket > 0 ?  "Free!!" : "Reroll";
            transform.Find("Text").GetComponent<Text>().color = textColor[_rerollTicket > 0 ? 1 : 0];
            transform.Find("CoinImage").GetComponent<Image>().enabled = _rerollTicket == 0;
            transform.Find("Onion").GetComponent<Image>().enabled = _rerollTicket > 0;
            transform.Find("Avocado").GetComponent<Image>().enabled = _rerollTicket > 0 && _shop.GetShopLevel() >= 4;
            transform.Find("Asparagus").GetComponent<Image>().enabled = _rerollTicket > 0 && _shop.GetShopLevel() >= 5;
        }

        public void SetMode(bool mode) {
            _canClick = mode;

        }
    }
}
